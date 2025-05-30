using UnityEngine;
using UnityEngine.UI;

public class WinLineDrawer : MonoBehaviour
{
    public RectTransform boardArea;
    public GameObject linePrefab;
    public float thickness = 15f;
    public Color lineColor = Color.yellow;

    private GameObject currentLine;
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

    public void DrawLine(Vector2Int start, Vector2Int end)
    {
        if (currentLine != null)
            Destroy(currentLine);

        currentLine = Instantiate(linePrefab, boardArea);
        var rt = currentLine.GetComponent<RectTransform>();
        var img = currentLine.GetComponent<Image>();
        img.color = lineColor;

        var tileStart = GameManager.Instance.board[start.x, start.y];
        var tileEnd = GameManager.Instance.board[end.x, end.y];
        var rtStart = tileStart.GetComponent<RectTransform>();
        var rtEnd = tileEnd.GetComponent<RectTransform>();

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
