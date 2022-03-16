using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OverworldUI : MonoBehaviour
{
    public event EmptyEventHandler OnInteractPressed;
    public event EmptyEventHandler OnPauseButtonPressed;

    private UIDocument UIDoc;
    private VisualElement overworldBaseUI;
    private Button interactButton;
    private Button pauseButton;
    private bool isInteractButtonInteractable;
    public void EnableUI()
    {
        overworldBaseUI.visible = true;
        overworldBaseUI.Focus();
    }

    public void DisableUI()
    {
        overworldBaseUI.visible = false;
    }
    public void ActivateInteractButton()
    {
        isInteractButtonInteractable = true;
        interactButton.SetEnabled(true);
    }
    public void DeactivateInteractButton()
    {
        isInteractButtonInteractable = false;
        interactButton.SetEnabled(false);
    }

    private void PauseButton()
    {
        AudioManager.playSound("menuchange");
        DisableUI();

        OnPauseButtonPressed?.Invoke();
    }

    private void InteractButton()
    {
        //ensures that this will only be used if the button is enabled(temp fixes ui bug)
        if (!isInteractButtonInteractable) return;

        OnInteractPressed?.Invoke();
    }
    private void Awake()
    {
        UIDoc = GetComponent<UIDocument>();
        var root = UIDoc.rootVisualElement;
        overworldBaseUI = root.Q<VisualElement>("overworld_overlay");
        interactButton = overworldBaseUI.Q<Button>("interactive_item_check");
        pauseButton = overworldBaseUI.Q<Button>("pause_button");

        interactButton.clicked += InteractButton;
        pauseButton.clicked += PauseButton;
    }

    
}
