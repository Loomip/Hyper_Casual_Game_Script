using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSMController : MonoBehaviour
{
    // 몬스터의 현재 동작 중인 상태 컴포넌트
    [SerializeField] private EnemyState currentState;

    // 몬스터의 모든 상태 컴포넌트들
    [SerializeField] private EnemyState[] EnemyStatas;

    // 회전 보간 수치
    [SerializeField] protected float smoothValue;

    // 플레이어 참조
    protected GameObject player;
    public GameObject Player { get => player; set => player = value; }

    [SerializeField] private GameObject[] headModels; // 머리 모델 배열
    [SerializeField] private GameObject[] bodyModels; // 몸 모델 배열
    [SerializeField] private GameObject[] WeaponModels; // 무기 모델 배열

    // 사망 후 생성될 아이템
    [SerializeField] protected GameObject[] items;
    [SerializeField] protected float[] dropChances; // 아이템 드롭 확률 배열

    private const float totalChance = 100f; // 전체 드롭 확률

    // 올라갈 점수
    [SerializeField] private int score;

    // 피격 파티클
    [SerializeField] private ParticleSystem hitParticle;
    public ParticleSystem HitParticle { get => hitParticle; set => hitParticle = value; }

    // 상태 전환 메소드
    public void TransactionToState(e_EnemyState state)
    {
        currentState?.ExitState(); // 이전 상태 정리
        currentState = EnemyStatas[(int)state]; // 상태 전환 처리
        currentState.EnterState(state); // 세로운 상태 전이
    }

    // 보조 컨트롤러 기능들

    // 플레이어와 몬스터간의 거리 측정
    public float GetPlayerDistance()
    {
        return Vector3.Distance(transform.position, player.transform.position);
    }

    // 공격 대상을 주시
    public void LookAtTarget()
    {
        // 공격 대상을 향한 방향을 계산
        Vector3 direction = (Player.transform.position - transform.position).normalized;

        // 회전 쿼터니언 계산
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));

        // 보간 회전
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * smoothValue);
    }

    // 플레이어에게 공격을 받음
    public void Hit()
    {
        // 현재 상태가 이미 사망한 상태면 피격 처리하지 않음
        if (currentState == EnemyStatas[(int)e_EnemyState.Die]) return;

        // 피격 상태로 전환
        TransactionToState(e_EnemyState.Hit);
    }

    public void Knockdown()
    {
        // 현재 상태가 이미 사망한 상태면 피격 처리하지 않음
        if (currentState == EnemyStatas[(int)e_EnemyState.Die]) return;

        TransactionToState(e_EnemyState.Knockdown);
    }

    public void Death()
    {
        // 죽음 상태로 전환
        TransactionToState(e_EnemyState.Die);
    }

    public void DropItem()
    {
        float remainingChance = totalChance; // 남은 확률을 전체 확률로 초기화

        // 아이템 배열의 길이와 드롭 확률 배열의 길이가 같은지 확인
        if (items.Length != dropChances.Length)
        {
            Debug.LogError("아이템과 드롭 확률의 개수가 일치하지 않습니다.");
            return;
        }

        // 모든 아이템에 대해 확률 조정
        for (int i = 0; i < items.Length; i++)
        {
            // 아이템의 드롭 확률이 0보다 작거나 같은 경우 꽝으로 처리
            if (dropChances[i] <= 0)
            {
                Debug.LogWarning(items[i].name + "의 드롭 확률이 0보다 작거나 같습니다. 이 아이템은 드롭되지 않습니다.");
                continue;
            }

            // 아이템의 드롭 확률이 남은 확률보다 클 경우 남은 확률을 아이템의 드롭 확률로 조정
            if (dropChances[i] > remainingChance)
            {
                dropChances[i] = remainingChance;
            }

            // 남은 확률에서 아이템의 드롭 확률을 빼고, 이를 다시 남은 확률로 업데이트
            remainingChance -= dropChances[i];
        }

        // 아이템을 랜덤으로 선택
        float randomValue = Random.Range(0f, totalChance);
        float cumulativeChance = 0f;

        for (int i = 0; i < items.Length; i++)
        {
            cumulativeChance += dropChances[i];
            if (randomValue <= cumulativeChance)
            {
                // 선택된 아이템을 드롭
                Instantiate(items[i], transform.position + items[i].transform.position, items[i].transform.rotation);
                return;
            }
        }
    }

    public void Score()
    {
        GameManager.instance.Add_Score(score);
        GameManager.instance.Refresh_Score();
    }

    void Start()
    {
        player = GameObject.FindWithTag("Player");

        // 모든 머리와 몸 모델을 비활성화
        foreach (var model in headModels) model.SetActive(false);
        foreach (var model in bodyModels) model.SetActive(false);

        if (WeaponModels != null)
        {
            foreach (var model in WeaponModels)
                model.SetActive(false);
        }

        // 머리와 몸 모델 중에서 랜덤하게 선택
        GameObject headModel = headModels[Random.Range(0, headModels.Length)];
        GameObject bodyModel = bodyModels[Random.Range(0, bodyModels.Length)];

        // 선택한 머리와 몸 모델을 활성화
        headModel.SetActive(true);
        bodyModel.SetActive(true);

        if (WeaponModels != null && WeaponModels.Length > 0)
        {
            GameObject weaponModel = WeaponModels[Random.Range(0, WeaponModels.Length)];
            weaponModel.SetActive(true);
        }

        // 대기 상태로 시작
        TransactionToState(e_EnemyState.Idle);
    }

    private void Update()
    {
        // 현재 설정된 상태의 기능을 동작
        currentState?.UpdateState();
    }
}
