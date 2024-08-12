using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitState : EnemyAttackableState
{
    private bool isHit = false;

    public bool IsHit { get => isHit; set => isHit = value; }

    public override void EnterState(e_EnemyState state)
    {
        isHit = true;

        // 이동 중지
        nav.isStopped = true;

        // 히트 이펙트 실행
        controller.HitParticle.Play();

        // 히트 모션 실행
        Anima.SetInteger("state", (int)state);

        SoundManager.instance.PlaySfx(e_Sfx.EnemyHit);
    }

    public override void UpdateState()
    {
        if(Health.Hp <= 0)
        {
            controller.Death();
            return;
        }

        if (!IsHit)
        {
            // 플레이어가 공격 가능 거리안에 들어왔다면
            if (controller.GetPlayerDistance() <= attackDistance)
            {
                // 공격 상태로 전환
                controller.TransactionToState(e_EnemyState.Attack);
                return;
            }

            // 공격 범위를 넘어가면
            if (controller.GetPlayerDistance() > attackDistance)
            {
                // 달리기 상태로 전환
                controller.TransactionToState(e_EnemyState.Run);
                return;
            }
        }
    }

    public override void ExitState()
    {

    }
}
