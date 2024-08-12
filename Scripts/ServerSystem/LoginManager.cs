using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginManager : MonoBehaviour
{
    // ���� ���̵� �Է� �ʵ�
    [SerializeField] private TMP_InputField idInputField;
    // ���� ��й�ȣ �Է� �ʵ�
    [SerializeField] private TMP_InputField pwInputField;
    // �α� �޽��� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI logMessageText;

    // �α��� ���� ����Ƽ �̺�Ʈ
    NetworkLogTextEvent logTextEvent = new NetworkLogTextEvent();
    NetworkResponseDataEvent responseDataEvent = new NetworkResponseDataEvent();

    private void OnEnable()
    {
        // �̺�Ʈ ����
        logTextEvent.AddListener(OnPrintLogText);
        responseDataEvent.AddListener(OnLoginCompleteData);
    }
    private void OnDisable()
    {
        // �̺�Ʈ ���� ����
        logTextEvent.RemoveListener(OnPrintLogText);
        responseDataEvent.RemoveListener(OnLoginCompleteData);
    }

    // �α��� ��ư Ŭ��
    public void OnLoginButtonClick()
    {
        string userId = idInputField.text.Trim();
        string userPw = pwInputField.text.Trim();

        if (userId.Length < 4)
        {
            logMessageText.text = "���̵�� �ּ� 4�� �̻��Դϴ�.";
            return;
        }

        if (userPw.Length <= 0)
        {
            logMessageText.text = "��й�ȣ�� �Էµ��� �ʾҽ��ϴ�.";
            return;
        }

        NetAPIManager.instance.UserLogin(userId, userPw, logTextEvent, responseDataEvent);
    }

    // ȸ�� ���� ��ư Ŭ��
    public void OnJoinButtonClick()
    {
        LoadSceneManager.LoadScene("JoinScene");
    }

    // ���� �� ���ư��� ��ư
    public void OnReturnButtonClick()
    {
        LoadSceneManager.LoadScene("StartScene");
    }

    // �α� �޽��� ���
    public void OnPrintLogText(string log)
    {
        logMessageText.text = log;
    }

    public void OnLoginCompleteData(string responseTextData)
    {
        string result = responseTextData.Trim();

        Debug.Log("resopnse data : " + result);

        // �ݹ����� �Ѱܹ��� JSON �����͸� ������ �����迭 ��ü�� ������
        JsonData data = JsonMapper.ToObject(result);

        // �޽��� ������ ����
        string message = (string)data["message"];

        // �α��� �Ϸ� �޽��� ���
        logMessageText.text = message;

        // ��� ���� ���� ����
        int status = (int)data["status"];

        // �α��ο� �����ߴٸ�
        if(status == 2000)
        {
            // ���� ���̵�� �г��� �����͸� ������
            string userId = (string)data["data"]["user_data"]["id"];
            logMessageText.text = $"{userId} ���̵� ������ �α����� �Ϸ� �߽��ϴ�.";

            string userNick = (string)data["data"]["user_data"]["nick"];

            // ���� ���̵�� �г����� PlayerPrefs�� ������
            PlayerPrefs.SetString("USER_ID", userId);
            PlayerPrefs.SetString("USER_NICK", userNick);
            PlayerPrefs.Save();

            SoundManager.instance.StopBgm();
            // ���Ӿ����� �̵�
            LoadSceneManager.LoadScene("PVEScene");
        }
        else
        {
            // ȸ�� �α��� ���� �޽���
            logMessageText.text = (string)data["message"];
        }
    }

}
