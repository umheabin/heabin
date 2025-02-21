using UnityEngine;

public class BearMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 이동 속도
    private Vector3 movement;
    private Animator animator; // Animator 참조

    void Start()
    {
        // Animator 컴포넌트 가져오기
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 입력 값 받기
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 이동 벡터 설정
        movement = new Vector3(horizontal, 0f, vertical);

        // 캐릭터 이동
        Move();

        // 애니메이션 업데이트
        UpdateAnimation();
    }

    void Move()
    {
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);

        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }

    void UpdateAnimation()
    {
        // 이동 속도 계산
        float speed = movement.magnitude;
        animator.SetFloat("Speed", speed);
    }
}

