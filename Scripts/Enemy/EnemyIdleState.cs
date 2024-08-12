using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyAttackableState
{
    // 대기 상태 시작(진입) 처리 (상태 초기화)
    public override void EnterState(e_EnemyState state)
    {
        //// 네비게이션 에이전트 이동 정지
        //nav.isStopped = true;

        //// 대기 애니메이션 재생
        //animator.SetInteger("state", (int)state);
    }

    // 대기 상태 기능 동작 처리 (상태 실행)
    public override void UpdateState()
    {
        // 죽엇으면 리턴
        if (Health.Hp <= 0)
        {
            controller.Death();
            return;
        }

        if (controller.GetPlayerDistance() <= attackDistance)
        {
            controller.TransactionToState(e_EnemyState.Attack);
            return;
        }

        // 플레이어 인식
        if (controller.GetPlayerDistance() >= attackDistance)
        {
            // 추적 상태로 전환
            controller.TransactionToState(e_EnemyState.Run);
            return;
        }

    }

    // 대기 상태 종료(다른상태로 전이) 동작 처리(상태 정리)
    public override void ExitState()
    {

    }
}
