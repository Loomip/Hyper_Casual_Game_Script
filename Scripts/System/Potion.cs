using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    // 체력 올라갈 량
    [SerializeField] private int hpAmount;

    // 뜨는 모션의 속도
    [SerializeField] private float floatSpeed;
    // 뜨는 모션의 높이
    [SerializeField] private float floatHeight;
    // 아이템의 초기 위치를 저장
    private Vector3 originalPosition;

    // 아이템이 파괴되기까지의 시간 (초)
    [SerializeField] private float destroyTime;

    // 일정 시간 상호작용이 없으면 알아서 파괴되는 로직
    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    void Start()
    {
        // 초기 위치 저장
        originalPosition = transform.position;

        // 일정 시간 후 아이템 파괴
        StartCoroutine(DestroyAfterTime(destroyTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            PHealth pHealth = other.GetComponent<PHealth>();
            if(pHealth != null )
            {
                pHealth.Heal(hpAmount);
            }

            Destroy(gameObject);
        }
    }

    void Update()
    {
        // 시간에 따라 y축 위치 변경
        float newY = originalPosition.y + Mathf.Abs(Mathf.Sin(Time.time * floatSpeed) * floatHeight);
        transform.position = new Vector3(originalPosition.x, newY, originalPosition.z);
    }
}
