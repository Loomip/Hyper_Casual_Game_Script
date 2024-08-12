using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockdownState : EnemyAttackableState
{
    // ªÁ∏¡ øœ∑· √≥∏Æ Ω√∞£
    protected float time;
    [SerializeField] protected float deathDelayTime;

    // ªÁ∏¡ √≥∏Æ ¿Ã∆Â∆Æ
    [SerializeField] protected GameObject destroyParticlePrefab;

    private bool isKnockDown = false;

    public bool IsKnockDown { get => isKnockDown; set => isKnockDown = value; }

    public override void EnterState(e_EnemyState state)
    {
        isKnockDown = true;

        Anima.SetInteger("state", (int)state);

        // »˜∆Æ ¿Ã∆Â∆Æ Ω««‡
        controller.HitParticle.Play();

        SoundManager.instance.PlaySfx(e_Sfx.EnemyHit);
    }

    public override void UpdateState()
    {
        // ≥ÀπÈ ¡ﬂ¿Ã∏È ∏Æ≈œ
        if (IsKnockDown) return;

        // ≥ÀπÈ µÃ¿ª∂ß ¡◊¿∏∏È
        if (Health.Hp <= 0)
        {
            // ªÁ∏¡ √≥∏Æ ¡ˆø¨Ω√∞£ Ω√¿€
            time += Time.deltaTime;

            //ƒ›∂Û¿Ã¥ı∏¶ ≤®¡‹
            col.isTrigger = true;

            // ªÁ∏¡ √≥∏Æ ¡ˆø¨Ω√∞£¿Ã ¡ˆ≥µ¥Ÿ∏È
            if (time >= deathDelayTime)
            {
                // ªÁ∏¡ µÃ¥¬¡ˆ æÀ∑¡¡‹
                levelManager.OnMonsterDefeated();
                // ªÁ∏¡ ¿Ã∆—∆Æ ª˝º∫
                Instantiate(destroyParticlePrefab, transform.position, destroyParticlePrefab.transform.rotation);
                // ∑£¥˝ æ∆¿Ã≈€ ª˝º∫
                controller.DropItem();
                // ∞ÒµÂ ∏Æ«¡∑πΩ¨
                controller.Score();
                // Ω√√º ∆ƒ±´
                Destroy(gameObject);
            }
        }

        else if(Health.Hp > 0 && !IsKnockDown)
        {
            controller.TransactionToState(e_EnemyState.WakeUp);
        }
    }

    public override void ExitState()
    {
        
    }
}
