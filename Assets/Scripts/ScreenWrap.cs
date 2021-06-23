using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
  void FixedUpdate()
  {
    Vector3 currentViewportPosition = Camera.main.WorldToViewportPoint(transform.position);
    if (currentViewportPosition.x < 0)
    {
      // teleport object to the right
      currentViewportPosition = new Vector3(1f, currentViewportPosition.y, currentViewportPosition.z);
      transform.position = Camera.main.ViewportToWorldPoint(currentViewportPosition);
    }

    if (currentViewportPosition.x > 1)
    {
      // teleport object to the left
      currentViewportPosition = new Vector3(0f, currentViewportPosition.y, currentViewportPosition.z);
      transform.position = Camera.main.ViewportToWorldPoint(currentViewportPosition);
    }

    if (currentViewportPosition.y < 0)
    {
      // teleport object up
      currentViewportPosition = new Vector3(currentViewportPosition.x, 1f, currentViewportPosition.z);
      transform.position = Camera.main.ViewportToWorldPoint(currentViewportPosition);
    }
    if (currentViewportPosition.y > 1)
    {
      // teleport object down
      currentViewportPosition = new Vector3(currentViewportPosition.x, 0f, currentViewportPosition.z);
      transform.position = Camera.main.ViewportToWorldPoint(currentViewportPosition);
    }
  }
}
