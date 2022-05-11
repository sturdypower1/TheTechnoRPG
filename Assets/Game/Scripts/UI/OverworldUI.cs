using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class OverworldUI : MonoBehaviour
{
    public event EmptyEventHandler OnPauseButtonPressed;

    private UIDocument UIDoc;
    private VisualElement overworldBaseUI;
    private Button interactButton;
    private Button pauseButton;
    private bool isInteractButtonInteractable;
    private bool wasInteractButtonInteractable;
    private bool isInteractButtonPressed;
    private bool wasInteractButtonPressed;
    public bool IsInteractButtonPressed()
    {
        return isInteractButtonPressed;
    }
    public void ActivateInteractable()
    {
        isInteractButtonInteractable = true;
        if (!wasInteractButtonInteractable)
        {
            AudioManager.playSound("menuavailable");
            interactButton.SetEnabled(true);
        }
    }
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
        if (!wasInteractButtonInteractable) return;

        isInteractButtonPressed = true;
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

    private void FixedUpdate()
    {

        if (isInteractButtonInteractable)
        {
            isInteractButtonInteractable = false;
            wasInteractButtonInteractable = true;
        }
        else
        {
            wasInteractButtonPressed = false;
            wasInteractButtonInteractable = false;
            interactButton.SetEnabled(false);
        }

        if (wasInteractButtonPressed)
        {
            wasInteractButtonPressed = false;
            isInteractButtonPressed = false;
        }
        if (isInteractButtonPressed)
        {
            wasInteractButtonPressed = true;
        }
       
    }
}
