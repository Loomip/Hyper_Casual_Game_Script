using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackComponent : MonoBehaviour
{
    // 애니메이터 컴포넌트
    private Animator animator;

    // 공격 조이스틱 컴포넌트
    [SerializeField] private VariableJoystick attackJoy;

    // 공격 타겟 중심점 위치
    [SerializeField] private Transform attackTransfom;

    // 공격 범위
    [SerializeField] private float attackRadius;

    // 공격 범위 각도
    [SerializeField] private float hitAngle;

    // 공격 대상 레이어
    [SerializeField] protected LayerMask targetLayer;

    // 총알 오브젝트들
    [SerializeField] private GameObject[] shootersPrefab;

    // 총알이 나갈 위치 
    [SerializeField] private Transform bulletPos;
    public Transform BulletPos { get => bulletPos; set => bulletPos = value; }

    // 총쏘는 파티클
    [SerializeField] private ParticleSystem bulletEffect;

    // 공격 종류에 따른 무기가 바뀔 배열
    [SerializeField] private GameObject[] weaponObjects;

    // 근접 공격력을 전달할 변수
    [SerializeField] private int meleeAtk;

    private PHealth health;

    private void Start()
    {
        animator = GetComponent<Animator>();
        health = GetComponent<PHealth>();
    }

    private void Update()
    {
        Attack();
    }

    public void Attack()
    {
        if (health.IsDie) return;

        //// 공격 조이스틱 입력 처리
        //float hAttackJoy = attackJoy.Horizontal;
        //float vAttackJoy = attackJoy.Vertical;

        //// 공격 조이스틱이 움직였는지 확인
        //if (hAttackJoy != 0 || vAttackJoy != 0)
        //{
        //    // 캐릭터 회전 처리
        //    float angle = Mathf.Atan2(hAttackJoy, vAttackJoy) * Mathf.Rad2Deg;
        //    transform.rotation = Quaternion.Euler(0, angle, 0);
        //}

        //// 공격 애니메이션 재생
        //animator.SetBool("isAtteck", hAttackJoy != 0 || vAttackJoy != 0);

        if (Input.GetMouseButton(0))
        {
            animator.SetBool("isAtteck", true);
        }
        else
        {
            animator.SetBool("isAtteck", false);
        }
    }

    public void BulletEffect()
    {
        bulletEffect.Play();
    }

    public void ChangeWeapon(string newWeaponType)
    {
        // 모든 무기를 비활성화
        foreach (var weapon in weaponObjects)
        {
            weapon.SetActive(false);
        }

        switch (newWeaponType)
        {
            case "club":
                animator.SetInteger("WeaponType", 0);
                weaponObjects[0].SetActive(true);
                break;
            case "MachineGun":
                animator.SetInteger("WeaponType", 1);
                weaponObjects[1].SetActive(true);
                break;
            case "GrenadeLauncher":
                animator.SetInteger("WeaponType", 2);
                weaponObjects[2].SetActive(true);
                break;
            case "LaserGun":
                animator.SetInteger("WeaponType", 3);
                weaponObjects[3].SetActive(true);
                break;
        }
    }

    // 몽둥이
    public void MeleeAttack()
    {
        SoundManager.instance.PlaySfx(e_Sfx.MeleeAtteck);
        // 공격 범위 내의 충돌체 탐지
        Collider[] hits = Physics.OverlapSphere(attackTransfom.position, attackRadius, targetLayer);

        foreach (Collider hit in hits)
        {
            Vector3 directionToTarget = hit.transform.position - transform.position;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleToTarget < hitAngle)
            {
                if (hit.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    // 히트 판정
                    PHealth pHealth = hit.GetComponent<PHealth>();
                    if (pHealth != null)
                    {
                        pHealth.Hit(meleeAtk);
                        pHealth.Knokdown();
                    }
                }
                else if (hit.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EHealth eHeath = hit.GetComponent<EHealth>();
                    EnemyFSMController enemyFSMController = hit.GetComponent<EnemyFSMController>();
                    if (eHeath != null)
                    {
                        eHeath.Hit(meleeAtk);
                        enemyFSMController.Knockdown();
                    }
                }
            }
        }
    }

    // 기관총
    public void BulletShot()
    {
        SoundManager.instance.PlaySfx(e_Sfx.BulletShot);
        // 랜덤한 오프셋을 생성
        Vector3 offset = Random.insideUnitSphere * 0.1f;
        // 총알을 생성
        GameObject bullet = Instantiate(shootersPrefab[0], BulletPos.position + offset, BulletPos.rotation);
        bullet.transform.localRotation *= Quaternion.Euler(90, 0, 0);
        Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
        rigidbody.velocity = BulletPos.forward * 20f;
    }

    // 레이저 건
    public void LaserShot()
    {
        SoundManager.instance.PlaySfx(e_Sfx.LaserShot);
        GameObject laserLineInstance = Instantiate(shootersPrefab[2], BulletPos.position, BulletPos.rotation);
    }


    // 유탄발사기
    public void StrayBullet()
    {
        SoundManager.instance.PlaySfx(e_Sfx.StrayBullet);
        // 총알을 생성
        GameObject bullet = Instantiate(shootersPrefab[1], BulletPos.position, BulletPos.rotation);
        bullet.transform.localRotation *= Quaternion.Euler(90, 0, 0);
        Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
        rigidbody.velocity = BulletPos.forward * 20f;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackTransfom.position, attackRadius);
    }
}
