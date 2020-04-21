using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMenu : MonoBehaviour{
    private bool _paused = false;

    public GameObject PausePanel;

    // Update is called once per frame
    void Update(){
        if (Input.GetButtonDown("Cancel")) {
            PauseGame();
        }

    }

    public void PauseGame() {
        PausePanel.SetActive(!PausePanel.activeSelf);
        _paused = !_paused;

        if (_paused) {
            Time.timeScale = 0f;
        }
        else {
            Time.timeScale = 1f;
        }
    }

    public void MainMenu() {
        GameState.Hero = null;
        GameState.Princess = null;
        GameState.Level = 0;

        SceneLoader.Instance.MainMenu();
    }

    public void QuitGame() {
        SceneLoader.Instance.Quit();
    }
}
