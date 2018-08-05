using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour {

    public static SceneSwitcher manager;

    private void Awake()
    {
        manager = this;
    }

    public void SwitchScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void SwitchSceneWithWait(int sceneIndex)
    {
        StartCoroutine(Fadeout(1.5f,sceneIndex)); 
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator Fadeout(float fadeTime, int sceneIndex)
    {
        yield return new WaitForSeconds(fadeTime);
        SceneManager.LoadScene(sceneIndex);
    }

}
