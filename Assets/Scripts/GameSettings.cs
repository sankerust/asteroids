using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{
  public static bool gamePaused = true;

  PlayerController playerController;
  UIController uIController;

  public static int controlScheme;
  private static bool gameStarted = false;

    private void Awake() {
      playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
      uIController = GetComponent<UIController>();
      playerController.SetControlScheme(controlScheme);
    }

    public void NewGame() {
      SceneManager.LoadScene(0);
      gameStarted = true;
      gamePaused = false;
      uIController.NewGameButtonActive(false);
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
      if (controlScheme == 0) {
        controlScheme = 1;
      } else {
      controlScheme = 0;
      }

    playerController.SetControlScheme(controlScheme);
    uIController.UpdateSwitchButton();
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
      uIController.EnablePauseMenu(true);
    }
    else
    {
      ResumeGame();
    }
  }

  private void ResumeGame()
  {
    Time.timeScale = 1;
    uIController.EnablePauseMenu(false);
  }
}
