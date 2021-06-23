using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class Weapon : MonoBehaviour
{
  GameObject whoShot;
  AudioSource audioSource;

  [SerializeField] float weaponRange = 40f;

  private void Awake()
  {
    audioSource = GetComponent<AudioSource>();
  }

  public void ShootBullet(Vector3 origin, Vector2 velocity, Color color, GameObject shooter, AudioClip shootSound)
  {
    GameObject bullet = ObjectPool.SharedInstance.GetPooledObject("Bullets");
    if (bullet != null)
    {
      Bullet bulletScript = bullet.GetComponent<Bullet>();
      audioSource.PlayOneShot(shootSound);

      bullet.SetActive(true);
      bulletScript.SetBulletColor(color);
      bulletScript.SetBulletOrigin(origin);
      bulletScript.SetBulletVelocity(velocity);
      bulletScript.SetBulletShooter(shooter);
      bulletScript.SetBulletRange(weaponRange);
    }
  }
}
