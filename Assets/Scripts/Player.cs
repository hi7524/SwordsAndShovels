using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    public LayerMask groundLayer;

    public float sampleDistance = 5f;

    public Transform[] tunnelEndLocation;

    public bool test = false;

    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, groundLayer))
            {
                MovePosition(hit.point);
            }
        }

        if (test)
        {
            agent.SetDestination(tunnelEndLocation[1].position);
        }


        if (animator != null)
        {
            float currentSpeed = agent.velocity.magnitude;
            float normalizedSpeed = currentSpeed / agent.speed;

            animator.SetFloat("Speed", normalizedSpeed);
        }
    }

    public void MoveThroughTunnel()
    {
        var door1 = Vector3.Distance(tunnelEndLocation[0].position, transform.position);
        var door2 = Vector3.Distance(tunnelEndLocation[1].position, transform.position);

        Debug.Log(door1);
        Debug.Log(door2);

        if(door1 < door2)
        {
            agent.SetDestination(tunnelEndLocation[1].position);
        }
        else
        {
            agent.SetDestination(tunnelEndLocation[0].position);
        }
    }

    public void MovePosition(Vector3 target)
    {
        if (NavMesh.SamplePosition(target, out NavMeshHit navMeshHit, sampleDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(navMeshHit.position);
        }
    }
}