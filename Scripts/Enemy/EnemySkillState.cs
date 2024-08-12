using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkillState : EnemyAttackableState
{
    // 공격 대상 레이어
    [SerializeField] protected LayerMask targetLayer;

    // 스킬 프리펩
    [SerializeField] protected GameObject enemySkill;

    // 스킬 발동 위치
    [SerializeField] protected Transform SkillPos;

    // 공격력
    [SerializeField] protected int atk;

    // 점프력 
    [SerializeField] private float jumpSpeed;

    // 중력
    [SerializeField] private float gravity;

    // 점프중인지
    [SerializeField] protected bool isJump;

    public bool IsJump { get => isJump; set => isJump = value; }

    public void SkillAttack()
    {
        GameObject Effect = Instantiate(enemySkill, SkillPos.position, enemySkill.transform.rotation);
        EnemyEffect akt = Effect.GetComponent<EnemyEffect>();
        akt.Atk = atk;
        SoundManager.instance.PlaySfx(e_Sfx.EnemySkill);

        
    }

    public IEnumerator JumpCoroutine()
    {
        // 점프 높이와 지속 시간을 설정합니다.
        float jumpHeight = 1.5f; // 점프 높이
        float jumpDuration = 1f; // 점프 지속 시간

        // 점프 시작 위치를 현재 에이전트의 위치로 설정합니다.
        Vector3 startPos = nav.transform.position;

        Vector3 newVelocity = new Vector3(0, nav.velocity.y, 0);

        // 점프 종료 위치를 설정합니다.
        Vector3 endPos = startPos + newVelocity * jumpDuration;

        // 경과 시간을 초기화합니다.
        float elapsedTime = 0f;

        // 점프가 지속되는 동안 반복합니다.
        while (elapsedTime < jumpDuration)
        {
            // 경과 시간에 따른 점프의 진행률을 계산합니다.
            float t = elapsedTime / jumpDuration;

            // 시간에 따라 점프의 높이를 조절하여 자연스러운 점프를 구현합니다.
            float height = Mathf.Sin(Mathf.PI * t) * jumpHeight;

            // 점프 중인 동안 에이전트의 위치를 보간합니다.
            nav.transform.position = Vector3.Lerp(startPos, endPos, t) + Vector3.up * height;

            Anima.SetFloat("vertical", elapsedTime);

            // 경과 시간을 업데이트합니다.
            elapsedTime += Time.deltaTime;

            // 한 프레임을 기다립니다.
            yield return null;
        }
    }

    public override void EnterState(e_EnemyState state)
    {
        if (!IsJump)
        {
            nav.isStopped = true;

            nav.speed = 0f;

            Anima.SetInteger("state", (int)state);

            StartCoroutine("JumpCoroutine");
        }
    }

    public override void UpdateState()
    {
        if (IsJump) return;

        // 죽엇으면 리턴
        if (Health.Hp <= 0)
        {
            controller.Death();
            return;
        }

        // 공격 범위를 넘어가면
        if (controller.GetPlayerDistance() > attackDistance)
        {
            // 달리기 상태로 전환
            controller.TransactionToState(e_EnemyState.Run);
            return;
        }

        controller.LookAtTarget();
    }

    public override void ExitState()
    {
        nav.isStopped = false;

        nav.speed = 1f;
    }
}
