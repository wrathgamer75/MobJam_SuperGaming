using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Grid Layout")]
    public GameObject enemyPrefab;
    public int rows = 5;
    public int columns = 5;
    public float space = 2f;
    [Space(10)]

    public Transform parentTransform;
    public List<GameObject> spawnedEnemies = new List<GameObject>();
    public static EnemySpawner instance;

    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        rows = GameManager.instance.CurrentLevelData.enemyRows;
        columns = GameManager.instance.CurrentLevelData.enemyColumns;
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        float rowOffset = (rows - 1) * space / 2;
        float columnOffset = (columns - 1) * space / 2;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector3 position = new Vector3(i * space - rowOffset, 0f, j * space - columnOffset);
                GameObject obj = Instantiate(enemyPrefab, transform.position + position, Quaternion.identity, parentTransform);
                spawnedEnemies.Add(obj);
                obj.name = $"Enemy_{i}_{j}";
            }
        }
        UIManager.instance.enemiesCount.text = "Mob : " + spawnedEnemies.Count.ToString();
    }
}
