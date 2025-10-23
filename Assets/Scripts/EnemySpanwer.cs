using TMPro;
using UnityEngine;

public class EnemySpanwer : MonoBehaviour
{
    public TextMeshProUGUI killCountText;
    [Space]
    public Enemy meleeAttackerPrf;
    public Enemy rangedAttackerPrf;
    public Transform[] spawnPoints;
    public int spawnEnemyCount = 3;

    private int killedEnemyCount = 0;

    private void Start()
    {
        for (int i = 0; i < spawnEnemyCount; i++)
        {
            SpawnEnemy();
        }

        killCountText.text = $"Killed Enemy: {killedEnemyCount}";
    }

    private void SpawnEnemy()
    {
        Debug.Log("몬스터 스폰");
        var obj = Instantiate(ReturnRandomAttacker());
        obj.transform.position = ReturnRandomSpawnPos().position;
        obj.GetComponent<Enemy>().OnDeath += SpawnEnemy;
        obj.GetComponent<Enemy>().OnDeath += AddKillCount;
    }

    private Transform ReturnRandomSpawnPos()
    {
        int random = Random.Range(0, spawnPoints.Length);
        return spawnPoints[random];
    }

    private Enemy ReturnRandomAttacker()
    {
        bool random = Random.Range(0.0f, 1.0f) < 0.5f;

        if (random)
            return meleeAttackerPrf;
        else
            return rangedAttackerPrf;
    }

    private void AddKillCount()
    {
        killedEnemyCount++;
        killCountText.text = $"Killed Enemy: {killedEnemyCount}";
    }
}