using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public void ReceiveDestroy()
    {
        int enemiesCount = EnemySpawner.instance.spawnedEnemies.Count;
        enemiesCount = enemiesCount - 1; 
        UIManager.instance.enemiesCount.text = "Mob : " + enemiesCount.ToString();
        Destroy(this.gameObject);
    }
}
