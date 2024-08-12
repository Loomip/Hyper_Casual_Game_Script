using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    // 최대 체력을 담을 변수
    [SerializeField] private int maxHp;
    public int MaxHp { get => maxHp; set => maxHp = value; }

    // 현재 체력을 담을 변수
    [SerializeField] private int hp;
    public int Hp
    {
        get => hp;
        set
        {
            hp = Mathf.Clamp(value, 0, maxHp); // 체력이 0과 maxHp 사이의 값으로 유지되도록 설정
        }
    }

    // 데미지 쿨시간 
    [SerializeField] protected float damageCooldown;

    // 맞았는지 
    private bool canTakeDamage = true;
    public bool CanTakeDamage { get => canTakeDamage; set => canTakeDamage = value; }

    public abstract void Hit(int damage);

    void Awake()
    {
        Hp = maxHp; // 처음 체력을 최대 체력으로 설정
    }
}
