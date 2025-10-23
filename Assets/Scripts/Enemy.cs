using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

// KHI
// 적 캐릭터 (원거리, 근거리) 이동, 공격, 피격, 죽음
public class Enemy : LivingEntity
{
    public float detectRange = 2;
    public float attackInterval = 1;
    public int damage = 10;
    public Slider healthBar;

    private float attackRangeSqr;
    private float lastAttackTime;
    private bool isAttacking = false;

    private Animator animator;
    private NavMeshAgent agent;

    protected IDamagable Target { get; private set; }
    protected Transform TargetTrans { get; private set; }

    private const string TARGET_TAG = "Player";
    public static readonly int speedHash = Animator.StringToHash("Speed");
    public static readonly int attackHash = Animator.StringToHash("Attack");


    private void Awake()
    {
        attackRangeSqr = detectRange * detectRange;

        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        // 체력바 초기화
        healthBar.value = Health / MaxHealth;

        // 플레이어 (타겟) 캐싱
        GameObject targetObj = GameObject.FindGameObjectWithTag(TARGET_TAG);

        if (targetObj == null)
        {
            Debug.Log($"공격 타겟: 캐싱 실패");
            return;
        }

        Debug.Log($"공격 타겟: {Target} 캐싱");
        
        Target = targetObj.GetComponent<IDamagable>();
        TargetTrans = targetObj.transform;
        targetObj.GetComponent<LivingEntity>().OnDeath += HandleTargetDeath;
    }

    private void Update()
    {
        if (Target == null)
            return;

        if (IsTargetInAttackRange())
        {
            agent.isStopped = false;
            isAttacking = false;
            Move();
        }
        else
        {
            agent.isStopped = true;
        }

        if (IsTargetInAttackRange() && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            isAttacking = true;

            // 일정 간격 공격
            if (lastAttackTime + attackInterval <= Time.time)
            {
                lastAttackTime = Time.time;
                animator.SetTrigger(attackHash);
            }
        }

        if (isAttacking)
        {
            RotateToTarget();
        }

        animator.SetFloat(speedHash, agent.velocity.magnitude <= 0.5f ? 0 : agent.velocity.magnitude * 1.1f);
    }

    // 타겟 범위 내에 들어왔는지 여부 반환
    private bool IsTargetInAttackRange()
    {
        float sqrDisToTarget = (TargetTrans.position - transform.position).sqrMagnitude;

        return sqrDisToTarget <= attackRangeSqr;
    }

    // 이동
    private void Move()
    {
        if (Target == null)
            return;

        agent.SetDestination(TargetTrans.position);
    }

    private void RotateToTarget()
    {
        transform.LookAt(TargetTrans);
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
        healthBar.value = Health / MaxHealth;
        Debug.Log($"{gameObject.name} HP: {Health}");
    }

    // 죽음
    protected override void Die()
    {
        base.Die();
        gameObject.SetActive(false);
    }

    // 공격 사거리 표시
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }

    // 타겟 사망시
    private void HandleTargetDeath()
    {
        Target = null;
        TargetTrans = null;
    }
}
