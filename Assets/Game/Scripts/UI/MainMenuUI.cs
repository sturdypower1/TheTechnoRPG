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
        _menuBackground.visible = true;
    }

    public void Deactivate()
    {
        _menuBackground.visible = false;
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
        OnContinueButtonPressed?.Invoke();
        // load ui using the save and load system
        //SaveAndLoadManager.instance.UpdateSaveFileUI(fileSelectBackground);

    }
    /// <summary>
    /// open up the settings menu
    /// </summary>
    private void OptionsButton()
    {
        AudioManager.playSound("menuselect");
        OnSettingsButtonPressed?.Invoke();
        
    }

    private void CreditsButton()
    {
        AudioManager.playSound("menuchange");
        Deactivate();
        OnCreditsButtonPressed?.Invoke();
    }
    private void ExitButton()
    {
        Application.Quit();
    }

    private void Awake()
    {
        _UIDoc = GetComponent<UIDocument>();
        var root = _UIDoc.rootVisualElement;

        _menuBackground = root.Q<VisualElement>("title_background");

        _menuBackground.Q<Button>("Start").clicked += StartButton;
        _menuBackground.Q<Button>("Continue").clicked += continueButton;
        _menuBackground.Q<Button>("Options").clicked += OptionsButton;
        _menuBackground.Q<Button>("Credits").clicked += CreditsButton;
        _menuBackground.Q<Button>("Exit").clicked += ExitButton;

        
    }
}
