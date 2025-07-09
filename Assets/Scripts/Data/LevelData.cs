using UnityEngine;
using SmartGridsToolkit;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    public string levelName;
    public GameObject tower;
    public GameObject npc;
    public Grid2DString gridLayout;
    public float timeLimit;
    public int towerHealth;
    public int enemyRows;
    public int enemyColumns;

    public int gridWidth => gridLayout.WidthCount;
    public int gridHeight => gridLayout.HeightCount;
}