using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 입력 이동 처리 컴포넌트
public class CharacterInputMovement : MonoBehaviour
{
    // 이동 조이스틱 컴포넌트
    //[SerializeField] private VariableJoystick moveJoy;

    // 애니메이터 컴포넌트
    private Animator animator;

    // 캐릭터 컨트롤러 컴포넌트
    private CharacterController characterController;

    // 이동속도
    [SerializeField] private float speed;

    // 이동 벡터
    private Vector3 movement;

    // 캐릭터 회전을 위한 마우스 포인트 레이캐스트 바닥 충돌 레이어 마스크
    [SerializeField] private LayerMask floorMask;

    // 캐릭터 회전을 위한 마우스 포인트 레이캐스트 길이
    [SerializeField] private float camRayLength;

    // 중력 수치
    [SerializeField] private float gravity;

    private float h; // 좌우 이동 방향
    private float v; // 상하 이동 방향

    private PHealth health;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        health = GetComponent<PHealth>();
    }

    private void Update()
    {
        Move();
        Turning();
    }

    private void Move()
    {
        if (health.IsDie) return;

        // 키입력 처리
        //float hJoy = moveJoy.Horizontal;
        //float vJoy = moveJoy.Vertical;
        //float hKey = Input.GetAxis("Horizontal");
        //float vKey = Input.GetAxis("Vertical");

        //h = hJoy != 0 ? hJoy : hKey;
        //v = vJoy != 0 ? vJoy : vKey;

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        // 이동 방향 벡터 설정
        movement = new Vector3(h, 0f, v).normalized;

        // 이동 애니메이션 재생
        animator.SetFloat("Move", movement.magnitude);

        // 중력 적용
        movement.y -= gravity * Time.deltaTime;

        // 이동 처리
        characterController.Move(movement * (speed * Time.deltaTime));
    }


    // 캐릭터 회전 처리 (마우스 포인터 방향으로 캐릭터가 바라보게)
    private void Turning()
    {
        if (health.IsDie) return;

        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;

        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouseDirection = floorHit.point - transform.position;
            playerToMouseDirection.y = 0f;

            Quaternion rotation = Quaternion.LookRotation(playerToMouseDirection);

            // 캐릭터 회전 처리
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10f);
        }
    }
}