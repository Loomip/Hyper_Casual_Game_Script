using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRunState : EnemyAttackableState
{
    // 대기 상태 시작(진입) 처리 (상태 초기화)
    public override void EnterState(e_EnemyState state)
    {
        // 몬스터 스피드 설정
        nav.speed = 3.5f;

        // 공격 대상 추적 처리
        nav.isStopped = false;

        Anima.SetInteger("state", (int)state);
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

        // 플레이어가 공격 가능 거리안에 들어왔다면
        if (controller.GetPlayerDistance() < attackDistance)
        {
            // 공격 상태로 전환
            controller.TransactionToState(e_EnemyState.Attack);
            return;
        }

        // 공격 대상 추적 처리
        nav.isStopped = false;
        nav.SetDestination(controller.Player.transform.position);
    }

    // 대기 상태 종료(다른상태로 전이) 동작 처리(상태 정리)
    public override void ExitState()
    {

    }
}
