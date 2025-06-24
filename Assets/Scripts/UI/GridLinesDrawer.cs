using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Draws grid lines over a UI grid layout based on the defined row and column count.
/// </summary>
public class GridLinesDrawer : MonoBehaviour
{
    // Grid dimensions
    [SerializeField] private int rows = 5;
    [SerializeField] private int cols = 5;

    // Line appearance
    [SerializeField] private float thickness = 10f;
    [SerializeField] private Color lineColor = Color.black;

    // Assigned references
    public RectTransform boardArea;
    public GameObject linePrefab;

    /// <summary>
    /// Delays drawing until layout is finalized.
    /// </summary>
    private void Start()
    {
        StartCoroutine(DrawLinesDelayed());
    }

    /// <summary>
    /// Draws vertical and horizontal lines based on cell size and spacing.
    /// </summary>
    private IEnumerator DrawLinesDelayed()
    {
        yield return new WaitForEndOfFrame();

        if (boardArea == null || linePrefab == null)
            yield break;

        var grid = boardArea.parent.GetComponentInChildren<GridLayoutGroup>();
        if (grid == null)
            yield break;

        float cellW = grid.cellSize.x;
        float spaceW = grid.spacing.x;
        float cellH = grid.cellSize.y;
        float spaceH = grid.spacing.y;

        float totalW = cellW * cols + spaceW * (cols - 1);
        float totalH = cellH * rows + spaceH * (rows - 1);

        float startX = -totalW / 2f;
        float startY = -totalH / 2f;

        // Draw vertical lines
        for (int i = 1; i < cols; i++)
        {
            float x = startX + i * (cellW + spaceW) - spaceW / 2f;
            CreateLine(new Vector2(x, 0f), new Vector2(thickness, totalH));
        }

        // Draw horizontal lines
        for (int j = 1; j < rows; j++)
        {
            float y = startY + j * (cellH + spaceH) - spaceH / 2f;
            CreateLine(new Vector2(0f, y), new Vector2(totalW, thickness));
        }
    }

    /// <summary>
    /// Instantiates and positions a single line on the board.
    /// </summary>
    private void CreateLine(Vector2 anchoredPosition, Vector2 size)
    {
        var line = Instantiate(linePrefab, boardArea);
        var rt = line.GetComponent<RectTransform>();
        var img = line.GetComponent<Image>();

        if (rt != null)
        {
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = size;
            rt.anchoredPosition = anchoredPosition;
        }

        if (img != null)
            img.color = lineColor;
    }
}