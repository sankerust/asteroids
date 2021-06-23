using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
  [SerializeField] float acceleration = 2f;
  [SerializeField] float rotation = 200f;
  [SerializeField] float bulletSpeed = 5f;
  [SerializeField] float maxSpeed = 10f;
  [SerializeField] AudioClip shootSound;
  [SerializeField] AudioClip deathSound;
  [SerializeField] AudioClip thrustSound;
  [SerializeField] AudioClip killSound;
  [SerializeField] UIController uiController;

  private int lifesLeft, score, shotsMade, controlScheme;
  private bool canShoot;
  private float timeSinceLastShot, lastShotTime;
  
  Rigidbody2D rb;
  AudioSource audioSource;
  Weapon weapon;
  

  void Start()
  {
    CacheReferences();
    lifesLeft = 3;
    score = 0;
    uiController.UpdateScore();
    uiController.UpdateLifes();
    shotsMade = 0;
    canShoot = true;
  }

  private void CacheReferences()
  {
    rb = GetComponent<Rigidbody2D>();
    audioSource = GetComponent<AudioSource>();
    weapon = GetComponent<Weapon>();
    uiController = uiController.GetComponent<UIController>();
  }

  public void SetControlScheme(int whichOne)
  {
    controlScheme = whichOne;
  }

  public int GetPlayerScore() {
    return score;
  }

  public int GetPlayerLifes() {
    return lifesLeft;
  }

  void Update()
  {
    if (!GameSettings.gamePaused && canShoot) {
      ProcessShootInput();
    }

    if (shotsMade >= 3) {
      StartCoroutine(reloadMag());
    }

    timeSinceLastShot = Time.time - lastShotTime;
    if (timeSinceLastShot >= 1f) {
      shotsMade = 0;
    }

    if (lifesLeft == 0) {
      GameSettings.gamePaused = true;
    }
  }

  public void IncreaseScore(int increaseAmount)
  {
    audioSource.PlayOneShot(killSound);
    score += increaseAmount;
    uiController.UpdateScore();
  }

  void FixedUpdate()
  {
    Accelerate();
    Rotate();
  }

  private void ProcessShootInput()
  {
    if (controlScheme == 0) {
      if (Input.GetKeyDown(KeyCode.Space))
      {
        Shoot();
      }
    }
    
    if (controlScheme == 1) {
      if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
      {
        Shoot();
      }
    }
  }

  private void Shoot()
  {
    lastShotTime = Time.time;
    shotsMade++;
    Vector3 origin = transform.position + transform.up;
    Vector2 velocity = transform.TransformDirection(Vector2.up * bulletSpeed);
    Color color = Color.green;

    weapon.ShootBullet(origin, velocity, color, this.gameObject, shootSound);
  }

  private IEnumerator reloadMag()
  {
    canShoot = false;
    shotsMade = 0;
    yield return new WaitForSeconds(1f);
    canShoot = true;
  }

  private void Rotate()
  {
    if (Input.GetAxis("Horizontal") != 0 && controlScheme == 0) {
      transform.Rotate(0, 0, -Input.GetAxis("Horizontal") * Time.deltaTime * rotation);
    }

    if (controlScheme == 1)
    {
      Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      Quaternion rotateTowards = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
      transform.rotation = Quaternion.RotateTowards(transform.rotation, rotateTowards, rotation * Time.deltaTime);
    }
    
  }

  private void Accelerate()
  {
    if(controlScheme == 1 && Input.GetMouseButton(1))
    {
      rb.AddForce(transform.up * acceleration);
      PlayAccelerationSound();
    }

    if (Input.GetAxis("Vertical") > 0) {
      rb.AddForce(transform.up * acceleration * Input.GetAxis("Vertical"));
      PlayAccelerationSound();
    }

    if (rb.velocity.magnitude > maxSpeed)
    {
      rb.velocity = rb.velocity.normalized * maxSpeed;
    }
    
  }

  private void PlayAccelerationSound()
  {
    if (!audioSource.isPlaying)
    {
      audioSource.PlayOneShot(thrustSound);
    }
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    Respawn();
  }

  private void Respawn()
  {
    audioSource.PlayOneShot(deathSound);
    if (lifesLeft > 0) {
      lifesLeft--;
      uiController.UpdateLifes();
      transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f,10f));
      StartCoroutine(Invincible());
      StartCoroutine(Blink(3f));
    } else {
      gameObject.SetActive(false);
    }
  }

  private IEnumerator Blink(float respawnTime)
  {
    float endTime = Time.time + respawnTime;
    while (Time.time < endTime) {
      GetComponent<SpriteRenderer>().enabled = false;
      yield return new WaitForSeconds(0.5f);
      GetComponent<SpriteRenderer>().enabled = true;
      yield return new WaitForSeconds(0.5f);
    }

  }

  private IEnumerator Invincible()
  {
    GetComponent<PolygonCollider2D>().enabled = false;
    yield return new WaitForSeconds(3f);
    GetComponent<PolygonCollider2D>().enabled = true;
  }
}
