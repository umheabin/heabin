using UnityEngine;
using UnityEngine.AI;

public class ChickFollowPlayer : MonoBehaviour
{
    public Transform player;  // 플레이어의 Transform
    private NavMeshAgent agent;
    private bool shouldFollow = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (shouldFollow && player != null)
        {
            agent.SetDestination(player.position);
        }
    }

    // 플레이어가 일정 거리 이상 움직이면 따라오게 함
    public void CheckPlayerMovement()
    {
        if (Vector3.Distance(transform.position, player.position) > 2.0f)
        {
            shouldFollow = true;
        }
        else
        {
            shouldFollow = false;
            agent.ResetPath();
        }
    }

    void FixedUpdate()
    {
        CheckPlayerMovement();
    }
}
