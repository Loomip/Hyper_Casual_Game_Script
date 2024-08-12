using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserRankCell : MonoBehaviour
{
    [SerializeField] private Text rankText; // 랭킹 출력 텍스트
    [SerializeField] private Text nickNameText; // 닉네임 출력 텍스트
    [SerializeField] private Text bestScoreText; // 최고 점수 출력 텍스트

    // 셀 생성 시 정보 표시 초기화
    public void Init(string rank, string nickName, string bextScore)
    {
        rankText.text = rank;
        nickNameText.text = nickName;
        bestScoreText.text = bextScore;
    }
}
