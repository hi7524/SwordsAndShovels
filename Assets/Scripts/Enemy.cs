using UnityEngine;
using UnityEngine.AI;

// KHI
// 적 캐릭터 (원거리, 근거리) 이동, 공격, 피격, 죽음
public class Enemy : LivingEntity
{
    public float attackRange = 2;
    public float attackInterval = 1;
    public float damage = 10;

    private float attackRangeSqr;
    private float lastAttackTime;
    private bool isMoving = false;

    private Animator animator;
    private NavMeshAgent agent;

    private IDamagable target;
    private Transform targetTrans;

    private const string TARGET_TAG = "Player";


    private void Awake()
    {
        attackRangeSqr = attackRange * attackRange;

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        // 플레이어 (타겟) 캐싱
        GameObject targetObj = GameObject.FindGameObjectWithTag(TARGET_TAG);

        if (targetObj == null)
        {
            Debug.Log($"공격 타겟: 캐싱 실패");
            return;
        }

        Debug.Log($"공격 타겟: {target} 캐싱");
        
        target = targetObj.GetComponent<IDamagable>();
        targetTrans = targetObj.transform;
        targetObj.GetComponent<LivingEntity>().OnDeath += () => target = null;
        targetObj.GetComponent<LivingEntity>().OnDeath += () => targetTrans = null;
    }

    private void Update()
    {
        if (target == null)
            return;

        if (IsTargetInAttackRange())
        {
            agent.isStopped = false;
            Move();
        }
        else
        {
            agent.isStopped = true;
        }

        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // 일정 간격 공격
            if (lastAttackTime + attackInterval <= Time.time)
            {
                lastAttackTime = Time.time;
                animator.SetTrigger("Attack");
            }
        }

        animator.SetFloat("Speed", agent.velocity.magnitude <= 0.5f ? 0 : agent.velocity.magnitude * 1.1f);
    }

    // 타겟 범위 내에 들어왔는지 여부 반환
    private bool IsTargetInAttackRange()
    {
        float sqrDisToTarget = (targetTrans.position - transform.position).sqrMagnitude;

        return sqrDisToTarget <= attackRangeSqr;
    }

    // 이동
    private void Move()
    {
        if (target == null)
            return;

        agent.SetDestination(targetTrans.position);
    }

    // 공격
    protected virtual void Hit()
    {
        Debug.Log("Attack");
    }

    // 피격
    public override void OnDamage(int damage)
    {
        base.OnDamage(damage);
        Debug.Log($"{gameObject.name} HP: {Health}");
    }

    // 죽음
    protected override void Die()
    {
        base.Die();
        Debug.Log($"{gameObject.name} 사망");
    }

    // 공격 사거리 표시
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
