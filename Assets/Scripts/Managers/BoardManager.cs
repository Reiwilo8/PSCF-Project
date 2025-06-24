using UnityEngine;

/// <summary>
/// Generates the game board using tile prefabs and assigns them to the GameManager.
/// </summary>
public class BoardManager : MonoBehaviour
{
    // Tile prefab reference (assigned via Inspector)
    public GameObject tilePrefab;

    // Board dimensions
    [SerializeField] private int rows = 5;
    [SerializeField] private int cols = 5;

    /// <summary>
    /// Generates the tile grid on scene start.
    /// </summary>
    private void Start()
    {
        GenerateBoard();
    }

    /// <summary>
    /// Instantiates tiles and assigns them to the game board.
    /// </summary>
    void GenerateBoard()
    {
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                GameObject tileObj = Instantiate(tilePrefab, transform);
                Tile tile = tileObj.GetComponent<Tile>();

                if (tile != null)
                {
                    tile.Init(x, y);

                    if (GameManager.Instance != null && GameManager.Instance.board != null)
                        GameManager.Instance.board[x, y] = tile;
                }
            }
        }
    }
}