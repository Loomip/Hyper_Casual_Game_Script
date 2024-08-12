using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] headModels; // 머리 모델 배열
    [SerializeField] private GameObject[] bodyModels; // 몸 모델 배열

    void Start()
    {
        // 모든 머리와 몸 모델을 비활성화
        foreach (var model in headModels) model.SetActive(false);
        foreach (var model in bodyModels) model.SetActive(false);

        // 머리와 몸 모델 중에서 랜덤하게 선택
        GameObject headModel = headModels[Random.Range(0, headModels.Length)];
        GameObject bodyModel = bodyModels[Random.Range(0, bodyModels.Length)];

        // 선택한 머리와 몸 모델을 활성화
        headModel.SetActive(true);
        bodyModel.SetActive(true);

    }
}
