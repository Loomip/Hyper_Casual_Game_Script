using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttackComponent : MonoBehaviour
{
    // �ִϸ����� ������Ʈ
    private Animator animator;

    // ���� ���̽�ƽ ������Ʈ
    [SerializeField] private VariableJoystick attackJoy;

    // ���� Ÿ�� �߽��� ��ġ
    [SerializeField] private Transform attackTransfom;

    // ���� ����
    [SerializeField] private float attackRadius;

    // ���� ���� ����
    [SerializeField] private float hitAngle;

    // ���� ��� ���̾�
    [SerializeField] protected LayerMask targetLayer;

    // �Ѿ� ������Ʈ��
    [SerializeField] private GameObject[] shootersPrefab;

    // �Ѿ��� ���� ��ġ 
    [SerializeField] private Transform bulletPos;
    public Transform BulletPos { get => bulletPos; set => bulletPos = value; }

    // �ѽ�� ��ƼŬ
    [SerializeField] private ParticleSystem bulletEffect;

    // ���� ������ ���� ���Ⱑ �ٲ� �迭
    [SerializeField] private GameObject[] weaponObjects;

    // ���� ���ݷ��� ������ ����
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

        //// ���� ���̽�ƽ �Է� ó��
        //float hAttackJoy = attackJoy.Horizontal;
        //float vAttackJoy = attackJoy.Vertical;

        //// ���� ���̽�ƽ�� ���������� Ȯ��
        //if (hAttackJoy != 0 || vAttackJoy != 0)
        //{
        //    // ĳ���� ȸ�� ó��
        //    float angle = Mathf.Atan2(hAttackJoy, vAttackJoy) * Mathf.Rad2Deg;
        //    transform.rotation = Quaternion.Euler(0, angle, 0);
        //}

        //// ���� �ִϸ��̼� ���
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
        // ��� ���⸦ ��Ȱ��ȭ
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

    // ������
    public void MeleeAttack()
    {
        SoundManager.instance.PlaySfx(e_Sfx.MeleeAtteck);
        // ���� ���� ���� �浹ü Ž��
        Collider[] hits = Physics.OverlapSphere(attackTransfom.position, attackRadius, targetLayer);

        foreach (Collider hit in hits)
        {
            Vector3 directionToTarget = hit.transform.position - transform.position;
            float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

            if (angleToTarget < hitAngle)
            {
                if (hit.gameObject.layer == LayerMask.NameToLayer("Player"))
                {
                    // ��Ʈ ����
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

    // �����
    public void BulletShot()
    {
        SoundManager.instance.PlaySfx(e_Sfx.BulletShot);
        // ������ �������� ����
        Vector3 offset = Random.insideUnitSphere * 0.1f;
        // �Ѿ��� ����
        GameObject bullet = Instantiate(shootersPrefab[0], BulletPos.position + offset, BulletPos.rotation);
        bullet.transform.localRotation *= Quaternion.Euler(90, 0, 0);
        Rigidbody rigidbody = bullet.GetComponent<Rigidbody>();
        rigidbody.velocity = BulletPos.forward * 20f;
    }

    // ������ ��
    public void LaserShot()
    {
        SoundManager.instance.PlaySfx(e_Sfx.LaserShot);
        GameObject laserLineInstance = Instantiate(shootersPrefab[2], BulletPos.position, BulletPos.rotation);
    }


    // ��ź�߻��
    public void StrayBullet()
    {
        SoundManager.instance.PlaySfx(e_Sfx.StrayBullet);
        // �Ѿ��� ����
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
