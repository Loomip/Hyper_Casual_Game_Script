using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffect : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;

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
        if (other.tag == "Enemy")
        {
            other.GetComponent<EHealth>().Hit(atk);
            other.GetComponent<EnemyFSMController>().Knockdown();
        }

        Collider collider = GetComponent<Collider>();

        StartCoroutine(SetOffCollider(collider));
    }

    IEnumerator SetOffCollider(Collider collider)
    {
        yield return null;
        collider.enabled = false;

        yield return new WaitForSeconds(3f);
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