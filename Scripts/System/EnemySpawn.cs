using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    // Enemy �����յ�
    [SerializeField] private GameObject[] enemyPrefabs;

    // Enemy ���� ��ġ
    [SerializeField] private Transform spawnTransform;

    // ���� ����
    [SerializeField] private float spawnRange;

    // �� ������ ��ȯ Ȯ��
    [SerializeField] private float[] spawnProbabilities;

    public void SpawnMonster()
    {
        // ��ȯ ���� ����
        float x = Random.Range(-spawnRange, spawnRange);
        float z = Random.Range(-spawnRange, spawnRange);

        // ��ȯ ��ġ ���� ����
        Vector3 spawnPosition = new Vector3(spawnTransform.position.x + x, 0f, spawnTransform.position.z + z);

        // Ȯ�� ������� ��ȯ�� ���� �ε��� ���� (ù ��° �ε��� ����)
        int enemyIndex = GetRandomProbabilityIndexExcludingFirst(spawnProbabilities);

        // ������ ���� ��ȯ
        Instantiate(enemyPrefabs[enemyIndex + 1], spawnPosition, spawnTransform.rotation);
    }

    public void SpawnSpecificMonster(int index)
    {
        // ��ȯ ���� ����
        float x = Random.Range(-spawnRange, spawnRange);
        float z = Random.Range(-spawnRange, spawnRange);

        // ��ȯ ��ġ ���� ����
        Vector3 spawnPosition = new Vector3(spawnTransform.position.x + x, 0f, spawnTransform.position.z + z);

        // ������ �ε����� ���� ��ȯ
        Instantiate(enemyPrefabs[index], spawnPosition, spawnTransform.rotation);
    }

    // ù ��° Ȯ���� �����ϰ� ���� �ε��� ��ȯ (�� Ȯ���� 100%)
    private int GetRandomProbabilityIndexExcludingFirst(float[] probabilities)
    {
        float total = 0;

        // ù ��° Ȯ���� ������ ��ü Ȯ�� �հ� ���
        for (int i = 1; i < probabilities.Length; i++)
        {
            total += probabilities[i];
        }

        // ���� Ȯ�� �� ����
        float randomPoint = Random.value * total;

        // ù ��° Ȯ���� ������ �ε��� ����
        for (int i = 1; i < probabilities.Length; i++)
        {
            if (randomPoint < probabilities[i])
            {
                return i - 1; // ù ��° Ȯ���� ���������Ƿ� �ε����� 1�� ����
            }
            else
            {
                randomPoint -= probabilities[i];
            }
        }
        return probabilities.Length - 2; // ù ��° Ȯ���� ���������Ƿ� ������ �ε����� 1 ����
    }
}
