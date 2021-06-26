using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
  Camera cam;

  private void Awake() {
    cam = Camera.main;
  }

  void FixedUpdate()
  {
    Vector3 currentViewportPosition = cam.WorldToViewportPoint(transform.position);
    if (currentViewportPosition.x < 0)
    {
      // teleport object to the right
      currentViewportPosition = new Vector3(1f, currentViewportPosition.y, currentViewportPosition.z);
      transform.position = cam.ViewportToWorldPoint(currentViewportPosition);
    }

    if (currentViewportPosition.x > 1)
    {
      // teleport object to the left
      currentViewportPosition = new Vector3(0f, currentViewportPosition.y, currentViewportPosition.z);
      transform.position = cam.ViewportToWorldPoint(currentViewportPosition);
    }

    if (currentViewportPosition.y < 0)
    {
      // teleport object up
      currentViewportPosition = new Vector3(currentViewportPosition.x, 1f, currentViewportPosition.z);
      transform.position = cam.ViewportToWorldPoint(currentViewportPosition);
    }
    if (currentViewportPosition.y > 1)
    {
      // teleport object down
      currentViewportPosition = new Vector3(currentViewportPosition.x, 0f, currentViewportPosition.z);
      transform.position = cam.ViewportToWorldPoint(currentViewportPosition);
    }
  }
}
