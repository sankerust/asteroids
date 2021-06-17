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
  PlayerController player;
  AudioSource audioSource;

  private void Start() {
    rb = GetComponent<Rigidbody2D>();
    audioSource = GetComponent<AudioSource>();
    player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    ufoSpeed = Screen.width / 100f / 10f;
    Invoke("Spawn", Random.Range(spawnRateFloor, spawnRateCeil));
  }


  private void Spawn()
  {
    int randomSide = Random.Range(0, 2); // 0 for left side, 1 for right side
    float randomAxisPos = Random.Range(0.2f, 0.8f); // 20% of min height & 80% of max height
    transform.position = Camera.main.ViewportToWorldPoint(new Vector3(randomSide, randomAxisPos, 10f));
    EnableUfo(true);

    // set in motion
    if (randomSide == 0) {
      rb.velocity = transform.TransformDirection(Vector2.right * ufoSpeed);
    } else {
      rb.velocity = transform.TransformDirection(Vector2.left * ufoSpeed);
    }
    
  }

  private void OnCollisionEnter2D(Collision2D other) {
    if (other.gameObject.tag == "Bullet") {
      other.gameObject.SetActive(false);
      if (other.gameObject.GetComponent<SpriteRenderer>().color == Color.green) {
        player.IncreaseScore(200);
      }
    }
    EnableUfo(false);
    Invoke("Spawn", Random.Range(spawnRateFloor, spawnRateCeil));
  }

  private void EnableUfo(bool state)
  {
    GetComponent<SpriteRenderer>().enabled = state;
    GetComponent<BoxCollider2D>().enabled = state;
    isAlive = state;
  }

  IEnumerator ShootAtPlayer() {
    canShoot = false;
    float randomShotDelay = Random.Range(2f, 5.1f);

    GameObject bullet = ObjectPool.SharedInstance.GetPooledObject("Bullets");

    if (bullet != null)
    {
      bullet.GetComponent<SpriteRenderer>().color = Color.red;
      audioSource.PlayOneShot(ufoShootSound);
      bullet.SetActive(true);
      bullet.transform.position = transform.position + (player.transform.position - gameObject.transform.position).normalized;
      bullet.GetComponent<Rigidbody2D>().velocity = (player.transform.position - bullet.transform.position).normalized * bulletSpeed;
      StartCoroutine(DisableBullet(bullet));
    }

    yield return new WaitForSeconds(randomShotDelay);
    canShoot = true;
  }

  IEnumerator DisableBullet(GameObject bullet) {
    float bulletLifeTime = Screen.width / bullet.GetComponent<Rigidbody2D>().velocity.magnitude / 100f;
    yield return new WaitForSeconds(bulletLifeTime);
    bullet.SetActive(false);
    ObjectPool.SharedInstance.ReturnToPool("Bullets", bullet);
  }

  private void Update() {
    if (canShoot && isAlive) {
      StartCoroutine(ShootAtPlayer());
    }
    if (isAlive) {
      FlewAway();
    }
  }

private void FlewAway() {
    Vector3 currentViewportPosition = Camera.main.WorldToViewportPoint(transform.position);
    if (currentViewportPosition.x > 1 || currentViewportPosition.x < 0) {
      EnableUfo(false);
      Invoke("Spawn", Random.Range(spawnRateFloor, spawnRateCeil));
    }
}
  
}
