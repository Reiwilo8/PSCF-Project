using UnityEngine;
using UnityEngine.UI;

public class SettingsProfileSceneUI : MonoBehaviour
{
    public Button resetButton;

    private void Start()
    {
        resetButton.onClick.AddListener(OnResetClicked);
    }

    private void OnResetClicked()
    {
        if (StatsManager.Instance != null)
        {
            StatsManager.Instance.ResetStats();
        }
        else
        {
        }
    }
}
