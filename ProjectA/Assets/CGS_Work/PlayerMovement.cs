using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #pragma warning disable IDE0044 // Unity 인스펙터로 설정되는 필드는 readonly로 만들 수 없음
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Animator animator;
#pragma warning restore IDE0044

    private Vector2 destination;

    [SerializeField] private float moveSpeed = 5f;

    void Start()
    {
        // 목적지 = 현재 위치
        destination = transform.position;
        animator = GetComponent<Animator>();
    }

    void Update()
    {

        // 목적지와의 거리로 이동 여부 판단
        bool Move = Vector2.Distance(transform.position, destination) > 0.01f;

        // 트렌스폼의 위치를 목적지로 이동 (속도: moveSpeed)
        transform.position = Vector2.MoveTowards(transform.position, destination, Time.deltaTime * moveSpeed);

        animator.SetBool("Move", Move);

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
