using UnityEngine;

public enum SkillHitboxType
{
    None,
    Capsule,    
    Sphere,      
    Cone,        
    Projectile   
}
[CreateAssetMenu(fileName = "SkillData", menuName = "Scriptable Objects/SkillData")]
public class SkillData : ScriptableObject
{
    
    public int skillDamage;
    public int cost;
    public float cooldown;
    public GameObject prefab;
    public SkillHitboxType hitboxType;
    public LayerMask enemyLayer;
    public float range = 2.2f, radius = 0.25f, angle = 70f;
}

