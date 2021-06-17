using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
  public void ShootBullet(Vector3 origin, Vector2 velocity, Color color)
  {
    GameObject bullet = ObjectPool.SharedInstance.GetPooledObject("Bullets");
    if (bullet != null)
    {
      bullet.SetActive(true);
      bullet.GetComponent<SpriteRenderer>().color = color;
      bullet.transform.position = origin;
      bullet.GetComponent<Rigidbody2D>().velocity = velocity;
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
