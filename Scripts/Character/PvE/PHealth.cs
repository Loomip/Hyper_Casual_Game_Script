using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PHealth : Health
{
    private Animator animator;

    // ��Ʈ ��ƼŬ
    [SerializeField] private ParticleSystem hitParticle;

    // �׾�����
    private bool isDie = false;
    public bool IsDie { get => isDie; set => isDie = value; }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    protected IEnumerator IsHitCoroutine(int damage)
    {
        CanTakeDamage = false;

        //// ������� ���� ��ŭ ü���� ����
        Hp -= damage;

        // GameManager���� ü�� ��������
        GameManager.instance.RefreshHp(gameObject.tag, this);

        yield return new WaitForSeconds(damageCooldown);

        CanTakeDamage = true;
    }

    public override void Hit(int damage)
    {
        if (Hp > 0 && CanTakeDamage)
        {
            // ����� ȿ��
            StartCoroutine(IsHitCoroutine(damage));
            hitParticle.Play();
            SoundManager.instance.PlaySfx(e_Sfx.PlayerHit);
        }

        else if(Hp <= 0)
        {
            IsDie = true;
            animator.SetTrigger("Death");
            
            StartCoroutine(HandleDeath());
        }
    }

    private IEnumerator HandleDeath()
    {
        SoundManager.instance.PlaySfx(e_Sfx.PlayerDeath);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        SoundManager.instance.StopBgm();
        SoundManager.instance.StopSfx();
        LoadSceneManager.LoadScene("EndScene");
    }

    public void Knokdown()
    {
        animator.SetTrigger("Knokdown");
    }

    public void Heal(int amount)
    {
        Hp += amount;
        GameManager.instance.RefreshHp(gameObject.tag, this);
    }
}
