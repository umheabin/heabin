using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CrowdAgent : MonoBehaviour
{

    [SerializeField]
    private Transform target; // 의자 위치
    private NavMeshAgent agent;
    private Animator animator;
    private bool isSitting = false;
    private bool isRotating = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.speed = Random.Range(4.0f, 5.0f);

        // 목표 지점을 의자의 정면 앞으로 조정
        Vector3 adjustedTarget = target.position - target.forward * 0.5f;
        agent.SetDestination(adjustedTarget);

        animator.SetBool("isWalking", true);
    }

    void Update()
    {
        if (!agent.isOnNavMesh || agent == null) return;

        if (!isSitting && agent.remainingDistance <= 0.5f && !agent.pathPending)
        {
            if (!isRotating)
            {
                StartCoroutine(RotateAndMoveForward());
            }
        }
    }

    IEnumerator RotateAndMoveForward()
    {
        isRotating = true;
        agent.isStopped = true; // 이동 중지
        animator.SetBool("isWalking", false);

        // 1️⃣ 의자 방향으로 회전
        Quaternion targetRotation = Quaternion.LookRotation(target.forward);
        float rotationSpeed = 20.0f;

        while (Quaternion.Angle(transform.rotation, targetRotation) > 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        // 2️⃣ 앞으로 한 걸음 이동
        Vector3 finalPosition = transform.position + transform.forward * 0.3f; // 조금 앞으로 이동
        float moveSpeed = 1.5f;
        float distance = Vector3.Distance(transform.position, finalPosition);

        while (distance > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, finalPosition, moveSpeed * Time.deltaTime);
            distance = Vector3.Distance(transform.position, finalPosition);
            yield return null;
        }

        SitOnChair();
    }

    void SitOnChair()
    {
        isSitting = true;
        animator.SetTrigger("Sit"); // 앉기 애니메이션 실행
    }
}
