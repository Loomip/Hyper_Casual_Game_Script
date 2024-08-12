using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JoinManager : MonoBehaviour
{
    // ���� ���̵� �Է� �ʵ�
    [SerializeField] private TMP_InputField idInputField;
    // ���� ��й�ȣ �Է� �ʵ�
    [SerializeField] private TMP_InputField pwInputField;
    // ���� �г��� �Է� �ʵ�
    [SerializeField] private TMP_InputField nickInputField;
    // ���� ��й�ȣ Ȯ�� �Է� �ʵ�
    [SerializeField] private TMP_InputField repwInputField;

    // ȸ�� ���� ���� ����Ƽ �̺�Ʈ
    NetworkLogTextEvent logTextEvent = new NetworkLogTextEvent();
    NetworkResponseDataEvent responseDataEvent = new NetworkResponseDataEvent();

    // �α� �޽��� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI logMessageText;

    public void OnJoinButtonClick()
    {
        string userId = idInputField.text.Trim();
        string userNick = nickInputField.text.Trim();
        string userPw = pwInputField.text.Trim();
        string userRePw = repwInputField.text.Trim();

        if (userId.Length < 4)
        {
            logMessageText.text = "���̵�� �ּ� 4�� �̻��Դϴ�.";
            return;
        }

        if (userNick.Length < 4)
        {
            logMessageText.text = "�г����� �ּ� 4�� �̻��Դϴ�.";
            return;
        }

        if (userPw.Length <= 0)
        {
            logMessageText.text = "��й�ȣ�� �Էµ��� �ʾҽ��ϴ�.";
            return;
        }

        if (!userRePw.Equals(userPw))
        {
            logMessageText.text = "��й�ȣ�� ��ġ���� �ʽ��ϴ�.";
            return;
        }

        // ������ ���� ȸ�� ������ ��û��
        NetAPIManager.instance.UserJoin(userId, userPw, userNick, logTextEvent, responseDataEvent);
    }

    private void OnEnable()
    {
        // �α� ��� �̺�Ʈ ����
        logTextEvent.AddListener(OnPrintLogText);
        // ȸ������ �Ϸ� �ݹ� �̺�Ʈ ����
        responseDataEvent.AddListener(OnJoinCompleteData);
    }

    private void OnDisable()
    {
        // �α� ��� �̺�Ʈ ���� ����
        logTextEvent.RemoveListener(OnPrintLogText);
        // ȸ������ �Ϸ� �ݹ� �̺�Ʈ ���� ����
        responseDataEvent.RemoveListener(OnJoinCompleteData);
    }

    public void OnPrintLogText(string log)
    {
        logMessageText.text = log;
    }

    // ȸ�� ���� �Ϸ� �̺�Ʈ �ݹ� �޼ҵ�
    public void OnJoinCompleteData(string responseTextData)
    {
        // ���� JSON ������
        string result = responseTextData.Trim();

        Debug.Log("response data : " + result);

        // LitJson�� �̿��� ��ųʸ�/�迭 ��ü ����
        JsonData data = JsonMapper.ToObject(result);

        // �޽��� �� ����
        string message = (string)data["message"];

        // �α��� �Ϸ� �޽��� ���
        logMessageText.text = message;

        // ���� ��� ���� ������ ���
        int status = (int)data["status"];

        // ȸ�������� ���� ó�� �Ǿ��ٸ� 
        if (status == 1000)
        {
            // { "statu" : 1000, "error": false, "message": "ȸ�� ������ �Ϸ��Ͽ����ϴ�.", "validata": null, "data":("uid":11)}
            
            int userId = (int)data["data"]["uid"];
            logMessageText.text = $"{userId} ���̵� ���� ȸ�������� �Ϸ�Ǿ����ϴ�.";
            Debug.Log(logMessageText);

            // �α��� ������ �̵�
            LoadSceneManager.LoadScene("LoginScene");
        }
        else
        {
            logMessageText.text = (string)data["message"];
        }
    }

    // ���� �� ���ư��� ��ư
    public void OnReturnButtonClick()
    {
        LoadSceneManager.LoadScene("LoginScene");
    }
}
