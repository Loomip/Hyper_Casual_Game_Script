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
        // ���� Ȱ��ȭ�� ���� ������
        Scene currentScene = SceneManager.GetActiveScene();

        // ���� ���� �̸��� �̿��� ���� �ٽ� �ε���
        LoadSceneManager.LoadScene(currentScene.name);
    }

    public void OnStartSceneButtonClick()
    {
        LoadSceneManager.LoadScene("StartScene");
    }
}
