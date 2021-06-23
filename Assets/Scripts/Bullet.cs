using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
  [SerializeField] float allowedTravelDistance;
  Rigidbody2D rb;
  SpriteRenderer spriteRenderer;
  GameObject whoShotMe;

  Vector3 bulletOrigin;
  Vector3 previousPos;
  float distanceTravelled;
    
    void Awake()
    {
      rb = GetComponent<Rigidbody2D>();
      spriteRenderer = GetComponent<SpriteRenderer>();
      previousPos = transform.position;
    }

    private void OnEnable() {
      previousPos = transform.position;
    }

    public void SetBulletVelocity(Vector2 velocity) {
      rb.velocity = velocity;
    }

    public void SetBulletColor(Color color) {
      spriteRenderer.color = color;
    }

    public void SetBulletShooter(GameObject shooter) {
      whoShotMe = shooter;
    }

    public void SetBulletOrigin(Vector3 origin) {
      transform.position = origin;
      bulletOrigin = origin;
    }

    public GameObject GetBulletShooter() {
      return whoShotMe;
    }

    public void SetBulletRange(float range) {
      allowedTravelDistance = range;
    }

    // private void DistanceTravelled() {
    //   distanceTravelled = distanceTravelled + Vector3.Distance(transform.position, previousPos);
    //   previousPos = transform.position;
    // }

    private void Update() {
      // DistanceTravelled();
      // DisableBullet();
    }
    

    // private void DisableBullet() {
    //   if (distanceTravelled >= allowedTravelDistance) {
    //     distanceTravelled = 0;
    //     gameObject.SetActive(false);
    //     ObjectPool.SharedInstance.ReturnToPool("Bullets", gameObject);
    //   }
    // }

}
