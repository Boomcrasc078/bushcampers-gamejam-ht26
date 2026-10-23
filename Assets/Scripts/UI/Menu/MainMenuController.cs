
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Scenes")]
    [SerializeField] private string gameplaySceneName;

    [Header("Main Menu")]
    [SerializeField] private GameObject bootLogPanel;
    [SerializeField] private GameObject menuPanel;

    [Header("Submenus")]
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject settingsPanel;

    private void Start()
    {
        Time.timeScale = 1f;
        ShowMainMenu();
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void OpenCredits()
    {
        HideAllPanels();

        if (creditsPanel != null)
            creditsPanel.SetActive(true);
    }

    public void OpenSettings()
    {
        HideAllPanels();

        if (settingsPanel != null)
            settingsPanel.SetActive(true);
        else
            ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        HideAllPanels();

        if (bootLogPanel != null)
            bootLogPanel.SetActive(true);

        if (menuPanel != null)
            menuPanel.SetActive(true);
    }

    private void HideAllPanels()
    {
        if (bootLogPanel != null)
            bootLogPanel.SetActive(false);

        if (menuPanel != null)
            menuPanel.SetActive(false);

        if (creditsPanel != null)
            creditsPanel.SetActive(false);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
