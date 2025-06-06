using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Vector2Int coordinates;
    public Button button;
    public TextMeshProUGUI symbolText;
    private bool isOccupied = false;
    public bool IsOccupied => isOccupied;

    public void Init(int x, int y)
    {
        coordinates = new Vector2Int(x, y);
        isOccupied = false;
        symbolText.text = "";
        button.interactable = true;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (isOccupied) return;

        string symbol = GameManager.Instance.IsPlayerOneTurn ? "X" : "O";
        GameManager.Instance.RegisterMove(coordinates.x, coordinates.y, symbol);
    }

    public void SetSymbol(string symbol)
    {
        symbolText.text = symbol;
        isOccupied = true;
        button.interactable = false;
    }

    public string GetSymbol()
    {
        return symbolText.text;
    }

    public bool IsEmpty()
    {
        return !isOccupied;
    }
}
