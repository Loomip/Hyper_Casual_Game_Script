using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // 스포너 배열 참조
    [SerializeField] private EnemySpawn[] spawners;

    // 소환할 몬스터 수
    private int monstersToSpawn;
    public int MonstersToSpawn { get => monstersToSpawn; set => monstersToSpawn = value; }

    // 잡은 몬스터 수
    private int monstersDefeated;
    public int MonstersDefeated { get => monstersDefeated; set => monstersDefeated = value; }

    // 맵에 동시에 존재할 수 있는 몬스터의 최대 수
    [SerializeField] private int maxMonstersOnMap;

    // 현재 맵에 소환된 몬스터 수를 추적하기 위한 임시 변수
    private int currentMonstersOnMap;

    // 현재 라운드에 소환된 총 몬스터 수를 추적하기 위한 변수
    private int totalSpawnedMonsters;

    // 보스 몬스터 소환 간격 (라운드)
    private const int BossSpawnInterval = 3;

    public void StartRound(int roundNumber)
    {
        // 라운드에 따라 난이도 조절
        MonstersToSpawn = roundNumber * 16;

        // 모든 변수 초기화
        MonstersDefeated = 0;
        currentMonstersOnMap = 0;
        totalSpawnedMonsters = 0;

        // 몬스터 소환 코루틴 시작
        StartCoroutine(SpawnMonsters(roundNumber));
    }

    private IEnumerator SpawnMonsters(int roundNumber)
    {
        int bossCount = roundNumber / BossSpawnInterval; // 5 라운드마다 보스 수 증가

        // 먼저 보스 몬스터 소환
        for (int i = 0; i < bossCount; i++)
        {
            SpawnBossMonster();
            yield return new WaitForSeconds(1f); // 보스 몬스터 소환 간격
        }

        while (totalSpawnedMonsters < MonstersToSpawn)
        {
            if (currentMonstersOnMap < maxMonstersOnMap)
            {
                // 랜덤한 스포너 선택
                int spawnerIndex = Random.Range(0, spawners.Length);
                EnemySpawn spawner = spawners[spawnerIndex];

                // 선택한 스포너에서 몬스터 소환
                spawner.SpawnMonster();
                currentMonstersOnMap++;
                totalSpawnedMonsters++;

                Debug.Log("소환되는 몬스터 수: " + currentMonstersOnMap);

                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                Debug.Log("현재 맵에 있는 몬스터 수가 최대치에 도달했습니다. 대기 중...");
                yield return new WaitForSeconds(1f); // 1초 후에 다시 시작
            }
        }

        Debug.Log("모든 몬스터가 소환되었습니다.");
    }

    private void SpawnBossMonster()
    {
        // 랜덤한 스포너 선택
        int spawnerIndex = Random.Range(0, spawners.Length);
        EnemySpawn spawner = spawners[spawnerIndex];

        // 선택한 스포너에서 보스 몬스터 소환
        spawner.SpawnSpecificMonster(0); // 0번째 인덱스의 몬스터를 보스 몬스터로 설정
        currentMonstersOnMap++;
        totalSpawnedMonsters++;

        Debug.Log("보스 몬스터가 소환되었습니다.");
    }

    public void OnMonsterDefeated()
    {
        // 몬스터가 패배하면 카운트 증가
        currentMonstersOnMap--;
        MonstersDefeated++;
        GameManager.instance.UpdateUI();

        Debug.Log(" 죽인 몬스터 수 : " + MonstersDefeated);


        // 모든 몬스터가 패배하면 
        if (MonstersDefeated >= MonstersToSpawn)
        {
            // GameManager에게 레벨 클리어를 알림
            GameManager.instance.OnLevelCleared();
        }
    }
}
