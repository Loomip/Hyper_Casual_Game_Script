using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 총알 공격력을 전달할 변수
    [SerializeField] private int atk;

    // 타겟 레이어 판단 코드
    [SerializeField] List<string> hitLayerNames;

    // 총알이 사라질 시작 위치 저장
    private Vector3 launchPosition;

    // 총알이 사라져야될 최대 길이
    [SerializeField] private float maxTravelDistance = 100f;

    private void OnTriggerEnter(Collider other)
    {
        // hitLayerName에 해당하는 레이어에 있는 오브젝트가 있는지 확인
        if ((other != null && hitLayerNames.Contains(LayerMask.LayerToName(other.gameObject.layer))) || other.tag == "Ground")
        {
            if (other.tag == "Enemy")
            {
                other.GetComponent<EHealth>().Hit(atk);
                other.GetComponent<EnemyFSMController>().Hit();
            }

            Destroy(gameObject);
        }
    }

    private void Update()
    {
        // 버그로 총알이 빗맞앗을 경우 총알이 계속 살아있기 때문에 일정 길이 이상으로 벌어지면
        // 총알을 삭제 해줘야함
        if (Vector3.Distance(transform.position, launchPosition) > maxTravelDistance)
        {
            Destroy(gameObject);
        }
    }
}
