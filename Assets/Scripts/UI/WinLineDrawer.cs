using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Draws a line across winning tiles on the UI board.
/// </summary>
public class WinLineDrawer : MonoBehaviour
{
    // Assigned via Inspector
    public RectTransform boardArea;
    public GameObject linePrefab;

    // Line appearance
    [SerializeField] private float thickness = 15f;
    [SerializeField] private Color lineColor = Color.yellow;

    private GameObject currentLine;

    /// <summary>
    /// Converts world position to local UI space within the board area.
    /// </summary>
    private Vector2 WorldToLocal(Vector3 worldPosition)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            boardArea,
            RectTransformUtility.WorldToScreenPoint(null, worldPosition),
            null,
            out localPoint
        );
        return localPoint;
    }

    /// <summary>
    /// Draws a visual line between two tiles to indicate a win.
    /// </summary>
    public void DrawLine(Vector2Int start, Vector2Int end)
    {
        if (boardArea == null || linePrefab == null || GameManager.Instance?.board == null)
            return;

        if (currentLine != null)
            Destroy(currentLine);

        currentLine = Instantiate(linePrefab, boardArea);
        var rt = currentLine.GetComponent<RectTransform>();
        var img = currentLine.GetComponent<Image>();

        if (img != null)
            img.color = lineColor;

        var tileStart = GameManager.Instance.board[start.x, start.y];
        var tileEnd = GameManager.Instance.board[end.x, end.y];

        if (tileStart == null || tileEnd == null)
            return;

        var rtStart = tileStart.GetComponent<RectTransform>();
        var rtEnd = tileEnd.GetComponent<RectTransform>();

        if (rtStart == null || rtEnd == null || rt == null)
            return;

        Vector2 posStart = WorldToLocal(rtStart.position);
        Vector2 posEnd = WorldToLocal(rtEnd.position);
        Vector2 mid = (posStart + posEnd) * 0.5f;
        Vector2 dir = posEnd - posStart;
        float length = dir.magnitude;

        rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = mid;
        rt.sizeDelta = new Vector2(length, thickness);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rt.localRotation = Quaternion.Euler(0, 0, angle);
    }
}