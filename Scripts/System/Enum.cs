using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum e_EnemyState
{
    Idle,       // ��û���
    Run,        // �ɾ�ٴϴ� ����
    Attack,     // ���� ����
    Hit,        // ��Ʈ ����
    Knockdown, // �˴ٿ� ����
    WakeUp, // �Ͼ�� ����
    Die         // ���� ����
}

public enum e_PlayerState
{
    Idle,
    Run,
    Attack,
    Hit,
    Knockdown,
    WakeUp,
    Die
}

public enum e_MenuType
{
    None,
    Face,   // ��
    Top,    // ����
    Length
}

public enum e_Bgm
{
    StartScene,
    GameScene
}

public enum e_Sfx
{
    MeleeAtteck,
    BulletShot,
    LaserShot,
    StrayBullet,
    Explosion,
    PlayerHit,
    PlayerDeath,
    EnemyHit,
    EnemyDie,
    EnemyMelee,
    EnemyRanged,
    EnemySkill,
}
