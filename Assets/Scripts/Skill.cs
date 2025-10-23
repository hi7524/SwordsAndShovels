using UnityEngine;

public class Skill : MonoBehaviour
{

    public SkillData[] skillDatas;
   
    public Transform swordRoot, swordTip, spawnPoint;
    public GameObject EnemyEffect;
    public SkillData skillData;
   
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UseSkill(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseSkill(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UseSkill(2);
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

        
        fwd.y = 0f;
        fwd.Normalize();

        foreach (var c in buf)
        {
            
            Vector3 dir = (c.transform.position - transform.position);
            dir.y = 0f; // YÃà ¹«½Ã
            dir.Normalize();

            float angle = Vector3.Angle(fwd, dir);
            

            if (angle <= skillData.angle * 0.5f)
                TryDamage(c);
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
       
        if (enemy == null)
        {
            return;
        }
        enemy.OnDamage(skillData.skillDamage);
        
    }

    void SpawnVfx()
    {
        if (!skillData.prefab) return;

        Vector3 spawnPos = transform.position;
        Quaternion spawnRot = Quaternion.identity;

        
        switch (skillData.hitboxType)
        {
            case SkillHitboxType.Capsule:
                spawnPos = (swordRoot.position + swordTip.position) * 0.5f;
                spawnRot = transform.rotation;
                break;

            case SkillHitboxType.Sphere:
                spawnPos = transform.position;
                spawnRot = Quaternion.identity;
                break;

            case SkillHitboxType.Cone:
                spawnPos = transform.position;
                spawnRot = Quaternion.LookRotation(transform.forward);
                break;

            case SkillHitboxType.Projectile:
                spawnPos = spawnPoint.position;
                spawnRot = spawnPoint.rotation;
                break;
        }

        
        var go = Instantiate(skillData.prefab, spawnPos, spawnRot);
        
        Destroy(go, 1.0f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector3 fwd = transform.forward;
        fwd.y = 0;
        fwd.Normalize();

        Quaternion left = Quaternion.AngleAxis(-skillData.angle * 0.5f, Vector3.up);
        Quaternion right = Quaternion.AngleAxis(skillData.angle * 0.5f, Vector3.up);

        Vector3 leftDir = left * fwd;
        Vector3 rightDir = right * fwd;

        Gizmos.DrawRay(transform.position, leftDir * skillData.range);
        Gizmos.DrawRay(transform.position, rightDir * skillData.range);
        Gizmos.DrawWireSphere(transform.position, skillData.range);
    }
}
