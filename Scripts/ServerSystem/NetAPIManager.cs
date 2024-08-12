using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

// 유니티 콜벡 이벤트
public class NetworkLogTextEvent : UnityEvent<string> { }
public class NetworkResponseDataEvent : UnityEvent<string> { }

// 네트워크 API 매니저 (싱글턴)
public class NetAPIManager : MonoBehaviour
{
    public static NetAPIManager instance;

    //프로토콜
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
        onLogTextEvent.Invoke("회원 가입을 진행중입니다.");

        yield return new WaitForSeconds(1f);

        // POST 파라미터 설정을 위해 WWWForm 객체를 설정함
        WWWForm postParam = new WWWForm();
        // POST 파라미터 추가함
        postParam.AddField("uid", userId.Trim());
        postParam.AddField("upw", userPw.Trim());
        postParam.AddField("nick", nickName.Trim());

        string url = protocol + "://" + serverAddress + "/gameserver/public/api/userjoin";

        // HTTP POST 요청 처리
        UnityWebRequest www = UnityWebRequest.Post(url, postParam);

        yield return www.SendWebRequest(); // HTTP 통신 요청/응답 지연 객체 생성

        // 서버와의 통신 중에 문제가 발생하면
        if (www.result != UnityWebRequest.Result.Success)
        {
            // 오류 콜백 메소드를 호출함
            if (onLogTextEvent != null)
            {
                onLogTextEvent.Invoke("서버와의 통신에 에러가 발생했습니다.");
            }
            Debug.Log(www.error); // 오류 메시지 로그 출력
        }
        else //서버와의 통신에 성공했다면
        {
            Debug.Log(www.downloadHandler.text.Trim());

            // 데이터 처리 콜백 메소드를 호출함
            onResponseDataEvent.Invoke(www.downloadHandler.text.Trim());
        }

        // 연결 해제
        www.Dispose();
    }

    // 회원 가입 처리
    public void UserJoin(string userId, string userPw, string nickName, NetworkLogTextEvent onLogTextEvent, NetworkResponseDataEvent onResponseDataEvent)
    {
        StartCoroutine(UserJoinCoroutine(userId, userPw, nickName, onLogTextEvent, onResponseDataEvent));
    }

    IEnumerator UserLoginCoroutine(string userId, string userPw, NetworkLogTextEvent onLogTextEvent, NetworkResponseDataEvent onResponseDataEvent)
    {
        onLogTextEvent.Invoke("유저 인증을 수행 중입니다.");

        yield return new WaitForSeconds(1f);

        // POST 파라미터 설정을 위해 WWWForm 객체를 설정함
        WWWForm postParam = new WWWForm();
        // POST 파라미터 추가함
        postParam.AddField("uid", userId.Trim());
        postParam.AddField("upw", userPw.Trim());

        string url = protocol + "://" + serverAddress + "/gameserver/public/api/userlogin";

        Debug.Log("Login URL : " + url);


        // HTTP POST 요청 처리
        using (UnityWebRequest www = UnityWebRequest.Post(url, postParam))
        {
            yield return www.SendWebRequest(); // HTTP 통신 요청/응답 지연 객체 생성

            // 서버와의 통신 중에 문제가 발생하면
            if (www.result != UnityWebRequest.Result.Success)
            {
                // 오류 콜백 메소드를 호출함
                if (onLogTextEvent != null)
                {
                    onLogTextEvent.Invoke("서버와의 통신에 에러가 발생했습니다.");
                }
                Debug.Log(www.error); // 오류 메시지 로그 출력
            }
            else //서버와의 통신에 성공했다면
            {
                // 데이터 처리 콜백 메소드를 호출함
                onResponseDataEvent.Invoke(www.downloadHandler.text.Trim());
            }
        }

        // using문을 쓰면 따로 www.Dispose();를 안써두 됨
    }

    public void UserLogin(string userId, string userPw, NetworkLogTextEvent onLogTextEvent, NetworkResponseDataEvent onResponseDataEvent)
    {
        StartCoroutine(UserLoginCoroutine(userId, userPw, onLogTextEvent, onResponseDataEvent));
    }

    // 유저 로그인 처리 통신 코루틴
    IEnumerator UserScoreUpdateCoroutine(string id, string score, NetworkLogTextEvent onLogTextEvent, NetworkResponseDataEvent onResponseDataEvent)
    {
        // 업데이트 관련 POST 파라미터 데이터 설정
        WWWForm postParam = new WWWForm();
        postParam.AddField("id", id.Trim());
        postParam.AddField("score", score.Trim());

        // 업데이트 API URL 설정
        string url = protocol + "://" + serverAddress + "/gameserver/public/api/userscoreupdate";

        Debug.Log("Login URL : " + url);

        // HTTP POST 요청 처리
        using (UnityWebRequest www = UnityWebRequest.Post(url, postParam))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                if (onLogTextEvent != null)
                    onLogTextEvent.Invoke("서버와의 통신에 에러가 발생했습니다.");
                Debug.Log(www.error);
            }
            else
            {
                onResponseDataEvent.Invoke(www.downloadHandler.text.Trim());
            }
        }
    }

    // 유저 점수 업데이트 처리
    public void UserScoreUpdate(string id, string score, NetworkLogTextEvent onLogTextEvent, NetworkResponseDataEvent onResponseDataEvent)
    {
        StartCoroutine(UserScoreUpdateCoroutine(id, score, onLogTextEvent, onResponseDataEvent));
    }

    // 유저 랭킹 정보 통신 코루틴
    IEnumerator UserRankListCoroutine(NetworkLogTextEvent onLogTextEvent, NetworkResponseDataEvent onResponseDataEvent)
    {
        onLogTextEvent.Invoke("유저 랭킹을 조회중입니다.");

        string url = protocol + "://" + serverAddress + "/gameserver/public/api/userrank";

        Debug.Log("Login URL : " + url);

        using(UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                if (onLogTextEvent != null)
                    onLogTextEvent.Invoke("서버와의 통신에 에러가 발생했습니다.");
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
