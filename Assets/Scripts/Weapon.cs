using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
  GameObject whoShot;

  [SerializeField] float weaponRange = 40f;

  public void ShootBullet(Vector3 origin, Vector2 velocity, Color color, GameObject shooter)
  {
    GameObject bullet = ObjectPool.SharedInstance.GetPooledObject("Bullets");
    if (bullet != null)
    {
      Bullet bulletScript = bullet.GetComponent<Bullet>();

      bullet.SetActive(true);
      bulletScript.SetBulletColor(color);
      bulletScript.SetBulletOrigin(origin);
      bulletScript.SetBulletVelocity(velocity);
      bulletScript.SetBulletShooter(shooter);
      //bulletScript.SetBulletRange(weaponRange);
      StartCoroutine(DisableBullet(bullet));
    }
  }

  private IEnumerator DisableBullet(GameObject bullet)
  {
    float lifeTime = Screen.width / bullet.GetComponent<Rigidbody2D>().velocity.magnitude / 100f;
    yield return new WaitForSeconds(lifeTime);
    bullet.SetActive(false);
    ObjectPool.SharedInstance.ReturnToPool("Bullets", bullet);
  }
}
