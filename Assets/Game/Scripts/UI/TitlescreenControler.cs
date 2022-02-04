using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TitlescreenControler : MonoBehaviour
{

    [SerializeField]
    private MainMenuUI _mainMenuUI;
    [SerializeField]
    private CreditsUI _creditsUI;
    [SerializeField]
    private SettingsUI _settingsUI;
    [SerializeField]
    private SaveAndLoadUI _saveAndLoadUI;
    private void Start()
    {
        _mainMenuUI.OnStartButtonPressed += StartGame_OnStartButtonPressed;
        _mainMenuUI.OnSettingsButtonPressed += OpenSettings_OnSettingsButtonPressed;
        _mainMenuUI.OnCreditsButtonPressed += OpenCredits_OnCreditsButtonPressed;
        _mainMenuUI.OnContinueButtonPressed += OpenLoadUI_OnContinuePressed;


    }
    
    private void StartGame_OnStartButtonPressed()
    {
        _mainMenuUI.Deactivate();
        // load in the game in a default state
    }
    private void OpenLoadUI_OnContinuePressed()
    {
        _mainMenuUI.Deactivate();
        _saveAndLoadUI.Activate();
    }
    private void OpenSettings_OnSettingsButtonPressed()
    {
        _mainMenuUI.Deactivate();
        _settingsUI.Activate();
    }
    private void OpenCredits_OnCreditsButtonPressed()
    {
        _mainMenuUI.Deactivate();
        _creditsUI.Activate();
    }
    
}
