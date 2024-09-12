using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour {

    [SerializeField] GameObject _pauseMenuCanvas;

    public bool _paused;

    void Start () { _paused = false; }

    void Update() {

        if (Input.GetKeyDown(KeyCode.Escape)) {

            if (_paused) { Resume(); }

            else { Pause(); }

        }
    }

    void Pause() {
        _pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        _paused = true;
    }

    public void Resume() {
        _pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        _paused = false;
    }

    public void QuitGame() {
        Application.Quit();
    }

}
