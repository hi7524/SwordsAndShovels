using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float force = 20f;

    private int damage;
    private Vector3 fireDir;
    private Transform target;
    private Rigidbody rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
        rb.AddForce(fireDir * force, ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamagable damagable = target.gameObject.GetComponent<IDamagable>();
        damagable?.OnDamage(damage);

        gameObject.SetActive(false);
    }
}
