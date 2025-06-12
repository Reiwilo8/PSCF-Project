using UnityEngine;
using UnityEngine.UI;

public class SettingsAIUI : MonoBehaviour
{
    public Button resetButton;

    private void Start()
    {
        resetButton.onClick.AddListener(OnResetClicked);
    }

    private void OnResetClicked()
    {
        QTableIO.Reset();
    }
}
