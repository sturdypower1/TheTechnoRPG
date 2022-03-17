using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.UIElements;

public class TitlescreenControler : MonoBehaviour
{

    [SerializeField]
    private MainMenuUI mainMenuUI;
    [SerializeField]
    private CreditsUI creditsUI;
    [SerializeField]
    private SettingsUI settingsUI;
    [SerializeField]
    private FileLoadManager loadManager;
    private void Start()
    {
        mainMenuUI.OnStartButtonPressed += StartGame_OnStartButtonPressed;
        mainMenuUI.OnSettingsButtonPressed += OpenSettings_OnSettingsButtonPressed;
        mainMenuUI.OnCreditsButtonPressed += OpenCredits_OnCreditsButtonPressed;
        mainMenuUI.OnContinueButtonPressed += OpenLoadUI_OnContinuePressed;

        loadManager.OnBackPressed += OpenMainMenu_OnFileSelectBack;

        settingsUI.OnBackPressed += OpenMainMenu_OnCreditsBackPressed;

        creditsUI.OnBackPressed += OpenMainMenu_OnCreditsBackPressed;
    }
    private void OpenMainMenu_OnFileSelectBack()
    {
        mainMenuUI.Activate();
    }
    private void StartGame_OnStartButtonPressed()
    {
        mainMenuUI.Deactivate();
        // load in the game in a default state

        SceneManager.LoadSceneAsync("BeforeThePyramid");
        foreach (string file in Directory.GetFiles(Application.persistentDataPath + "/tempsave"))
        {
            File.Delete(file);
        }
    }
    private void OpenLoadUI_OnContinuePressed()
    {
        mainMenuUI.Deactivate();
        loadManager.Activate();
    }
    private void OpenSettings_OnSettingsButtonPressed()
    {
        mainMenuUI.Deactivate();
        settingsUI.Activate();
    }
    private void OpenCredits_OnCreditsButtonPressed()
    {
        mainMenuUI.Deactivate();
        creditsUI.Enable();
    }
    
    private void OpenMainMenu_OnCreditsBackPressed()
    {
        mainMenuUI.Activate();
    }
}
