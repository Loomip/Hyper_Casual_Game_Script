using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Photon 관련 using 처리
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun.Demo.PunBasics;

public class TitleManager : MonoBehaviourPunCallbacks
{
    // 넘어갈 씬 이름
    [SerializeField] private string SingleMode;
    [SerializeField] private string MultiMode;

    // 접속 관련 로드 텍스트
    [SerializeField] private TextMeshProUGUI logMessageText;

    // 접속 버튼
    [SerializeField] private Button startButton;

    private bool isLobbyUpdatePaused = false;
    private float lobbyUpdatePauseDuration = 2.0f;

    private void Awake()
    {
        // 포톤 씬 자동 동기화 기능 활성화
        PhotonNetwork.AutomaticallySyncScene = true;

        // 포톤 메시지 큐 사용 여부 활성화
        PhotonNetwork.IsMessageQueueRunning = true;

        startButton.interactable = false; // 버튼 비활성화
    }

    public override void OnEnable()
    {
        base.OnEnable();

        // 씬이 활성화될 때 메시지 큐를 다시 실행
        PhotonNetwork.IsMessageQueueRunning = true;

        // 현재 포톤 클라우드에 접속된 상태가 아니면
        if (!PhotonNetwork.IsConnected)
        {
            logMessageText.text = "포톤 네트워크 접속 시도";
            PhotonNetwork.ConnectUsingSettings();
        }

        // 버튼 상태를 초기화
        startButton.interactable = PhotonNetwork.IsConnectedAndReady;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        // 씬이 비활성화될 때 메시지 큐를 중지
        PhotonNetwork.IsMessageQueueRunning = false;
    }

    private void Start()
    {
        SoundManager.instance.PlayBgm(e_Bgm.StartScene);
    }

    private void Update()
    {
        // 현재 포톤 클라우드에 접속된 상태가 아니면
        if (!isLobbyUpdatePaused && PhotonNetwork.InLobby)
        {
            logMessageText.text = "포톤 네트워크 로비 접속 상태 : " +
                (PhotonNetwork.InLobby ? "접속 완료 (인원 : " +
                PhotonNetwork.CountOfPlayersOnMaster + ")" : "접속 실패");
        }
    }

    // [포톤 네트워크 이벤트 콜백 메소드들]

    // 포톤 클라우드 접속 완료 이벤트
    public override void OnConnectedToMaster()
    {
        logMessageText.text = "포톤 네트워크 접속 완료";

        // 게임 시작 버튼 활성화
        startButton.interactable = true;

        // 만약 로비에 접속한 상태가 아니라면
        if (!PhotonNetwork.InLobby)
        {
            // 포톤 클라우드 로비에 접속함
            PhotonNetwork.JoinLobby();
        }
    }

    // 포톤 클라우드 로비 접속 완료 이벤트
    public override void OnJoinedLobby()
    {

    }

    // 포톤 랜덤 접속 시도
    public void JoinRandomRoom()
    {
        if (PhotonNetwork.InLobby)
        {
            // 쇼룸에 랜덤 접속 시도
            PhotonNetwork.JoinRandomRoom();
        }
    }

    // 포톤 랜덤 접속 성공 이벤트
    public override void OnJoinedRoom()
    {
        // 포톤 씬 전환 API를 이용해 씬을 전환함

        // 멀티 플레이 로딩 씬 시작
        PhotonLoadSceneManager.PhotonLoadScene(MultiMode);
    }

    // 랜덤 접속이 실패했을 경우 현재 유저가 랜덤한 방을 생성함
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // 랜덤 접속이 실패할 경우 쇼룸을 직접 생성함 (최대 유저수는 10)
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 10 });
    }

    public void OnSinggleModeButtonClick()
    {
        LoadSceneManager.LoadScene(SingleMode);
    }

    public void OnMultiModeButtonClick()
    {
        // 멀티플레이 모드가 아직 개발 중임을 알리는 메시지를 표시함
        logMessageText.text = "멀티플레이 모드 개발 중입니다.";

        // 로비 상태 업데이트 일시 중단
        isLobbyUpdatePaused = true;
        StartCoroutine(ResumeLobbyUpdateAfterDelay());

        // 버튼을 누를 때 더 이상의 행동을 하지 않음
        // 포톤 램덤 방 접속 기능 실행
        // JoinRandomRoom();
    }

     private IEnumerator ResumeLobbyUpdateAfterDelay()
    {
        yield return new WaitForSeconds(lobbyUpdatePauseDuration);
        isLobbyUpdatePaused = false;
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
    }
}
