using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum e_EnemyState
{
    Idle,       // 평시상태
    Run,        // 걸어다니는 상태
    Attack,     // 공격 상태
    Hit,        // 히트 상태
    Knockdown, // 넉다운 상태
    WakeUp, // 일어나는 상태
    Die         // 죽음 상태
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
    Face,   // 얼굴
    Top,    // 상의
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
