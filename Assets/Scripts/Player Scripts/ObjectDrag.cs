using System.Collections.Generic;
using UnityEngine;


public enum MovingDirections { None, Forward, Backward, Left, Right };

public class ObjectDrag : MonoBehaviour
{
    public float threshold = 40f;
    public LayerMask draggableLayer;

    private bool isHolding;
    private Vector2 originalMousePos;
    private Vector2 currentMousePos;
    private Camera cam;
    private Transform selected;
    private Transform selectedChild;
    private Vector2Int gridPos;

    public List<GameObject> draggableObjects = new List<GameObject>();
    public List<Vector2Int> occupied = new List<Vector2Int>(); 
    public static ObjectDrag instance;
    public int draggableObjectsCount;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        cam = Camera.main;
    }

    void OnMouseDown()
    {
        originalMousePos = currentMousePos = Input.mousePosition;

        Ray ray = cam.ScreenPointToRay(originalMousePos);
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, draggableLayer, QueryTriggerInteraction.Collide))
        {
            if (hit.collider.CompareTag("Block") && hit.collider.isTrigger)
            {
                selected = hit.collider.transform.parent;
                selectedChild = hit.collider.transform;
                isHolding = true;
            }
        }
    }

    void OnMouseDrag()
    {
        if (!isHolding || selected == null) return;

        currentMousePos = Input.mousePosition;
        if (Vector2.Distance(currentMousePos, originalMousePos) > threshold)
        {
            TryMoveOneCell();
            originalMousePos = currentMousePos;
        }
    }
    private int wallValue;
    void TryMoveOneCell()
    {
        draggableObjectsCount = draggableObjects.Count;
        Vector2 delta = currentMousePos - originalMousePos;
        Vector2Int dir = Mathf.Abs(delta.x) > Mathf.Abs(delta.y) ? (delta.x > 0 ? Vector2Int.right : Vector2Int.left) : (delta.y > 0 ? Vector2Int.up : Vector2Int.down);

        bool canMove = true;

        foreach (Transform r in selected)
        {
            float cellSize = GridManager.instance.cellSize;
            Vector3 world = r.position;
            gridPos = new Vector2Int(Mathf.RoundToInt(world.x / cellSize), Mathf.RoundToInt(world.z / cellSize));
            Vector2Int nextPos = gridPos + dir;
            string cellType = GridManager.instance.gridString.GetCellValue(nextPos.x, nextPos.y);

            if (string.IsNullOrEmpty(cellType) || selected.gameObject.name == cellType)
            {
                canMove = true;
            }
            else if (cellType.EndsWith("wall"))
            {
                GameObject wall;
                if (GridManager.instance.wallsByGrid.TryGetValue(nextPos, out wall))
                {
                    var selColor = selected.GetComponentInChildren<MeshRenderer>().material.color;
                    var wallColor = wall.GetComponent<MeshRenderer>().material.color;

                    if (selColor == wallColor)
                    {
                        canMove = true;
                        if(GridManager.instance.labelValues.ContainsKey(cellType))
                        {
                            GameManager.instance.SpawnEnemies(GridManager.instance.labelValues[cellType], selected.transform.childCount);
                        }
                        foreach (Transform child in selected)
                        {
                            Vector3 worldPos = child.position;
                            var orig = new Vector2Int(Mathf.RoundToInt(worldPos.x / cellSize), Mathf.RoundToInt(worldPos.z / cellSize));
                            GridManager.instance.gridString.SetCellValue(orig.x, orig.y, string.Empty);
                        }
                        Destroy(selected.gameObject);
                        RefreshWallLabels();
                        return;
                    }
                    else
                    {
                        canMove = false;
                        break;
                    }
                }
            }
            else
            {
                canMove = false;
                break;
            }

        }
        
        if(canMove)
        {
            MovingObjects(selected, dir);
        }

        Debug.Log("CanMove" + canMove);
        originalMousePos = currentMousePos;
    }

    public void RefreshWallLabels()
    {
        GridManager.instance.labelValues.Clear();
        foreach (var wall in GridManager.instance.wallsByGrid)
        {
            var wallType = GridManager.instance.gridString.GetCellValue(wall.Key.x, wall.Key.y);

            if (!string.IsNullOrEmpty(wallType) && !wallType.StartsWith("black"))
            {
                var wallGO = wall.Value;
                var label = wallGO.transform.Find("WallLabel")?.GetComponent<TextMesh>();
                if (label != null)
                {
                    wallValue = UnityEngine.Random.Range(-10, 30);
                    GridManager.instance.labelValues[wallType] = wallValue;
                    label.text = wallValue.ToString();
                }
            }
        }
    }

    private void MovingObjects(Transform parent, Vector2Int direction)
    {
        var cellSize = GridManager.instance.cellSize;
        var grid = GridManager.instance.gridString;

        List<Vector2Int> originalPositions = new List<Vector2Int>();
        List<Vector2Int> targetPositions = new List<Vector2Int>();

        foreach (Transform child in parent)
        {
            Vector3 world = child.position;
            var orig = new Vector2Int(Mathf.RoundToInt(world.x / cellSize), Mathf.RoundToInt(world.z / cellSize));
            originalPositions.Add(orig);
            targetPositions.Add(orig + direction);
        }

        foreach (var orig in originalPositions)
        {
            string cell = grid.GetCellValue(orig.x, orig.y);
            grid.SetCellValue(orig.x, orig.y, string.Empty);
        }

        foreach(var tar in targetPositions)
        {
            grid.SetCellValue(tar.x, tar.y, parent.name);
        }

        foreach (Transform child in parent)
        {
            child.position += new Vector3(direction.x  * cellSize, 0f, direction.y * cellSize);
        }
    }

    void OnMouseUp()
    {
        originalMousePos = currentMousePos;
        isHolding = false;
        selected = null;
    }
}
