using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

    [SerializeField] string _startSceneToLoad;
    [SerializeField] string _creditsSceneToLoad;
    [SerializeField] float _sceneTransitionDelay = 1f;

    public void StartGame() {
        StartCoroutine(LoadSceneWithDelay(_sceneTransitionDelay, _startSceneToLoad));
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void Credits() {
        StartCoroutine(LoadSceneWithDelay(_sceneTransitionDelay, _creditsSceneToLoad));
    }

    private IEnumerator LoadSceneWithDelay(float delay, string sceneName) {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
