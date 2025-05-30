using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Vector2Int gridPosition;

    public void Init(int x, int y)
    {
        gridPosition = new Vector2Int(x, y);
    }

    public void OnClick()
    {

    }
}
