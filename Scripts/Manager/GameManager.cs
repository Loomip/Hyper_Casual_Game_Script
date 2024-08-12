using LitJson;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [Header("라운드")]
    [SerializeField] private LevelManager levelManager; // 레벨 매니저 참조
    [SerializeField] private TextMeshProUGUI txt_DeathCount;
    [SerializeField] private int currentRound = 0; // 현재 라운드
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private TextMeshProUGUI roundText;

    NetworkResponseDataEvent responseScoreUpdateDataEvent = new NetworkResponseDataEvent();

    private void OnEnable()
    {
        // 이벤트 연결
        responseScoreUpdateDataEvent.AddListener(OnUserScoreCompleteData);
    }

    private void OnDisable()
    {
        // 이벤트 연결 해제
        responseScoreUpdateDataEvent.RemoveListener(OnUserScoreCompleteData);
    }

    public void OnUserScoreCompleteData(string responseTextData)
    {
        string result = responseTextData.Trim();

        Debug.Log("response data : " + result);

        // 콜백으로 넘겨 받은 JSON 데이터를 가지고 연관배열 객체를 생성함
        JsonData data = JsonMapper.ToObject(result);

        // 메시지 데이터 추출
        string message = (string)data["message"];

        // 결과 상태 정보 추출
        int status = (int)data["status"];

        // 로그인에 성공했다면
        if (status == 3000)
        {
            // 유저 아이디와 닉네임 데이터를 추출함
            string userId = (string)data["data"]["id"];

            Debug.Log("유저 점수 업데이트 : " + userId);
        }
        else
        {
            Debug.Log("서버 통신 오류 발생 : " + message);
        }
    }

    void StartNextRound()
    {
        currentRound++;
        levelManager.StartRound(currentRound); // 레벨 매니저에게 다음 라운드 시작을 알림
        UpdateUI();
    }

    public void OnLevelCleared()
    {
        StartRoundCountdown();
    }

    // 데스 카운트 리프레쉬
    public void UpdateUI()
    {
        int remainingMonsters = levelManager.MonstersToSpawn - levelManager.MonstersDefeated;
        txt_DeathCount.text =  remainingMonsters + " / " + levelManager.MonstersToSpawn;
        roundText.text = $"Round: {currentRound}";
    }

    private void StartRoundCountdown()
    {
        StartCoroutine(StartNextRoundCoroutine());
    }

    private IEnumerator StartNextRoundCoroutine()
    {
        // Round 시작 대기 시간
        int countdownTime = 5; // 5초 대기

        countdownText.gameObject.SetActive(true); // 텍스트 UI 활성화
        countdownText.text = countdownTime.ToString();
        while (countdownTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            countdownTime--;
            countdownText.text = countdownTime.ToString();
        }
        countdownText.text = "GO!";

        yield return new WaitForSeconds(1.0f);

        countdownText.gameObject.SetActive(false); // 텍스트 UI 비활성화
        StartNextRound(); // 라운드 시작
    }

    //=================================================================================================================

    [Header("UI")]
    [SerializeField] private Slider playerHp;
    
    // 생성된 각 몬스터의 체력바를 저장할 딕셔너리
    [SerializeField] private Dictionary<Health, Slider> enemyHealthBars = new Dictionary<Health, Slider>();

    // 몬스터와 그에 해당하는 체력바를 딕셔너리에 등록
    public void RegisterEnemyHealthBar(Health enemyHealth, Slider healthBar)
    {
        if (enemyHealth is EHealth && !enemyHealthBars.ContainsKey(enemyHealth))
        {
            enemyHealthBars.Add(enemyHealth, healthBar);
        }
    }

    // 체력 리프레쉬
    public void RefreshHp(string tag, Health entity)
    {
        switch (tag)
        {
            case "Player":
                if (entity is PHealth)
                {
                    playerHp.value = (float)entity.Hp / entity.MaxHp;
                }
                break;
            case "Enemy":
                if (entity is EHealth && enemyHealthBars.TryGetValue(entity, out Slider enemyHp))
                {
                    enemyHp.value = (float)entity.Hp / entity.MaxHp;
                }
                break;
        }
    }

    //=================================================================================================================

    [Header("점수")]
    //골드
    [SerializeField] TextMeshProUGUI scoreText;

    // 현재 골드를 저장하는 변수
    private int score = 0;

    //골드 UI를 리프레쉬 해주는 함수
    public void Refresh_Score()
    {
        if (scoreText != null)
            scoreText.text = string.Format("{0: #,##0}", score);
    }

    public void UpdateUserScore(int newScore)
    {
        // 유저 아이디를 가져옵니다.
        string userId = PlayerPrefs.GetString("USER_ID", "");

        // 서버에 점수 업데이트를 요청합니다.
        NetAPIManager.instance.UserScoreUpdate(userId, newScore.ToString(), null,responseScoreUpdateDataEvent);

        // 점수를 PlayerPrefs에 저장합니다.
        PlayerPrefs.SetInt("USER_SCORE", newScore);
    }

    public void Add_Score(int addScore)
    {
        score += addScore;

        // 점수 업데이트 서버 요청
        UpdateUserScore(score);
    }
    //=================================================================================================================

    private void Start()
    {
        Refresh_Score();
        StartNextRound();
        SoundManager.instance.PlayBgm(e_Bgm.GameScene);
    }
}
