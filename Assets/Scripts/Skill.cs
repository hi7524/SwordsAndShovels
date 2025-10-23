using UnityEngine;

public class Skill : MonoBehaviour
{

    public SkillData[] skillDatas;

    public Transform swordRoot, swordTip, spawnPoint;
    public GameObject EnemyEffect;
    private SkillData skillData;
    private float rightTimer = 2f;

    private void Update()
    {
        rightTimer += Time.deltaTime;
        if (Input.GetMouseButtonDown(1))
        {   
            if(rightTimer>2)
            {
                UseSkill(1);
                rightTimer = 0;
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UseSkill(0);
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

        Enemy enemy = collider.GetComponent<Enemy>();

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

}
