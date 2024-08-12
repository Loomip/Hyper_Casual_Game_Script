using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginManager : MonoBehaviour
{
    // 유저 아이디 입력 필드
    [SerializeField] private TMP_InputField idInputField;
    // 유저 비밀번호 입력 필드
    [SerializeField] private TMP_InputField pwInputField;
    // 로그 메시지 텍스트
    [SerializeField] private TextMeshProUGUI logMessageText;

    // 로그인 관련 유니티 이벤트
    NetworkLogTextEvent logTextEvent = new NetworkLogTextEvent();
    NetworkResponseDataEvent responseDataEvent = new NetworkResponseDataEvent();

    private void OnEnable()
    {
        // 이벤트 연결
        logTextEvent.AddListener(OnPrintLogText);
        responseDataEvent.AddListener(OnLoginCompleteData);
    }
    private void OnDisable()
    {
        // 이벤트 연결 해제
        logTextEvent.RemoveListener(OnPrintLogText);
        responseDataEvent.RemoveListener(OnLoginCompleteData);
    }

    // 로그인 버튼 클릭
    public void OnLoginButtonClick()
    {
        string userId = idInputField.text.Trim();
        string userPw = pwInputField.text.Trim();

        if (userId.Length < 4)
        {
            logMessageText.text = "아이디는 최소 4자 이상입니다.";
            return;
        }

        if (userPw.Length <= 0)
        {
            logMessageText.text = "비밀번호가 입력되지 않았습니다.";
            return;
        }

        NetAPIManager.instance.UserLogin(userId, userPw, logTextEvent, responseDataEvent);
    }

    // 회원 가입 버튼 클릭
    public void OnJoinButtonClick()
    {
        LoadSceneManager.LoadScene("JoinScene");
    }

    // 메인 씬 돌아가는 버튼
    public void OnReturnButtonClick()
    {
        LoadSceneManager.LoadScene("StartScene");
    }

    // 로그 메시지 출력
    public void OnPrintLogText(string log)
    {
        logMessageText.text = log;
    }

    public void OnLoginCompleteData(string responseTextData)
    {
        string result = responseTextData.Trim();

        Debug.Log("resopnse data : " + result);

        // 콜백으로 넘겨받은 JSON 데이터를 가지고 연관배열 객체를 생성함
        JsonData data = JsonMapper.ToObject(result);

        // 메시지 데이터 추출
        string message = (string)data["message"];

        // 로그인 완료 메시지 출력
        logMessageText.text = message;

        // 결과 상태 정보 추출
        int status = (int)data["status"];

        // 로그인에 성공했다면
        if(status == 2000)
        {
            // 유저 아이디와 닉네임 데이터를 추출함
            string userId = (string)data["data"]["user_data"]["id"];
            logMessageText.text = $"{userId} 아이디 유저가 로그인을 완료 했습니다.";

            string userNick = (string)data["data"]["user_data"]["nick"];

            // 유저 아이디와 닉네임을 PlayerPrefs에 저장함
            PlayerPrefs.SetString("USER_ID", userId);
            PlayerPrefs.SetString("USER_NICK", userNick);
            PlayerPrefs.Save();

            SoundManager.instance.StopBgm();
            // 게임씬으로 이동
            LoadSceneManager.LoadScene("PVEScene");
        }
        else
        {
            // 회원 로그인 실패 메시지
            logMessageText.text = (string)data["message"];
        }
    }

}
