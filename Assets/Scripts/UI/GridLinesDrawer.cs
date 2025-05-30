using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GridLinesDrawer : MonoBehaviour
{
    public int rows = 5;
    public int cols = 5;
    public float thickness = 10f;

    public Color lineColor = Color.black;
    public RectTransform boardArea;
    public GameObject linePrefab;

    private void Start()
    {
        StartCoroutine(DrawLinesDelayed());
    }

    private IEnumerator DrawLinesDelayed()
    {
        yield return new WaitForEndOfFrame();

        var grid = boardArea.parent.GetComponentInChildren<GridLayoutGroup>();
        float cellW = grid.cellSize.x;
        float spaceW = grid.spacing.x;
        float cellH = grid.cellSize.y;
        float spaceH = grid.spacing.y;

        float totalW = cellW * cols + spaceW * (cols - 1);
        float totalH = cellH * rows + spaceH * (rows - 1);

        float startX = -totalW / 2f;
        float startY = -totalH / 2f;

        for (int i = 1; i < cols; i++)
        {
            float x = startX + i * (cellW + spaceW) - spaceW / 2f;

            var line = Instantiate(linePrefab, boardArea);
            var rt = line.GetComponent<RectTransform>();
            var img = line.GetComponent<Image>();
            img.color = lineColor;

            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(thickness, totalH);
            rt.anchoredPosition = new Vector2(x, 0f);
        }

        for (int j = 1; j < rows; j++)
        {
            float y = startY + j * (cellH + spaceH) - spaceH / 2f;

            var line = Instantiate(linePrefab, boardArea);
            var rt = line.GetComponent<RectTransform>();
            var img = line.GetComponent<Image>();
            img.color = lineColor;

            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(totalW, thickness);
            rt.anchoredPosition = new Vector2(0f, y);
        }
    }
}