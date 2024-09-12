using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    [SerializeField] string _startSceneToLoad;
    [SerializeField] string _creditsSceneToLoad;
    [SerializeField] float _sceneTransitionDelay = 1f;
    [SerializeField] Sprite _buttonPushedSprite;
    [SerializeField] Image _startButtonImage;
    [SerializeField] Image _exitButtonImage;
    [SerializeField] Image _creditsButtonImage;

    public void StartGame() {
        _startButtonImage.sprite = _buttonPushedSprite;
        StartCoroutine(LoadSceneWithDelay(_sceneTransitionDelay, _startSceneToLoad));
    }

    public void ExitGame() {
        _exitButtonImage.sprite = _buttonPushedSprite;
        Application.Quit();
    }

    public void Credits() {
        _creditsButtonImage.sprite = _buttonPushedSprite;
        StartCoroutine(LoadSceneWithDelay(_sceneTransitionDelay, _creditsSceneToLoad));
    }

    private IEnumerator LoadSceneWithDelay(float delay, string sceneName) {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
    }
}
