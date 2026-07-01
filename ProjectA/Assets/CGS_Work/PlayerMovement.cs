using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 destination;

    [SerializeField] private float moveSpeed = 5f;

    void Start()
    {
        // 목적지 = 현재 위치
        destination = transform.position;
    }
    void Update()
    {
        // 트렌스폼의 위치를 목적지로 이동 (속도: 5f)
        transform.position = Vector2.MoveTowards(transform.position, destination, Time.deltaTime * moveSpeed);
    }
        // 목적지 설정
    public void MoveTo(Transform targetTransform)
    {
        // 목적지가 null이면 경고 로그 출력
        if (targetTransform == null)
        {
            Debug.LogWarning("목적지가 없습니다.");
            return;
        }
        // 목적지로 이동, 로그 출력 
        destination = targetTransform.position;
        Debug.Log("이동 시작!");
        // 목적지에 도착했는지 확인하는 로그 출력
        if (Vector2.Distance(transform.position, destination) < 0.01f)
        {
            Debug.Log("도착!");
        }
    }

}
