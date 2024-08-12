using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserRankCell : MonoBehaviour
{
    [SerializeField] private Text rankText; // ��ŷ ��� �ؽ�Ʈ
    [SerializeField] private Text nickNameText; // �г��� ��� �ؽ�Ʈ
    [SerializeField] private Text bestScoreText; // �ְ� ���� ��� �ؽ�Ʈ

    // �� ���� �� ���� ǥ�� �ʱ�ȭ
    public void Init(string rank, string nickName, string bextScore)
    {
        rankText.text = rank;
        nickNameText.text = nickName;
        bestScoreText.text = bextScore;
    }
}
