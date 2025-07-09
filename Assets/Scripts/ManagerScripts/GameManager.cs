using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public List<LevelData> levelDataList = new List<LevelData>();
    public LevelData CurrentLevelData { get; private set; }

    [HideInInspector] public int enemyTowerHealth;
    [HideInInspector] public int currentLevel = 0;
    [HideInInspector] public float intialTimer;
    [HideInInspector] public float timer;

    public static GameManager instance;

    public GameObject enemyTowerParent;

    public GameObject playerParent;
    public int npcCount;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        levelDataList.AddRange(Resources.LoadAll<LevelData>("Levels"));
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 0);

        if (currentLevel >= levelDataList.Count)
        {
            currentLevel = 0;
        }

        Initialize(levelDataList[currentLevel]);
    }

    private void Initialize(LevelData levelData)
    {
        CurrentLevelData = levelData;
        Instantiate(CurrentLevelData.tower, enemyTowerParent.transform);
        enemyTowerHealth = CurrentLevelData.towerHealth;
        intialTimer = CurrentLevelData.timeLimit;
        Time.timeScale = 1;
        timer = intialTimer;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            GoingToGameOverScreen();
            Time.timeScale = 0;
            timer = intialTimer;
        }
    }

    public IEnumerator SpawnWithDelay(int wallValue, int count)
    {
        if (wallValue < 0)
        {
            yield break;
        }
        var val = wallValue * count;
        npcCount += val;
        UIManager.instance.npcCount.text = "Npc : " + npcCount.ToString(); 
        for (int i = 0; i < val; i++)
        {
            GameObject npc = Instantiate(CurrentLevelData.npc, playerParent.transform);
            npc.transform.localScale = Vector3.zero;
            npc.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack,overshoot : 2.5f);
            yield return new WaitForSeconds(0.15f);
        }
        
    }

    public void SpawnEnemies(int wallValue, int count)
    {
        StartCoroutine(SpawnWithDelay(wallValue, count));
    }

    public void GoingToGameOverScreen()
    {
        UIManager.instance.RestartGamePanel();
    }
    public void GoingToResultScreen()
    {
        if (currentLevel + 1 < levelDataList.Count)
        {
            PlayerPrefs.SetInt("CurrentLevel", currentLevel + 1);
        }
        else
        {
            PlayerPrefs.SetInt("CurrentLevel", 0);
        }

        PlayerPrefs.Save();
        UIManager.instance.ResultScreenPanel();
    }
}
