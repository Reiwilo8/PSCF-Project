using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Vector2Int coordinates;
    public Button button;
    public TextMeshProUGUI symbolText;

    private bool isOccupied = false;

    public void Init(int x, int y)
    {
        coordinates = new Vector2Int(x, y);
        isOccupied = false;
        symbolText.text = "";
        button.interactable = true;
    }

    public void OnClick()
    {
        if (button.interactable && !isOccupied)
            MakeMove();
    }

    public void MakeMove()
    {
        if (isOccupied) return;

        string symbol = GameManager.Instance.IsPlayerOneTurn ? "X" : "O";
        symbolText.text = symbol;

        isOccupied = true;
        button.interactable = false;

        GameManager.Instance.CheckForWin(coordinates.x, coordinates.y, symbol);
        GameManager.Instance.SwitchTurn();
    }
}
