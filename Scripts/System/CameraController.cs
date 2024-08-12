using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // 카메라
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    // 기준점
    [SerializeField] private Transform benchmark;

    // 원의 반지름
    [SerializeField] private float radius = 10f;
    // 회전 속도
    [SerializeField] private float speed = 10f;
    // 카메라 높이 오프셋
    [SerializeField] private float heightOffset = 5f;

    // 현재 각도
    private float angle = 0f;

    private void Update()
    {
        CameraTurn();
    }

    private void CameraTurn()
    {
        // 각도 증가
        angle += speed * Time.deltaTime;
        if (angle >= 360f)
        {
            angle -= 360f;
        }

        // 새로운 위치 계산
        float x = benchmark.position.x + Mathf.Cos(angle) * radius;
        float z = benchmark.position.z + Mathf.Sin(angle) * radius;
        float y = benchmark.position.y + heightOffset; // 기준점보다 높은 위치 유지

        // 카메라 위치 설정
        virtualCamera.transform.position = new Vector3(x, y, z);

        // 카메라가 기준점을 바라보도록 회전
        virtualCamera.transform.LookAt(new Vector3(benchmark.position.x, benchmark.position.y, benchmark.position.z));
    }
}
