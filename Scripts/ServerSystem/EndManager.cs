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

	// 로그 메시지 텍스트
	[SerializeField] private TextMeshProUGUI logMessageText;

	[SerializeField] private Transform rankScrollView;
	[SerializeField] private GameObject rankCellPrefab;

    // 로그인 관련 유니티 이벤트
    NetworkLogTextEvent logTextEvent = new NetworkLogTextEvent();
    NetworkResponseDataEvent responseRankDataEvent = new NetworkResponseDataEvent();

    public override void OnEnable()
    {
        base.OnEnable();

        // 이벤트 연결
        logTextEvent.AddListener(OnPrintLogText);
        responseRankDataEvent.AddListener(OnRankCompleteData);
    }

    public override void OnDisable()
    {
        base.OnDisable();

        // 이벤트 연결 해제
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

        // 콜백으로 넘겨받은 JSON 데이터를 가지고 연관배열 객체를 생성함
        JsonData data = JsonMapper.ToObject(result);

        // 메시지 데이터 추출
        string message = (string)data["message"];

        logMessageText.text = message;

        // 결과 상태 정보 추출
        int status = (int)data["status"];

        // 랭크 조회 성공 처리
        if(status == 4000)
        {
            Debug.Log("랭크 리스트 로드");

            // 랭크 배열 정보에서 유저 정보를 추출하여 셀을 생성하고 출력함
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
        // PlayerPrefs에서 점수를 불러옵니다.
        int score = PlayerPrefs.GetInt("USER_SCORE", 0);
        scoreText.text = score.ToString();

        // 유저 랭킹을 조회함
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
        // 포톤 네트워크 연결 끊기
        PhotonNetwork.Disconnect();

        // 타이틀 씬으로 전환
        LoadSceneManager.LoadScene("StartScene");
    }
}
