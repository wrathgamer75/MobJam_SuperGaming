namespace SmartGridsToolkit
{
    using UnityEngine;
    using TMPro;

    public class LetterGridGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject tilePrefab;
        [SerializeField] private GameObject letterTilesParent;
        [SerializeField] private float tileSpacingX = 100f;
        [SerializeField] private float tileSpacingY = 100f;
        public Grid2DString lettersToSpawns; //Set the size of the grid and assign letters in the inspector

        private int gridWidth;
        private int gridHeight;

        private void Start()
        {
            GenerateGrid();
        }

        void GenerateGrid()
        {
            gridWidth = lettersToSpawns.WidthCount;
            gridHeight = lettersToSpawns.HeightCount;

            for(int i = 0; i < gridWidth; i++)
            {
                for (int j = 0; j < gridHeight; j++)
                {
                    GameObject tile = Instantiate(tilePrefab, letterTilesParent.transform);

                    RectTransform tileRectTransform = tile.GetComponent<RectTransform>();
                    tileRectTransform.anchoredPosition = new Vector2(i * tileSpacingX, j * tileSpacingY);

                    TMP_Text tileText = tile.GetComponentInChildren<TMP_Text>();
                    tileText.text = lettersToSpawns.GetCellValue(i, j);
                }
            }

            // We center the grid on the camera after the grid is generated
            CenterGridOnCamera();
        }

        void CenterGridOnCamera()
        {
            RectTransform parentRectTransform = letterTilesParent.GetComponent<RectTransform>();
            float totalGridWidth = ((gridWidth - 1) * tileSpacingX);
            float totalGridHeight = ((gridHeight - 1) * tileSpacingY);
            parentRectTransform.anchoredPosition = new Vector2(-totalGridWidth / 2, 0f);
        }
    }
}
