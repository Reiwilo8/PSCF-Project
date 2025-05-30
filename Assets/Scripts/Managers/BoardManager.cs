using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int rows = 5;
    public int cols = 5;

    private void Start()
    {
        GameManager.Instance.ResetTurnOrder();
        GenerateBoard();
    }

    void GenerateBoard()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                GameObject tileObj = Instantiate(tilePrefab, transform);
                Tile tile = tileObj.GetComponent<Tile>();
                tile.Init(x, y);
                GameManager.Instance.board[x, y] = tile;
            }
        }
    }
}
