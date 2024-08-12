using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWakeUpState : EnemyState
{
    // 대기 상태 시작(진입) 처리 (상태 초기화)
    public override void EnterState(e_EnemyState state)
    {
        nav.isStopped = true;

        Anima.SetInteger("state", (int)state);
    }

    // 대기 상태 기능 동작 처리 (상태 실행)
    public override void UpdateState()
    {

    }

    // 대기 상태 종료(다른상태로 전이) 동작 처리(상태 정리)
    public override void ExitState()
    {

    }
}
