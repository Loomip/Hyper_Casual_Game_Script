using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

// ����Ƽ �ݺ� �̺�Ʈ
public class NetworkLogTextEvent : UnityEvent<string> { }
public class NetworkResponseDataEvent : UnityEvent<string> { }

// ��Ʈ��ũ API �Ŵ��� (�̱���)
public class NetAPIManager : MonoBehaviour
{
    public static NetAPIManager instance;

    //��������
    [SerializeField] private string protocol;

    [SerializeField] private string serverAddress;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator UserJoinCoroutine(string userId, string userPw, string nickName, NetworkLogTextEvent onLogTextEvent, NetworkResponseDataEvent onResponseDataEvent)
    {
        onLogTextEvent.Invoke("ȸ�� ������ �������Դϴ�.");

        yield return new WaitForSeconds(1f);

        // POST �Ķ���� ������ ���� WWWForm ��ü�� ������
        WWWForm postParam = new WWWForm();
        // POST �Ķ���� �߰���
        postParam.AddField("uid", userId.Trim());
        postParam.AddField("upw", userPw.Trim());
        postParam.AddField("nick", nickName.Trim());

        string url = protocol + "://" + serverAddress + "/gameserver/public/api/userjoin";

        // HTTP POST ��û ó��
        UnityWebRequest www = UnityWebRequest.Post(url, postParam);

        yield return www.SendWebRequest(); // HTTP ��� ��û/���� ���� ��ü ����

        // �������� ��� �߿� ������ �߻��ϸ�
        if (www.result != UnityWebRequest.Result.Success)
        {
            // ���� �ݹ� �޼ҵ带 ȣ����
            if (onLogTextEvent != null)
            {
                onLogTextEvent.Invoke("�������� ��ſ� ������ �߻��߽��ϴ�.");
            }
            Debug.Log(www.error); // ���� �޽��� �α� ���
        }
        else //�������� ��ſ� �����ߴٸ�
        {
            Debug.Log(www.downloadHandler.text.Trim());

            // ������ ó�� �ݹ� �޼ҵ带 ȣ����
            onResponseDataEvent.Invoke(www.downloadHandler.text.Trim());
        }

        // ���� ����
        www.Dispose();
    }

    // ȸ�� ���� ó��
    public void UserJoin(string userId, string userPw, string nickName, NetworkLogTextEvent onLogTextEvent, NetworkResponseDataEvent onResponseDataEvent)
    {
        StartCoroutine(UserJoinCoroutine(userId, userPw, nickName, onLogTextEvent, onResponseDataEvent));
    }

    IEnumerator UserLoginCoroutine(string userId, string userPw, NetworkLogTextEvent onLogTextEvent, NetworkResponseDataEvent onResponseDataEvent)
    {
        onLogTextEvent.Invoke("���� ������ ���� ���Դϴ�.");

        yield return new WaitForSeconds(1f);

        // POST �Ķ���� ������ ���� WWWForm ��ü�� ������
        WWWForm postParam = new WWWForm();
        // POST �Ķ���� �߰���
        postParam.AddField("uid", userId.Trim());
        postParam.AddField("upw", userPw.Trim());

        string url = protocol + "://" + serverAddress + "/gameserver/public/api/userlogin";

        Debug.Log("Login URL : " + url);


        // HTTP POST ��û ó��
        using (UnityWebRequest www = UnityWebRequest.Post(url, postParam))
        {
            yield return www.SendWebRequest(); // HTTP ��� ��û/���� ���� ��ü ����

            // �������� ��� �߿� ������ �߻��ϸ�
            if (www.result != UnityWebRequest.Result.Success)
            {
                // ���� �ݹ� �޼ҵ带 ȣ����
                if (onLogTextEvent != null)
                {
                    onLogTextEvent.Invoke("�������� ��ſ� ������ �߻��߽��ϴ�.");
                }
                Debug.Log(www.error); // ���� �޽��� �α� ���
            }
            else //�������� ��ſ� �����ߴٸ�
            {
                // ������ ó�� �ݹ� �޼ҵ带 ȣ����
                onResponseDataEvent.Invoke(www.downloadHandler.text.Trim());
            }
        }

        // using���� ���� ���� www.Dispose();�� �Ƚ�� ��
    }

    public void UserLogin(string userId, string userPw, NetworkLogTextEvent onLogTextEvent, NetworkResponseDataEvent onResponseDataEvent)
    {
        StartCoroutine(UserLoginCoroutine(userId, userPw, onLogTextEvent, onResponseDataEvent));
    }

    // ���� �α��� ó�� ��� �ڷ�ƾ
    IEnumerator UserScoreUpdateCoroutine(string id, string score, NetworkLogTextEvent onLogTextEvent, NetworkResponseDataEvent onResponseDataEvent)
    {
        // ������Ʈ ���� POST �Ķ���� ������ ����
        WWWForm postParam = new WWWForm();
        postParam.AddField("id", id.Trim());
        postParam.AddField("score", score.Trim());

        // ������Ʈ API URL ����
        string url = protocol + "://" + serverAddress + "/gameserver/public/api/userscoreupdate";

        Debug.Log("Login URL : " + url);

        // HTTP POST ��û ó��
        using (UnityWebRequest www = UnityWebRequest.Post(url, postParam))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                if (onLogTextEvent != null)
                    onLogTextEvent.Invoke("�������� ��ſ� ������ �߻��߽��ϴ�.");
                Debug.Log(www.error);
            }
            else
            {
                onResponseDataEvent.Invoke(www.downloadHandler.text.Trim());
            }
        }
    }

    // ���� ���� ������Ʈ ó��
    public void UserScoreUpdate(string id, string score, NetworkLogTextEvent onLogTextEvent, NetworkResponseDataEvent onResponseDataEvent)
    {
        StartCoroutine(UserScoreUpdateCoroutine(id, score, onLogTextEvent, onResponseDataEvent));
    }

    // ���� ��ŷ ���� ��� �ڷ�ƾ
    IEnumerator UserRankListCoroutine(NetworkLogTextEvent onLogTextEvent, NetworkResponseDataEvent onResponseDataEvent)
    {
        onLogTextEvent.Invoke("���� ��ŷ�� ��ȸ���Դϴ�.");

        string url = protocol + "://" + serverAddress + "/gameserver/public/api/userrank";

        Debug.Log("Login URL : " + url);

        using(UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                if (onLogTextEvent != null)
                    onLogTextEvent.Invoke("�������� ��ſ� ������ �߻��߽��ϴ�.");
                Debug.Log(www.error);
            }
            else
            {
                onResponseDataEvent.Invoke(www.downloadHandler.text.Trim());
            }
        }
    }

    public void UserRankList(NetworkLogTextEvent onLogTextEvent, NetworkResponseDataEvent onResponseDataEvent)
    {
        StartCoroutine(UserRankListCoroutine(onLogTextEvent, onResponseDataEvent));
    }
}
