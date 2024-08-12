using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    // 발사체가 도달하는 최대 높이
    [SerializeField] private float h;
    // 중력 가속도
    [SerializeField] private float gravity;
    // 공격력 전달
    private int atk;

    public int Atk { get => atk; set => atk = value; }

    public LaunchData CalculateLaunchData(Transform target, Transform projectile)
    {
        // 발사체와 목표물 사이의 수직 및 수평 거리 계산
        float displacementY = target.position.y - projectile.position.y;
        Vector3 displacementXZ = new Vector3(target.position.x - projectile.position.x, 0, target.position.z - projectile.position.z);

        // 발사체가 목표물에 도달하는 데 걸리는 시간 계산
        float time = Mathf.Sqrt(-2 * h / gravity) + Mathf.Sqrt(2 * (displacementY - h) / gravity);

        // 발사체의 수직 및 수평 속도 계산
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(2 * gravity * displacementY);
        Vector3 velocityXZ = displacementXZ / time;

        // 초기 속도와 목표물에 도달하는 데 필요한 시간 반환
        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
    }

    public struct LaunchData
    {
        // 초기 속도
        public readonly Vector3 initialVelocity;

        // 목표물에 도달하는 데 필요한 시간
        public readonly float timeToTarget;

        public LaunchData(Vector3 initialVelocity, float timeToTarget)
        {
            this.initialVelocity = initialVelocity;
            this.timeToTarget = timeToTarget;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 충돌한 객체가 Player이거나 땅이면 프리펩 제거
        if (other.CompareTag("Player"))
        {
            // 히트 판정
            PHealth pHealth = other.GetComponent<PHealth>();

            if (pHealth != null)
            {
                pHealth.Hit(atk);
            }

            Destroy(gameObject);
        }
        else if (other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }

    }
}
