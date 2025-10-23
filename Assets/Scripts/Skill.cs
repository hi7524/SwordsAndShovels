using UnityEngine;

public class Skill : MonoBehaviour
{

    public SkillData[] skillDatas;
   
    public Transform swordRoot, swordTip, spawnPoint;
    public GameObject EnemyEffect;
    public SkillData skillData;
   
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            UseSkill(0);
        }
        if (Input.GetMouseButtonDown(1))
        {
            UseSkill(1);
        }


    }

    private void UseSkill(int idx)
    {
        skillData = skillDatas[idx];
        switch (skillData.hitboxType)
        {
            case SkillHitboxType.Capsule:
                DoCapsuleHit();
                break;
            case SkillHitboxType.Cone:
                DoConeHit();
                break;
            case SkillHitboxType.Sphere:
                DoSphereHit();
                break;
            case SkillHitboxType.Projectile:
                SpawnProjectile();
                break;
        }
        if (skillData.hitboxType != SkillHitboxType.Projectile)
        {
            SpawnVfx();
        }
        

    }
    void DoCapsuleHit()
    {
        var start = swordRoot.position;
        var end = swordTip.position;
        Collider[] buf = new Collider[16];
        int n = Physics.OverlapCapsuleNonAlloc(start, end, skillData.radius, buf, skillData.enemyLayer);
        for (int i = 0; i < n; i++) TryDamage(buf[i]);
    }
    void DoSphereHit()
    {
        Collider[] buf = Physics.OverlapSphere(transform.position, skillData.range, skillData.enemyLayer);
        
        foreach (var c in buf) TryDamage(c);
    }

    void DoConeHit()
    {
        
        Collider[] buf = Physics.OverlapSphere(transform.position, skillData.range, skillData.enemyLayer);
        Vector3 fwd = transform.forward;
        foreach (var c in buf)
        {
            Vector3 dir = (c.transform.position - transform.position).normalized;
            if (Vector3.Angle(fwd, dir) <= skillData.angle * 0.5f) TryDamage(c);
        }
    }

    void SpawnProjectile()
    {
        
        var proj = Instantiate(skillData.prefab, spawnPoint.position, spawnPoint.rotation);
        if (proj.TryGetComponent<Rigidbody>(out var rb))
            rb.linearVelocity = spawnPoint.forward * 10f;
        Destroy(proj, 1f); 
        
    }
    private void TryDamage(Collider collider)
    {
        
        Enemy enemy = collider.GetComponentInParent<Enemy>();
        Debug.Log(enemy);
        if (enemy == null)
        {
            return;
        }
        enemy.OnDamage(skillData.skillDamage);
        Debug.Log(123);
    }

    void SpawnVfx()
    {
        if (!skillData.prefab) return;
        var go = Instantiate(skillData.prefab, swordTip.position, swordTip.rotation);
        Destroy(go, 1.0f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, skillData.range);
    }
}
