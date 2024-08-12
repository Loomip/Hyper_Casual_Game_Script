using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class JoinManager : MonoBehaviour
{
    // 유저 아이디 입력 필드
    [SerializeField] private TMP_InputField idInputField;
    // 유저 비밀번호 입력 필드
    [SerializeField] private TMP_InputField pwInputField;
    // 유저 닉네임 입력 필드
    [SerializeField] private TMP_InputField nickInputField;
    // 유저 비밀번호 확인 입력 필드
    [SerializeField] private TMP_InputField repwInputField;

    // 회원 가입 관련 유니티 이벤트
    NetworkLogTextEvent logTextEvent = new NetworkLogTextEvent();
    NetworkResponseDataEvent responseDataEvent = new NetworkResponseDataEvent();

    // 로그 메시지 텍스트
    [SerializeField] private TextMeshProUGUI logMessageText;

    public void OnJoinButtonClick()
    {
        string userId = idInputField.text.Trim();
        string userNick = nickInputField.text.Trim();
        string userPw = pwInputField.text.Trim();
        string userRePw = repwInputField.text.Trim();

        if (userId.Length < 4)
        {
            logMessageText.text = "아이디는 최소 4자 이상입니다.";
            return;
        }

        if (userNick.Length < 4)
        {
            logMessageText.text = "닉네임은 최소 4자 이상입니다.";
            return;
        }

        if (userPw.Length <= 0)
        {
            logMessageText.text = "비밀번호가 입력되지 않았습니다.";
            return;
        }

        if (!userRePw.Equals(userPw))
        {
            logMessageText.text = "비밀번호와 일치하지 않습니다.";
            return;
        }

        // 서버에 유저 회원 가입을 요청함
        NetAPIManager.instance.UserJoin(userId, userPw, userNick, logTextEvent, responseDataEvent);
    }

    private void OnEnable()
    {
        // 로그 출력 이벤트 연결
        logTextEvent.AddListener(OnPrintLogText);
        // 회원가입 완료 콜백 이벤트 연결
        responseDataEvent.AddListener(OnJoinCompleteData);
    }

    private void OnDisable()
    {
        // 로그 출력 이벤트 연결 해제
        logTextEvent.RemoveListener(OnPrintLogText);
        // 회원가입 완료 콜백 이벤트 연결 해제
        responseDataEvent.RemoveListener(OnJoinCompleteData);
    }

    public void OnPrintLogText(string log)
    {
        logMessageText.text = log;
    }

    // 회원 가입 완료 이벤트 콜백 메소드
    public void OnJoinCompleteData(string responseTextData)
    {
        // 응답 JSON 데이터
        string result = responseTextData.Trim();

        Debug.Log("response data : " + result);

        // LitJson을 이용한 딕셔너리/배열 객체 생성
        JsonData data = JsonMapper.ToObject(result);

        // 메시지 값 추출
        string message = (string)data["message"];

        // 로그인 완료 메시지 출력
        logMessageText.text = message;

        // 응답 결과 상태 전ㅇ보 출력
        int status = (int)data["status"];

        // 회원가입이 정상 처리 되었다면 
        if (status == 1000)
        {
            // { "statu" : 1000, "error": false, "message": "회원 가입을 완료하였습니다.", "validata": null, "data":("uid":11)}
            
            int userId = (int)data["data"]["uid"];
            logMessageText.text = $"{userId} 아이디 유저 회원가입이 완료되었습니다.";
            Debug.Log(logMessageText);

            // 로그인 씬으로 이동
            LoadSceneManager.LoadScene("LoginScene");
        }
        else
        {
            logMessageText.text = (string)data["message"];
        }
    }

    // 메인 씬 돌아가는 버튼
    public void OnReturnButtonClick()
    {
        LoadSceneManager.LoadScene("LoginScene");
    }
}
