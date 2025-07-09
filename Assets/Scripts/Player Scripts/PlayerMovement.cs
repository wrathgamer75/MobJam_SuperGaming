using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 2f;

    void Start()
    {
        StartCoroutine(ChaseEnemiesInSequence());
    }

    IEnumerator ChaseEnemiesInSequence()
    {
        Transform target = null; 
        while (true)
        {
            EnemySpawner.instance.spawnedEnemies.RemoveAll(e => e == null || !e.activeInHierarchy);
            int npcCount = GameManager.instance.npcCount;
            if (EnemySpawner.instance.spawnedEnemies.Count > 0f)
            {
                float closestDist = float.MaxValue;

                foreach (var enemyObj in EnemySpawner.instance.spawnedEnemies)
                {
                    float dist = Vector3.Distance(transform.position, enemyObj.transform.position);
                    if (dist < closestDist)
                    {
                        closestDist = dist;
                        target = enemyObj.transform;
                    }
                }

                if (target != null && Vector3.Distance(transform.position, target.position) > 1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                }
                else if (target != null)
                {
                    target.GetComponent<EnemyDeath>().ReceiveDestroy();
                    npcCount = npcCount - 1;
                    UIManager.instance.npcCount.text = "Npc : " + npcCount.ToString();
                    GameManager.instance.npcCount = npcCount;
                    Destroy(gameObject);
                    yield break;
                }
            }
            else
            {
                Transform tower = GameManager.instance.enemyTowerParent.transform;

                if (Vector3.Distance(transform.position, tower.position) > 3f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, tower.position, speed * Time.deltaTime);
                }
                else
                {
                    tower.GetChild(0).GetComponent<EnemyTower>().TakingDamage();
                    npcCount = npcCount - 1;
                    UIManager.instance.npcCount.text = "Npc : " + npcCount.ToString();
                    GameManager.instance.npcCount = npcCount;
                    Destroy(gameObject);
                    yield break;
                }
            }

            yield return null;
        }
    }
}

