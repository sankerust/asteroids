using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
  [SerializeField] Text lifesLeftText;
  [SerializeField] Text scoreText;
  [SerializeField] Text pauseText;

  PlayerController player;

  private void Awake() {
    player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
  }

  public void UpdateScore() {
    scoreText.text = "Score: " + player.GetPlayerScore();
  }

  public void UpdateLifes() {
    lifesLeftText.text = "Lifes left: " + player.GetPlayerLifes();
  }

}
