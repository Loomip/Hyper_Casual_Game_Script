using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffect : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    // 스킬 공격력을 전달받을 계수
    private int atk;
    public int Atk { get => atk; set => atk = value; }
    private void Awake()
    {
        cinemachineVirtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
    }

    private void Start()
    {
        StartCoroutine(Shake(0.3f, 2f));
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
                pHealth.Knokdown();
            }
        }

        StartCoroutine(DestroyEffect());
    }

    private IEnumerator DestroyEffect()
    {
        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }

    private IEnumerator Shake(float duration, float magnitude)
    {
        // CinemachineVirtualCamera의 Noise 설정을 가져옴
        var noise = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        // 흔들림 시작
        noise.m_AmplitudeGain = magnitude;
        noise.m_FrequencyGain = magnitude;

        yield return new WaitForSeconds(duration);

        // 흔들림 종료
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }
}
