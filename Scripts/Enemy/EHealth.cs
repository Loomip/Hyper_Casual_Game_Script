using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EHealth : Health
{
    // 몬스터 체력바
    [SerializeField] private GameObject healthBarPrefab;

    // 체력바가 몬스터 위에 위치할 높이
    [SerializeField] private float healthBarHeight = 2.0f; 

    // 체력바 오브젝트를 담을 변수
    GameObject healthBarObject;

    protected IEnumerator IsHitCoroutine(int damage)
    {
        CanTakeDamage = false;

        //// 대미지가 들어온 만큼 체력을 깍음
        Hp -= damage;

        // GameManager에서 체력 리프레쉬
        GameManager.instance.RefreshHp(gameObject.tag, this);

        yield return new WaitForSeconds(damageCooldown);

        CanTakeDamage = true;
    }

    public override void Hit(int damage)
    {
        if (Hp > 0 && CanTakeDamage)
        {
            // 대미지 효과
            StartCoroutine(IsHitCoroutine(damage));
        }
    }

    private void Start()
    {
        healthBarObject = Instantiate(healthBarPrefab, transform.position + Vector3.up * healthBarHeight, Quaternion.identity, transform);
        Slider healthBar = healthBarObject.GetComponentInChildren<Slider>();
        GameManager.instance.RegisterEnemyHealthBar(this, healthBar);
    }

    private void Update()
    {
        if (Hp <= 0)
        {
            Destroy(healthBarObject);
        }
    }
}
