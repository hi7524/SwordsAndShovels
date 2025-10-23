using UnityEngine;
using UnityEngine.AI;

public class Player : LivingEntity
{
    private NavMeshAgent agent;
    private Animator animator;

    public LayerMask groundLayer;
    public int damage = 10;

    private IDamagable target;

    public float sampleDistance = 5f;

    public Transform[] tunnelEndLocation;

    public static readonly int speedHash = Animator.StringToHash("Speed");
    public static readonly int attackHash = Animator.StringToHash("Attack");

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

        if (animator != null)
        {
            animator.SetFloat(speedHash, agent.velocity.magnitude <= 0.5f ? 0 : agent.velocity.magnitude * 1.1f);
        }
    }

    public void MoveThroughTunnel()
    {
        var door1 = Vector3.Distance(tunnelEndLocation[0].position, transform.position);
        var door2 = Vector3.Distance(tunnelEndLocation[1].position, transform.position);

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

    public void PlayerAttack(GameObject targetObj)
    {
        var damageble = targetObj.GetComponent<IDamagable>();
        if(damageble != null)
        {
           target = damageble;
            animator.SetTrigger(attackHash);
        }
    }
    public void Hit()
    {
        Debug.Log("Damaging");
        target.OnDamage(damage);
    }

    protected override void Die()
    {
        base.Die();
        gameObject.SetActive(false);
    }

    public override void OnDamage(int damage)
    {
        base.OnDamage(damage);
    }
}