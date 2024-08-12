using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class EndManager : MonoBehaviourPunCallbacks
{
	[SerializeField] private Text scoreText;

	// �α� �޽��� �ؽ�Ʈ
	[SerializeField] private TextMeshProUGUI logMessageText;

	[SerializeField] private Transform rankScrollView;
	[SerializeField] private GameObject rankCellPrefab;

    // �α��� ���� ����Ƽ �̺�Ʈ
    NetworkLogTextEvent logTextEvent = new NetworkLogTextEvent();
    NetworkResponseDataEvent responseRankDataEvent = new NetworkResponseDataEvent();

    public override void OnEnable()
    {
        base.OnEnable();

        // �̺�Ʈ ����
        logTextEvent.AddListener(OnPrintLogText);
        responseRankDataEvent.AddListener(OnRankCompleteData);
    }

    public override void OnDisable()
    {
        base.OnDisable();

        // �̺�Ʈ ���� ����
        logTextEvent.RemoveListener(OnPrintLogText);
        responseRankDataEvent.RemoveListener(OnRankCompleteData);
    }

    public void OnPrintLogText(string log)
    {
        logMessageText.text = log;
    }

    public void OnRankCompleteData(string responseTextData)
    {
        string result = responseTextData.Trim();

        Debug.Log("response data : " + result);

        // �ݹ����� �Ѱܹ��� JSON �����͸� ������ �����迭 ��ü�� ������
        JsonData data = JsonMapper.ToObject(result);

        // �޽��� ������ ����
        string message = (string)data["message"];

        logMessageText.text = message;

        // ��� ���� ���� ����
        int status = (int)data["status"];

        // ��ũ ��ȸ ���� ó��
        if(status == 4000)
        {
            Debug.Log("��ũ ����Ʈ �ε�");

            // ��ũ �迭 �������� ���� ������ �����Ͽ� ���� �����ϰ� �����
            for (int i = 0; i < data["data"].Count; i++)
            {
                int rank = (int)data["data"][i]["rank"];
                string nick = (string)data["data"][i]["nick"];
                string bestScore = (string)data["data"][i]["bestscore"];

                Debug.Log($"{rank}, {nick}, {bestScore}");

                UserRankCell userRankCell = Instantiate(rankCellPrefab, rankScrollView).GetComponent<UserRankCell>();
                userRankCell.Init(rank.ToString(), nick, bestScore);
            }
        }
        else
        {
            logMessageText.text = (string)data["message"];
        }
    }

    private void Start()
	{
        // PlayerPrefs���� ������ �ҷ��ɴϴ�.
        int score = PlayerPrefs.GetInt("USER_SCORE", 0);
        scoreText.text = score.ToString();

        // ���� ��ŷ�� ��ȸ��
        NetAPIManager.instance.UserRankList(logTextEvent, responseRankDataEvent);

        SoundManager.instance.PlayBgm(e_Bgm.StartScene);
	}

	public void OnReStartButtonClick()
    {
        LoadSceneManager.LoadScene("PVEScene");
        SoundManager.instance.StopBgm();
    }

    public void OnExitButtonClick()
    {
        // ���� ��Ʈ��ũ ���� ����
        PhotonNetwork.Disconnect();

        // Ÿ��Ʋ ������ ��ȯ
        LoadSceneManager.LoadScene("StartScene");
    }
}
