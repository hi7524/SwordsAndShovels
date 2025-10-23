using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float force = 20f;
    public float lifeDuration = 5f;

    private int damage;
    private float fireTime;
    private Vector3 fireDir;
    private Transform target;
    private Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (fireTime + lifeDuration <= Time.time)
        {
            gameObject.SetActive(false);
        }
    }

    public void Init(int damage, Transform target)
    {
        this.damage = damage;
        this.target = target;
        fireDir = target.position - transform.position;

        Fire();
    }

    private void Fire()
    {
        fireTime = Time.time;
        fireDir.y += 2;
        rb.AddForce(fireDir * force, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamagable damagable = target.gameObject.GetComponent<IDamagable>();

        if (damagable != null)
        {
            damagable.OnDamage(damage);
            gameObject.SetActive(false);
        }
    }
}
