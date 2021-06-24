using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFO : MonoBehaviour
{
  [SerializeField] float spawnRateCeil = 8f;
  [SerializeField] float spawnRateFloor = 2f;
  [SerializeField] float bulletSpeed = 5f;
  [SerializeField] AudioClip deathSound;
  [SerializeField] AudioClip ufoShootSound;

  float spawnRate, ufoSpeed, lastShotTime;
  bool canShoot = true;
  bool isAlive;

  Rigidbody2D rb;
  GameObject player;
  PlayerController playerController;
  AudioSource audioSource;
  Weapon weapon;
  SpriteRenderer spriteRenderer;
  BoxCollider2D boxCollider2D;
  Camera cam;

  private void Awake()
  {
    CacheReferences();
    ufoSpeed = Screen.width / 100f / 10f;
    Invoke("Spawn", Random.Range(spawnRateFloor, spawnRateCeil));
  }

  private void CacheReferences()
  {
    rb = GetComponent<Rigidbody2D>();
    audioSource = GetComponent<AudioSource>();
    weapon = GetComponent<Weapon>();
    spriteRenderer = GetComponent<SpriteRenderer>();
    boxCollider2D = GetComponent<BoxCollider2D>();
    player = GameObject.FindWithTag("Player");
    playerController = player.GetComponent<PlayerController>();
    cam = Camera.main;
  }

  private void Spawn()
  {
    int randomSide = Random.Range(0, 2); // 0 for left side, 1 for right side
    float randomAxisPos = Random.Range(0.2f, 0.8f); // 20% of min height & 80% of max height
    transform.position = cam.ViewportToWorldPoint(new Vector3(randomSide, randomAxisPos, 10f));
    EnableUfo(true);

    // set in motion
    if (randomSide == 0)
    {
      rb.velocity = transform.TransformDirection(Vector2.right * ufoSpeed);
    }
    else
    {
      rb.velocity = transform.TransformDirection(Vector2.left * ufoSpeed);
    }
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.GetComponent<Bullet>() != null && other.gameObject.GetComponent<Bullet>().GetBulletShooter() == player)
    {
      playerController.IncreaseScore(200);
    }
    EnableUfo(false);
    Invoke("Spawn", Random.Range(spawnRateFloor, spawnRateCeil));
  }

  private void EnableUfo(bool state)
  {
    spriteRenderer.enabled = state;
    boxCollider2D.enabled = state;
    isAlive = state;
  }

  private IEnumerator ShootAtPlayer()
  {
    canShoot = false;
    float randomShotDelay = Random.Range(2f, 5.1f);

    Vector3 origin = transform.position + (playerController.transform.position - gameObject.transform.position).normalized;
    Vector2 velocity = (playerController.transform.position - origin).normalized * bulletSpeed;
    Color color = Color.red;

    weapon.ShootBullet(origin, velocity, color, this.gameObject, ufoShootSound);

    yield return new WaitForSeconds(randomShotDelay);
    canShoot = true;
  }

  private void Update()
  {
    if (canShoot && isAlive)
    {
      StartCoroutine(ShootAtPlayer());
    }
    if (isAlive)
    {
      FlyAway();
    }
  }

  private void FlyAway()
  {
    Vector3 currentViewportPosition = cam.WorldToViewportPoint(transform.position);
    if (currentViewportPosition.x > 1 || currentViewportPosition.x < 0)
    {
      EnableUfo(false);
      Invoke("Spawn", Random.Range(spawnRateFloor, spawnRateCeil));
    }
  }
}
