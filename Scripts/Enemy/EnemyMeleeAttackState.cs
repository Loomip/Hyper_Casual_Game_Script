using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttackState : EnemyAttackableState
{
    // 공격 타겟 중심점 위치
    [SerializeField] private Transform attackTransfom;

    // 공격 범위
    [SerializeField] private float attackRadius;

    // 공격 범위 각도
    [SerializeField] private float hitAngle;

    // 공격 대상 레이어
    [SerializeField] protected LayerMask targetLayer;

    // 공격력
    [SerializeField] protected int atk;

    public void MeleeAttack()
    {
        // 공격이 시작되었음을 디버그 로그로 출력

        // 공격 범위 내의 충돌체 탐지
        Collider[] hits = Physics.OverlapSphere(attackTransfom.position, attackRadius, targetLayer);
        SoundManager.instance.PlaySfx(e_Sfx.EnemyMelee);
        foreach (Collider hit in hits)
        {
            Vector3 directionToTarget = hit.transform.position - transform.position;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleToTarget < hitAngle)
            {
                // 플레이어 피격 처리
                if (hit.CompareTag("Player"))
                {
                    // 히트 판정
                    PHealth pHealth = hit.GetComponent<PHealth>();
                    if (pHealth != null)
                    {
                        pHealth.Hit(atk);
                    }
                }
            }
        }
    }

    public override void EnterState(e_EnemyState state)
    {
        nav.isStopped = true;

        nav.speed = 0f;

        Anima.SetInteger("state", (int)state);
    }

    public override void UpdateState()
    {
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

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackTransfom.position, attackRadius);
    }
}
