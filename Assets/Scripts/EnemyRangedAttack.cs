using UnityEngine;

// KHI
// 원거리 공격
public class EnemyRangedAttack : Enemy
{
    [Space]
    public Bomb bombPrf;
    public Transform bombFireTrans;

    protected override void Hit()
    {
        if (Target != null)
        {
            GameObject obj = Instantiate(bombPrf.gameObject);
            obj.transform.position = bombFireTrans.position;
            obj.GetComponent<Bomb>().Init(damage, TargetTrans);
        }
    }
}