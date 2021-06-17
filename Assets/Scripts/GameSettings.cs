using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{
  public static bool gamePaused = true;

  [SerializeField] Text pauseText;
  [SerializeField] GameObject continueGameButton;
  [SerializeField] GameObject newGameButton;
  [SerializeField] GameObject quitButton;
  [SerializeField] GameObject controlSwitchButton;
  [SerializeField] PlayerController playerController;

  public static int controlScheme;
  private static bool gameStarted = false;

  private void Awake() {
    playerController.SetControlScheme(controlScheme);
    controlSwitchButton.GetComponentInChildren<Text>().text = GetControlSchemeString();
  }


    public void NewGame() {
      SceneManager.LoadScene(0);
      gameStarted = true;
      gamePaused = false;
      newGameButton.SetActive(false);
      playerController.SetControlScheme(controlScheme);
    }

    public void QuitGame() {
      Application.Quit();
    }

    public void ContinueGameButton() {
      if (gameStarted) {
      gamePaused = false;
      ResumeGame();
      }
    }

    public void SwitchControlScheme() {
    if (controlScheme == 0)
    {
      controlScheme = 1;
    }
    else
    {
      controlScheme = 0;
    }

    playerController.SetControlScheme(controlScheme);
    controlSwitchButton.GetComponentInChildren<Text>().text = GetControlSchemeString();

    }

    private string GetControlSchemeString() {
      if(controlScheme == 0) {
        return "Current control scheme: Rotate + accelerate: wad/arrows; Shoot: space";
      }
      if (controlScheme == 1)
      {
          return "Current control scheme: Mouselook; Fire: LMB,Space; Accelerate: RMB,W,UP";
        }
        else {
          return "no control scheme defined";
        }
    }

  void Update()
  {
    PauseGame();
  }

  private void PauseGame()
  {
    if (Input.GetKeyDown(KeyCode.Escape))
    {
      gamePaused = !gamePaused;
    }
    if (gamePaused)
    {
      Time.timeScale = 0;
      EnablePauseMenu(true);
    }
    else
    {
      ResumeGame();
    }
  }

  private void ResumeGame()
  {
    Time.timeScale = 1;
    EnablePauseMenu(false);
  }

  private void EnablePauseMenu(bool isGamePaused)
  {
    continueGameButton.SetActive(isGamePaused);
    pauseText.enabled = isGamePaused;
    newGameButton.SetActive(isGamePaused);
    quitButton.SetActive(isGamePaused);
    controlSwitchButton.SetActive(isGamePaused);
  }
}
