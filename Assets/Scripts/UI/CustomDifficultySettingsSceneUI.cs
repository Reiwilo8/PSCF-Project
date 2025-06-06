using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CustomDifficultySettingsSceneUI : MonoBehaviour
{
    public Button backButton;
    public Button playButton;

    public Slider epsilonSlider;
    public Slider alphaSlider;
    public Slider gammaSlider;

    private void Start()
    {
        backButton.onClick.AddListener(OnBackClicked);
        playButton.onClick.AddListener(OnPlayClicked);
    }

    void OnBackClicked()
    {
        SceneManager.LoadScene("DifficultySelectScene");
    }

    void OnPlayClicked()
    {
        GameManager.Instance.CustomEpsilon = epsilonSlider.value;
        GameManager.Instance.CustomAlpha = alphaSlider.value;
        GameManager.Instance.CustomGamma = gammaSlider.value;

        SceneManager.LoadScene("GameScene");
    }
}
