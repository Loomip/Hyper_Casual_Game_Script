using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState : EnemyState
{
    // 사망 완료 처리 시간
    protected float time;
    [SerializeField] protected float deathDelayTime;

    // 사망 처리 이펙트
    [SerializeField] protected GameObject destroyParticlePrefab;

    public override void EnterState(e_EnemyState state)
    {
        // 이동 중지
        nav.isStopped = true;

        col.isTrigger = true;

        gameObject.layer = 0;

        //SoundManager.instance.PlaySfx(e_Sfx.EnemyDie);

        Anima.SetInteger("state", (int)state);

        Anima.SetBool("isDeath", true);

        levelManager.OnMonsterDefeated();

        // 골드 생성
        controller.Score();
    }

    public override void UpdateState()
    {
        time += Time.deltaTime;

        // 사망 처리 지연시간이 지났다면
        if (time >= deathDelayTime)
        {
            // 사망 이팩트 생성
            Instantiate(destroyParticlePrefab, transform.position, destroyParticlePrefab.transform.rotation);
            // 랜덤 아이템 생성
            controller.DropItem();

            SoundManager.instance.PlaySfx(e_Sfx.EnemyDie);
            // 시체 파괴
            Destroy(gameObject);
            
        }
    }

    public override void ExitState()
    {

    }
}
