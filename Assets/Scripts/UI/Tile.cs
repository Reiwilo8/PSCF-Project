using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents a single tile on the game board, handling state and input.
/// </summary>
public class Tile : MonoBehaviour
{
    // Tile position in the board grid
    private Vector2Int coordinates;

    // Assigned via prefab
    public Button button;
    public TextMeshProUGUI symbolText;

    // Tile state
    private bool isOccupied = false;
    public bool IsOccupied => isOccupied;

    /// <summary>
    /// Initializes the tile at given coordinates and resets state.
    /// </summary>
    public void Init(int x, int y)
    {
        coordinates = new Vector2Int(x, y);
        isOccupied = false;

        if (symbolText != null)
            symbolText.text = "";

        if (button != null)
        {
            button.interactable = true;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClick);
        }
    }

    /// <summary>
    /// Handles click events and forwards the move to the game manager.
    /// </summary>
    private void OnClick()
    {
        if (isOccupied) return;

        if (GameManager.Instance.SelectedGameMode == GameMode.PvE &&
            !GameManager.Instance.IsPlayerOneTurn) return;

        string symbol = GameManager.Instance.IsPlayerOneTurn ? "X" : "O";
        GameManager.Instance.RegisterMove(coordinates.x, coordinates.y, symbol);
    }

    /// <summary>
    /// Updates the tile with a symbol and marks it as occupied.
    /// </summary>
    public void SetSymbol(string symbol)
    {
        if (symbolText != null)
            symbolText.text = symbol;

        isOccupied = true;

        if (button != null)
            button.interactable = false;
    }

    /// <summary>
    /// Returns the current symbol on the tile.
    /// </summary>
    public string GetSymbol()
    {
        return symbolText != null ? symbolText.text : "";
    }

    /// <summary>
    /// Returns true if the tile is not yet occupied.
    /// </summary>
    public bool IsEmpty()
    {
        return !isOccupied;
    }
}