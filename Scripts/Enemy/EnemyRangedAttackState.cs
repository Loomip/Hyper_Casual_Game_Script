using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRangedAttackState : EnemyAttackableState
{
    // 공격 대상 레이어
    [SerializeField] protected LayerMask targetLayer;

    // 총알 프리펩
    [SerializeField] protected GameObject enemyBullet;

    // 총알 발사 위치
    [SerializeField] protected Transform bulletPos;

    // 공격력
    [SerializeField] protected int atk;

    public void RangedAttack()
    {
        // 대상 찾기
        Collider[] targets = Physics.OverlapSphere(transform.position, 11f, targetLayer);
        SoundManager.instance.PlaySfx(e_Sfx.EnemyRanged);
        if (targets.Length > 0)
        {
            // 가장 먼저 찾은 대상 선택
            Transform target = targets[0].transform;

            // 대상이 없으면 리턴
            if (target == null) return;

            GameObject proj = Instantiate(this.enemyBullet, bulletPos.position, this.enemyBullet.transform.rotation);
            EnemyBullet enemyBullet = proj.GetComponent<EnemyBullet>();
            Rigidbody rb = proj.GetComponent<Rigidbody>();
            enemyBullet.Atk = atk;
            rb.velocity = enemyBullet.CalculateLaunchData(target, bulletPos).initialVelocity;
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
}
