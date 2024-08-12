using LitJson;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [Header("����")]
    [SerializeField] private LevelManager levelManager; // ���� �Ŵ��� ����
    [SerializeField] private TextMeshProUGUI txt_DeathCount;
    [SerializeField] private int currentRound = 0; // ���� ����
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private TextMeshProUGUI roundText;

    NetworkResponseDataEvent responseScoreUpdateDataEvent = new NetworkResponseDataEvent();

    private void OnEnable()
    {
        // �̺�Ʈ ����
        responseScoreUpdateDataEvent.AddListener(OnUserScoreCompleteData);
    }

    private void OnDisable()
    {
        // �̺�Ʈ ���� ����
        responseScoreUpdateDataEvent.RemoveListener(OnUserScoreCompleteData);
    }

    public void OnUserScoreCompleteData(string responseTextData)
    {
        string result = responseTextData.Trim();

        Debug.Log("response data : " + result);

        // �ݹ����� �Ѱ� ���� JSON �����͸� ������ �����迭 ��ü�� ������
        JsonData data = JsonMapper.ToObject(result);

        // �޽��� ������ ����
        string message = (string)data["message"];

        // ��� ���� ���� ����
        int status = (int)data["status"];

        // �α��ο� �����ߴٸ�
        if (status == 3000)
        {
            // ���� ���̵�� �г��� �����͸� ������
            string userId = (string)data["data"]["id"];

            Debug.Log("���� ���� ������Ʈ : " + userId);
        }
        else
        {
            Debug.Log("���� ��� ���� �߻� : " + message);
        }
    }

    void StartNextRound()
    {
        currentRound++;
        levelManager.StartRound(currentRound); // ���� �Ŵ������� ���� ���� ������ �˸�
        UpdateUI();
    }

    public void OnLevelCleared()
    {
        StartRoundCountdown();
    }

    // ���� ī��Ʈ ��������
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
        // Round ���� ��� �ð�
        int countdownTime = 5; // 5�� ���

        countdownText.gameObject.SetActive(true); // �ؽ�Ʈ UI Ȱ��ȭ
        countdownText.text = countdownTime.ToString();
        while (countdownTime > 0)
        {
            yield return new WaitForSeconds(1.0f);
            countdownTime--;
            countdownText.text = countdownTime.ToString();
        }
        countdownText.text = "GO!";

        yield return new WaitForSeconds(1.0f);

        countdownText.gameObject.SetActive(false); // �ؽ�Ʈ UI ��Ȱ��ȭ
        StartNextRound(); // ���� ����
    }

    //=================================================================================================================

    [Header("UI")]
    [SerializeField] private Slider playerHp;
    
    // ������ �� ������ ü�¹ٸ� ������ ��ųʸ�
    [SerializeField] private Dictionary<Health, Slider> enemyHealthBars = new Dictionary<Health, Slider>();

    // ���Ϳ� �׿� �ش��ϴ� ü�¹ٸ� ��ųʸ��� ���
    public void RegisterEnemyHealthBar(Health enemyHealth, Slider healthBar)
    {
        if (enemyHealth is EHealth && !enemyHealthBars.ContainsKey(enemyHealth))
        {
            enemyHealthBars.Add(enemyHealth, healthBar);
        }
    }

    // ü�� ��������
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

    [Header("����")]
    //���
    [SerializeField] TextMeshProUGUI scoreText;

    // ���� ��带 �����ϴ� ����
    private int score = 0;

    //��� UI�� �������� ���ִ� �Լ�
    public void Refresh_Score()
    {
        if (scoreText != null)
            scoreText.text = string.Format("{0: #,##0}", score);
    }

    public void UpdateUserScore(int newScore)
    {
        // ���� ���̵� �����ɴϴ�.
        string userId = PlayerPrefs.GetString("USER_ID", "");

        // ������ ���� ������Ʈ�� ��û�մϴ�.
        NetAPIManager.instance.UserScoreUpdate(userId, newScore.ToString(), null,responseScoreUpdateDataEvent);

        // ������ PlayerPrefs�� �����մϴ�.
        PlayerPrefs.SetInt("USER_SCORE", newScore);
    }

    public void Add_Score(int addScore)
    {
        score += addScore;

        // ���� ������Ʈ ���� ��û
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
