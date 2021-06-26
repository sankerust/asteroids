using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
  [SerializeField] Text lifesLeftText;
  [SerializeField] Text scoreText;
  [SerializeField] Text pauseText;
  [SerializeField] GameObject continueButton;
  [SerializeField] GameObject newGameButton;
  [SerializeField] GameObject controlSwitchButton;
  [SerializeField] GameObject quitButton;

  PlayerController player;

  private void Awake()
  {
    player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    UpdateSwitchButton();
  }

  public void UpdateScore()
  {
    scoreText.text = "Score: " + player.GetPlayerScore();
  }

  public void UpdateLifes()
  {
    lifesLeftText.text = "Lifes left: " + player.GetPlayerLifes();
  }

  public void NewGameButtonActive(bool state)
  {
    newGameButton.SetActive(state);
  }

  public void ContinueButtonActive(bool state)
  {
    continueButton.SetActive(state);
  }

  public void UpdateSwitchButton()
  {
    controlSwitchButton.GetComponentInChildren<Text>().text = GetControlSchemeString();
  }

  private string GetControlSchemeString()
  {
    int controlScheme = GameSettings.controlScheme;
    if (controlScheme == 0)
    {
      return "Current control scheme: Rotate + accelerate: wad/arrows; Shoot: space";
    }
    
    if (controlScheme == 1)
    {
      return "Current control scheme: Mouselook; Fire: LMB,Space; Accelerate: RMB,W,UP";
    }
    else
    {
      return "no control scheme defined";
    }
  }

  public void EnablePauseMenu(bool state)
  {
    ContinueButtonActive(state);
    pauseText.enabled = state;
    NewGameButtonActive(state);
    quitButton.SetActive(state);
    controlSwitchButton.SetActive(state);
  }
}
