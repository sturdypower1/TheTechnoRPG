using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TextboxUI : MonoBehaviour
{
    private UIDocument UIDoc;
    private Button textboxBase;
    private Label text; 
    
    public void EnableUI()
    {
        textboxBase.visible = true;
    }
    public void DisableUI()
    {
        UIManager.instance.ResetFocus();
        textboxBase.visible = false;
    }

    public void SetCharacterPortrait(CharacterPortraitData characterPortrait)
    {

    }
    public void DisplayText(string text, bool isInstant, bool isUnskippable, bool isSlow)
    {

    }
    private void Awake()
    {
        UIDoc = GetComponent<UIDocument>();
    }
    private void Start()
    {
        var root = UIDoc.rootVisualElement;
        textboxBase = root.Q<Button>("textbox");
        text = textboxBase.Q<Label>("textbox_text");
    }
}
