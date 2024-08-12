using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    // Enemy 프리팹들
    [SerializeField] private GameObject[] enemyPrefabs;

    // Enemy 생성 위치
    [SerializeField] private Transform spawnTransform;

    // 생성 범위
    [SerializeField] private float spawnRange;

    // 각 몬스터의 소환 확률
    [SerializeField] private float[] spawnProbabilities;

    public void SpawnMonster()
    {
        // 소환 범위 지정
        float x = Random.Range(-spawnRange, spawnRange);
        float z = Random.Range(-spawnRange, spawnRange);

        // 소환 위치 랜덤 지정
        Vector3 spawnPosition = new Vector3(spawnTransform.position.x + x, 0f, spawnTransform.position.z + z);

        // 확률 기반으로 소환할 몬스터 인덱스 결정 (첫 번째 인덱스 제외)
        int enemyIndex = GetRandomProbabilityIndexExcludingFirst(spawnProbabilities);

        // 지정된 몬스터 소환
        Instantiate(enemyPrefabs[enemyIndex + 1], spawnPosition, spawnTransform.rotation);
    }

    public void SpawnSpecificMonster(int index)
    {
        // 소환 범위 지정
        float x = Random.Range(-spawnRange, spawnRange);
        float z = Random.Range(-spawnRange, spawnRange);

        // 소환 위치 랜덤 지정
        Vector3 spawnPosition = new Vector3(spawnTransform.position.x + x, 0f, spawnTransform.position.z + z);

        // 지정된 인덱스의 몬스터 소환
        Instantiate(enemyPrefabs[index], spawnPosition, spawnTransform.rotation);
    }

    // 첫 번째 확률을 제외하고 랜덤 인덱스 반환 (총 확률은 100%)
    private int GetRandomProbabilityIndexExcludingFirst(float[] probabilities)
    {
        float total = 0;

        // 첫 번째 확률을 제외한 전체 확률 합계 계산
        for (int i = 1; i < probabilities.Length; i++)
        {
            total += probabilities[i];
        }

        // 랜덤 확률 값 결정
        float randomPoint = Random.value * total;

        // 첫 번째 확률을 제외한 인덱스 결정
        for (int i = 1; i < probabilities.Length; i++)
        {
            if (randomPoint < probabilities[i])
            {
                return i - 1; // 첫 번째 확률을 제외했으므로 인덱스는 1씩 감소
            }
            else
            {
                randomPoint -= probabilities[i];
            }
        }
        return probabilities.Length - 2; // 첫 번째 확률을 제외했으므로 마지막 인덱스는 1 감소
    }
}
