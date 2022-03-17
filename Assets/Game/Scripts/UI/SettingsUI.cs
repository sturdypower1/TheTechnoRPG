using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SettingsUI : MonoBehaviour
{
    public bool isReturnToMainMenuActive;

    public event EmptyEventHandler OnBackPressed;
    public event EmptyEventHandler OnTitleButtonPressed;

    private UIDocument UIDoc;
    private VisualElement settingsBackground;
    public void Activate()
    {
        settingsBackground.visible = true;
    }
    public void Deactivate()
    {
        DeActivateSettingsTabs();
        settingsBackground.visible = false;
    }
    private void BackPressed()
    {
        AudioManager.playSound("menuback");
        Deactivate();
        OnBackPressed?.Invoke();
    }
    private void Awake()
    {
        UIDoc = GetComponent<UIDocument>();
        var root = UIDoc.rootVisualElement;

        settingsBackground = root.Q<VisualElement>("settings_background");

        var settingsBackButton = settingsBackground.Q<Button>("settings_back_button");
        settingsBackButton.clicked += BackPressed;

        Button volumeButton = settingsBackground.Q<Button>("volume_button");
        volumeButton.RegisterCallback<FocusEvent>(ev => ActivateSettingsTab(settingsBackground.Q<VisualElement>("volume_controls")));
        Button bindingsButton = settingsBackground.Q<Button>("bindings_button");
        bindingsButton.SetEnabled(false);
        bindingsButton.RegisterCallback<FocusEvent>(ev => ActivateSettingsTab(settingsBackground.Q<VisualElement>("bindings_controls")));
        Button othersButton = settingsBackground.Q<Button>("others_button");
        othersButton.RegisterCallback<FocusEvent>(ev => ActivateSettingsTab(settingsBackground.Q<VisualElement>("other_controls")));

        

        settingsBackground.Q<Slider>("volume_slider").RegisterValueChangedCallback(ev => ChangeVolume(ev.newValue));
        settingsBackground.Q<Button>("title_return_button").clicked += SettingsReturnToTitleButton;
        settingsBackground.Q<Button>("title_return_button").SetEnabled(isReturnToMainMenuActive);
    }
    private void SettingsReturnToTitleButton()
    {
        AudioManager.playSound("menuback");
        DeActivateSettingsTabs();
        Deactivate();
        OnTitleButtonPressed?.Invoke();
        
    }
    private void ActivateSettingsTab(VisualElement tab)
    {
        AudioManager.playSound("menuchange");
        DeActivateSettingsTabs();
        tab.visible = true;
    }
    private void DeActivateSettingsTabs()
    {
        settingsBackground.Q<VisualElement>("volume_controls").visible = false;
        settingsBackground.Q<VisualElement>("bindings_controls").visible = false;
        settingsBackground.Q<VisualElement>("other_controls").visible = false;
    }
    private void ChangeVolume(float newVolume)
    {
        AudioListener.volume = newVolume;
    }
}
