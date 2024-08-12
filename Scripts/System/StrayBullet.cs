using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CartoonFX.CFXR_Effect;

public class StrayBullet : MonoBehaviour
{
    // ����Ʈ ���ݷ��� ������ ����
    [SerializeField] private int atk;

    // Ÿ�� ���̾� �Ǵ� �ڵ�
    [SerializeField] List<string> hitLayerNames;

    // �Ѿ��� ����� ���� ��ġ ����
    private Vector3 launchPosition;

    // �Ѿ��� ������ߵ� �ִ� ����
    [SerializeField] private float maxTravelDistance = 100f;

    // ���� ����Ʈ
    [SerializeField] private GameObject explosionEffectPrefab;

    private void OnTriggerEnter(Collider other)
    {
        // hitLayerName�� �ش��ϴ� ���̾ �ִ� ������Ʈ�� �ִ��� Ȯ��
        if ((other != null && hitLayerNames.Contains(LayerMask.LayerToName(other.gameObject.layer))) || other.tag == "Ground")
        {
            if (other.tag == "Enemy")
            {
                GameObject effect = Instantiate(explosionEffectPrefab, transform.position - new Vector3(0f, 1f, 0f) , Quaternion.identity);
                PlayerEffect effect1 = effect.GetComponent<PlayerEffect>();
                effect1.Atk = atk;
                SoundManager.instance.PlaySfx(e_Sfx.Explosion);

                StartCoroutine(Shake(0.5f, 0.3f));
            }
            else if(other.tag == "Player")
            {
                PHealth pHealth = other.GetComponent<PHealth>();
                pHealth.Knokdown();
            }

            Destroy(gameObject);
        }
    }

    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
    }

    private void Update()
    {
        // ���׷� �Ѿ��� ���¾��� ��� �Ѿ��� ��� ����ֱ� ������ ���� ���� �̻����� ��������
        // �Ѿ��� ���� �������
        if (Vector3.Distance(transform.position, launchPosition) > maxTravelDistance)
        {
            Destroy(gameObject);
        }
    }
}
