using SmartGridsToolkit;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GridManager : MonoBehaviour
{
    public static GridManager instance;

    [Header("Grid Settings")]
    public float cellSize = 1f;
    [Space(10)]

    [Header("Wall Prefabs")]
    public GameObject blackWall, redWall, blueWall, greenWall, yellowWall, pinkWall;
    [Space(10)]

    [Header("Moveable Objects")]
    public GameObject redObj, blueObj, greenObj, yellowObj, pinkObj;
    [Space(10)]

    public Dictionary<string, GameObject> prefabMap = new Dictionary<string, GameObject>();
    public Dictionary<string, Transform> parentMap = new Dictionary<string, Transform>();
    public Dictionary<string, int> labelValues = new Dictionary<string, int>();

    public int wallValue;
    public string cellTypeOrg;

    [HideInInspector] public Grid2DString gridString;

    private HashSet<string> labeledWalls = new HashSet<string>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        var wallPrefabs = new (string key, GameObject prefab, string parentName)[]
        {
        ("blackwall", blackWall, "Walls"),
        ("redwall", redWall, "RedWalls"),
        ("bluewall", blueWall, "BlueWalls"),
        ("greenwall", greenWall, "GreenWalls"),
        ("yellowwall", yellowWall, "YellowWalls"),
        ("pinkwall", pinkWall, "PinkWalls")
        };

        var objColors = new (string baseKey, GameObject prefab, string parentName)[]
        {
        ("redobj", redObj, "RedObjects"),
        ("blueobj", blueObj, "BlueObjects"),
        ("greenobj", greenObj, "GreenObjects"),
        ("yellowobj", yellowObj, "YellowObjects"),
        ("pinkobj", pinkObj, "PinkObjects")
        };

        foreach (var wc in wallPrefabs)
        {
            prefabMap[wc.key] = wc.prefab;
            CreateTypeParent(wc.key, wc.parentName, true);
        }

        foreach (var oc in objColors)
        {
            for (int i = 1; i <= 3; i++)
            {
                string fullKey = oc.baseKey + i;
                prefabMap[fullKey] = oc.prefab;
                CreateTypeParent(fullKey, fullKey, false);
            }
        }
    }

    private void CreateTypeParent(string key, string parentName, bool textField)
    {
        GameObject go = new GameObject(parentName);
        go.transform.SetParent(this.transform);
        parentMap[key] = go.transform;
    }


    void Start()
    {
        gridString = new Grid2DString(GameManager.instance.CurrentLevelData.gridLayout);
        GenerateGrid();
        OriginalGrid();
    }
    public Dictionary<Vector2Int, GameObject> wallsByGrid = new Dictionary<Vector2Int, GameObject>();
    void GenerateGrid()
    {
        int w = gridString.WidthCount;
        int h = gridString.HeightCount;

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                string cellType = gridString.GetCellValue(i, j);
                string originalCellType = 
                cellTypeOrg = cellType;
                if (!prefabMap.TryGetValue(cellType, out GameObject prefab) || prefab == null)
                    continue;
                Transform parent = null;
                if (parentMap.ContainsKey(cellType))
                {
                    parent = parentMap[cellType];
                }
                else
                {
                    Debug.LogError("Parent not present");
                }

                GameObject go = Instantiate(prefab, new Vector3(i * cellSize, 0f, j * cellSize), Quaternion.identity, parent);
                if (cellType.EndsWith("wall") )
                {
                    Vector2Int pos = new Vector2Int(i, j);
                    GameObject wallGO = go.gameObject;
                    wallsByGrid[pos] = wallGO;
                    AddLabelObject(cellType, wallGO);
                }
            }
        }
    }

    public void AddLabelObject(string cellType, GameObject wallGO)
    {
        if (!cellType.StartsWith("black") && !labeledWalls.Contains(cellType))
        {
            labeledWalls.Add(cellType);
            GameObject textGO = new GameObject("WallLabel");
            textGO.transform.SetParent(wallGO.transform, false);

            textGO.transform.localPosition = new Vector3(0f, 1f, 0f);

            textGO.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

            TextMesh textMesh = textGO.AddComponent<TextMesh>();
            wallValue = Random.Range(-10, 30);
            textMesh.text = $"<b>{wallValue.ToString()}</b>";
            labelValues[cellType] = wallValue;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.alignment = TextAlignment.Center;
            textMesh.characterSize = 0.22f;
            textMesh.fontSize = 60;
            if(cellType.Contains("yellowwall") || cellType.Contains("greenwall"))
            {
                textMesh.color = Color.black;
            }
            else
            {
                textMesh.color = Color.white;
            }
            var renderer = textGO.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                renderer.receiveShadows = false;
            }
        }
    }

    public void OriginalGrid()
    {
        int w = gridString.WidthCount;
        int h = gridString.HeightCount;

        for (int i = 0; i < w; i++)
        {
            for (int j = 0; j < h; j++)
            {
                string cellType = gridString.GetCellValue(i, j);
                gridString.SetCellValue(i, j, cellType);
            }
        }
    }
}