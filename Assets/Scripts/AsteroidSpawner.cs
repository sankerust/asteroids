using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
  [SerializeField] int amountToSpawn = 2;
  [SerializeField] float speedCeiling = 200f;
  [SerializeField] float speedFloor = 50f;

    private void Start()
    {
        CheckEnvironment();
    }

    private Vector3 PositionRandomizer() {
      int randomAxis = Random.Range(0, 2);
      float randomAxisPos = Random.Range(0f, 1f);
      Vector3 randomSpawnPos;
      switch (randomAxis)
      {
          case 0:
            randomSpawnPos = new Vector3(randomAxis, randomAxisPos, 10f);
            return Camera.main.ViewportToWorldPoint(randomSpawnPos);
          case 1:
            randomSpawnPos = new Vector3(randomAxisPos, randomAxis, 10f);
            return Camera.main.ViewportToWorldPoint(randomSpawnPos);
      }
      return transform.position;
    }

  private void Spawn() {
    int i = amountToSpawn;
    while (i > 0)
    {
      i--;
      GameObject asteroid = ObjectPool.SharedInstance.GetPooledObject("Asteroids");
      asteroid.transform.localScale = new Vector3(2f, 2f, 1f);
      asteroid.transform.position = PositionRandomizer();
      asteroid.SetActive(true);
      asteroid.GetComponent<Asteroid>().ResetSize();
      SetInMotion(asteroid);
    }
    amountToSpawn++;
    }

  private void SetInMotion(GameObject asteroid)
  {
    Rigidbody2D rb = asteroid.GetComponent<Rigidbody2D>();
    float random = Random.Range(0f, 260f);
    Vector2 randomDirection = new Vector2(Mathf.Cos(random), Mathf.Sin(random));
    rb.AddForce(randomDirection * Random.Range(speedFloor, speedCeiling));
  }

  public void CheckEnvironment()
  {
    if (!GameObject.FindWithTag("Asteroid"))
    {
      Invoke ("Spawn", 2f);
    };
  }

}
