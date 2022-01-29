using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuUI : MonoBehaviour
{
    public event EmptyEventHandler OnStartButtonPressed;
    public event EmptyEventHandler OnCreditsButtonPressed;
    public event EmptyEventHandler OnSettingsButtonPressed;
    public event EmptyEventHandler OnContinueButtonPressed;

    private UIDocument _UIDoc;
    private VisualElement _root;
    private VisualElement _menuBackground;
    public void Activate()
    {

    }

    public void Deactivate()
    {

    }


    public void StartButton()
    {
        AudioManager.playSound("menuselect");
        OnStartButtonPressed?.Invoke();
    }
    /// <summary>
    /// open up the save ui to load a game
    /// </summary>
    private void continueButton()
    {
        AudioManager.playSound("menuselect");
        OnCreditsButtonPressed?.Invoke();
        // load ui using the save and load system
        //SaveAndLoadManager.instance.UpdateSaveFileUI(fileSelectBackground);

    }
    /// <summary>
    /// open up the settings menu
    /// </summary>
    private void OptionsButton()
    {
        AudioManager.playSound("menuselect");

        
    }

    private void CreditsButton()
    {
        AudioManager.playSound("menuchange");
    }
    private void ExitButton()
    {
        Application.Quit();
    }

}
