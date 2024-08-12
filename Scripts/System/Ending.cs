using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void OnSinggleModeButtonClick()
    {
        // 현재 활성화된 씬을 가져옴
        Scene currentScene = SceneManager.GetActiveScene();

        // 현재 씬의 이름을 이용해 씬을 다시 로드함
        LoadSceneManager.LoadScene(currentScene.name);
    }

    public void OnStartSceneButtonClick()
    {
        LoadSceneManager.LoadScene("StartScene");
    }
}
