using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles profile-related settings interactions in the settings sub-scene.
/// </summary>
public class SettingsProfileSceneUI : MonoBehaviour
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
    /// Resets player statistics via StatsManager.
    /// </summary>
    private void OnResetClicked()
    {
        if (StatsManager.Instance != null)
            StatsManager.Instance.ResetStats();
    }
}
