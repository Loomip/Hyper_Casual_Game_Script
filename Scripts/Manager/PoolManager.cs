using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    // 오브젝트 풀 딕셔너리
    // * 오브젝트 풀 리스트들을 가진 딕셔너리
    // - 키 : 게임오브젝트참조
    // - 값 : 리스트 참조
    private Dictionary<GameObject, List<GameObject>> objactPoolMap = new Dictionary<GameObject, List<GameObject>>();

    // 오브젝트 키 프리팹 리스트
    [SerializeField] private List<GameObject> prefabstoPool = new List<GameObject>();

    // 오브젝트 풀 사이즈
    [SerializeField] private int poolSize;

    // 지정한 프리팹의 게임 오브젝트 풀 생성
    private void CreateObjectPool(GameObject prefab, int poolSize)
    {
        // 지정한 프리팹이 이미 오브젝트 풀로 생성이 되어 있지 않다면
        if (!objactPoolMap.ContainsKey(prefab))
        {
            // 오브젝트 풀 리스트 객체 생성
            List<GameObject> objectPoolList = new List<GameObject>();
            for (int i = 0; i < poolSize; i++)
            {
                GameObject obj = Instantiate(prefab, Vector3.zero, Quaternion.identity);
                obj.SetActive(false); // 게임 오브젝트 비활성화
                objectPoolList.Add(obj); // 오브젝트 풀에 생성된 게임오브젝트 추가
            }

            //  오브젝트 풀 딕셔너리에 생성된 오브젝트 풀 추가
            objactPoolMap.Add(prefab, objectPoolList);
        }
    }

    // 지정한 프리팹의 오브젝트 풀에서 하나의 오브젝트를 반환함.
    public GameObject GetObjectFromPool(GameObject prefabKey, Vector3 position, Quaternion rotation)
    {
        // 오브젝트 풀 딕셔너리에 지정한 프리팹 키를 가진 오브젝트 풀 리스트가 존재하는지를 체크
        if (objactPoolMap.ContainsKey(prefabKey))
        {
            // 오브젝트 풀 딕셔너리에서 리스트를 참조함
            List<GameObject> objectPoolList = objactPoolMap[prefabKey];

            foreach (GameObject obj in objectPoolList)
            {
                // 현재 게임오브젝트 활성화 된 상태가 아니면
                if (!obj.activeInHierarchy)
                {
                    // 오브젝트의 위치와 회전을 설정하고 활성화한뒤 참조를 반환
                    obj.transform.position = position;
                    obj.transform.rotation = rotation;
                    obj.SetActive(true);
                    return obj;
                }

            }

            // 새로 게임오브젝트를 생성함
            GameObject newObj = Instantiate(prefabKey, position, rotation);
            objectPoolList.Add(newObj);
            return newObj;
        }

        Debug.LogError($"오브젝트 풀 딕셔너리에 지정한 요소가 없음 : {prefabKey.name}");
        return null;
    }

    // 게임오브젝트를 반환함(비활성화 처리)
    public void ReturnObjectToPool(GameObject returnObj)
    {
        returnObj.transform.position = Vector3.zero;
        returnObj.transform.rotation = Quaternion.identity;
        returnObj.SetActive(false);
    }

    private void Start()
    {
        // 오브젝트 풀 딕셔너리에 오브젝트 풀 리스트 생성
        foreach (GameObject prefab in prefabstoPool)
        {
            CreateObjectPool(prefab, poolSize);
        }
    }
}
