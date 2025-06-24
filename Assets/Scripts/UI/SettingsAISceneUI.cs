using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles AI settings interactions within the settings sub-scene.
/// </summary>
public class SettingsAISceneUI : MonoBehaviour
{
    // Reference to the Reset button (assigned via Inspector)
    public Button resetButton;

    /// <summary>
    /// Registers the reset button callback.
    /// </summary>
    private void Start()
    {
        if (resetButton != null)
            resetButton.onClick.AddListener(OnResetClicked);
    }

    /// <summary>
    /// Resets the AI Q-table to its default state.
    /// </summary>
    private void OnResetClicked()
    {
        QTableIO.Reset();
    }
}