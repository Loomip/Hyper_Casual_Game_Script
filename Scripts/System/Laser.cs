using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Laser : MonoBehaviour
{
    // 레이저 공격력
    [SerializeField] private int atk;

    // 타겟 레이어 판단 코드
    [SerializeField] private LayerMask targetLayer;

    // 레이저 길이
    [SerializeField] private float laserDistance;

    // 레이저 시각화
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        Destroy(gameObject, 0.1f);
        LaserStraight();
    }

    private void Update()
    {
        //Debug.DrawRay(transform.position, transform.forward * laserDistance, Color.red, 2f);
    }

    public void LaserStraight()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, laserDistance, targetLayer);

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, ray.origin + ray.direction * laserDistance);

        foreach (var hit in hits)
        {
            if (hit.collider != null)
            {
                var enemyHealth = hit.collider.GetComponent<EHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.Hit(atk);
                }

                var enemyFSMController = hit.collider.GetComponent<EnemyFSMController>();
                if (enemyFSMController != null)
                {
                    enemyFSMController.Hit();
                }
            }
            else
            {
                Debug.LogWarning($"Hit collider is null");
            }
        }
    }
}
