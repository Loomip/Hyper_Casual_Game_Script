using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyState : MonoBehaviour
{
    // 애니메이터 참조
    protected Animator animator;

    // 네비 컴포넌트 참조
    protected NavMeshAgent nav;

    // 몬스터 유한상태기계 컨트롤러
    protected EnemyFSMController controller;

    // 레벨 매니저 컴포넌트
    protected LevelManager levelManager;

    protected Health health;

    public Health Health { get => health; set => health = value; }
    public Animator Anima { get => animator; set => animator = value; }

    protected Collider col;

    // 몬스터 상태 관련 인터페이스(문법아님) 메소드 선언

    // 몬스터 상태 시작 (다른상태로 전이됨) 메소드
    public abstract void EnterState(e_EnemyState state);

    // 몬스터 상태 업데이트 추상 메소드 (상태 동작 수행)
    public abstract void UpdateState();

    // 몬스터 상태 종료 (다른상태로 전이됨) 메소드
    public abstract void ExitState();

    private void Awake()
    {
        Anima = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        controller = GetComponent<EnemyFSMController>();
        Health = GetComponent<Health>();
        levelManager = FindObjectOfType<LevelManager>();
        col = GetComponent<Collider>();
    }
}
