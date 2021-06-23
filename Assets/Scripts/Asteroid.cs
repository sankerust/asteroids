using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
  GameObject player;
  PlayerController playerController;
  Rigidbody2D rb;
  AsteroidSpawner asteroidSpawner;
  Size size;
  enum Size {big, medium, small};

  void Start()
  {
    asteroidSpawner = GameObject.FindWithTag("GameController").GetComponent<AsteroidSpawner>();
    player = GameObject.FindWithTag("Player");
    playerController = player.GetComponent<PlayerController>();
    rb = GetComponent<Rigidbody2D>();
  }

  private void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.CompareTag("Asteroid")) {
      return;
    }
    
    if (other.gameObject.GetComponent<Bullet>() != null && other.gameObject.GetComponent<Bullet>().GetBulletShooter() == player) {
        size--;
        IncreasePlayerScore();
      if (size == Size.big || size == Size.medium)
        {
          CrackAsteroidInTwo();
        }
    }
    gameObject.SetActive(false);
    ObjectPool.SharedInstance.ReturnToPool("Asteroids", gameObject);
    asteroidSpawner.CheckEnvironment();
  }

  private void CrackAsteroidInTwo()
  {
    Vector2 firstAsteroidSpawnPos = transform.position + GetAngleDirectionVector(45f);
    Vector2 secondAsteroidSpawnPos = transform.position + GetAngleDirectionVector(-45f);
    float randomChildrenSpeed = Random.Range(1f, 1.8f);

    SpawnSmallerAsteroid(firstAsteroidSpawnPos, GetAngleDirectionVector(45f) * randomChildrenSpeed);
    SpawnSmallerAsteroid(secondAsteroidSpawnPos, GetAngleDirectionVector(-45f) * randomChildrenSpeed);
  }

  private void IncreasePlayerScore()
  {
    switch (transform.localScale.x)
    {
        case 2f:
          size = Size.big;
          break;
        case 1f:
          size = Size.medium;
          break;
        case 0.5f:
          size = Size.small;
          break;
      }
    switch (size)
    {
      case Size.big:
        playerController.IncreaseScore(20);
        break;
      case Size.medium:
        playerController.IncreaseScore(50);
        break;
      case Size.small:
        playerController.IncreaseScore(100);
        break;
    }
  }

  private void SpawnSmallerAsteroid(Vector3 smallerAsteroidSpawnPosition, Vector2 smallerAsteroidMoveDirection) {
    GameObject smallerAsteroid;
    smallerAsteroid = ObjectPool.SharedInstance.GetPooledObject("Asteroids");
    
    smallerAsteroid.transform.position = smallerAsteroidSpawnPosition;
    smallerAsteroid.transform.localScale = transform.localScale * 0.5f;

    smallerAsteroid.SetActive(true);
    smallerAsteroid.GetComponent<Rigidbody2D>().velocity = smallerAsteroidMoveDirection;
  }

  private Vector3 GetAngleDirectionVector(float angle)
  {
    Vector2 rotatedPoint;
    Vector2 fatherVelocity = rb.velocity.normalized;
    rotatedPoint.x = fatherVelocity.x * Mathf.Cos(angle) - fatherVelocity.y * Mathf.Sin(angle);
    rotatedPoint.y = fatherVelocity.x * Mathf.Sin(angle) + fatherVelocity.y * Mathf.Cos(angle);
    return rotatedPoint;
  }

  public void ResetSize() {
    size = Size.big;
  }
}
