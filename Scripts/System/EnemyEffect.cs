using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEffect : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;

    // ��ų ���ݷ��� ���޹��� ���
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
        // �浹�� ��ü�� Player�̰ų� ���̸� ������ ����
        if (other.CompareTag("Player"))
        {
            // ��Ʈ ����
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
        // CinemachineVirtualCamera�� Noise ������ ������
        var noise = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        // ��鸲 ����
        noise.m_AmplitudeGain = magnitude;
        noise.m_FrequencyGain = magnitude;

        yield return new WaitForSeconds(duration);

        // ��鸲 ����
        noise.m_AmplitudeGain = 0f;
        noise.m_FrequencyGain = 0f;
    }
}
