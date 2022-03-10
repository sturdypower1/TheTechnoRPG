using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;

public class TextboxUI : MonoBehaviour
{
    public event EmptyEventHandler TextboxDoneDisplaying;
    public float textSpeed = .1f;
    private bool isDisplaying = false;
    private bool isScrollingText = false;

    private UIDocument UIDoc;
    private Button textboxBase;
    private Label textboxText;
    private VisualElement characterPortraitUI;
    private Coroutine textCouritine;
    private string textToDisplay;

    private AudioSource currentDialogueSound;
    
    public void EnableUI()
    {

        textboxBase.visible = true;
    }
    public void DisableUI()
    {
        UIManager.instance.ResetFocus();
        characterPortraitUI.style.backgroundImage = null;
        textboxBase.visible = false;
    }
    

    public void SetCharacterPortrait(CharacterPortraitData characterPortrait)
    {
        characterPortraitUI.style.backgroundImage = Background.FromSprite(characterPortrait.portraits[0]);
    }
    
    public void DisplayText(string text, bool isInstant, bool isUnskippable, bool isSlow)
    {
        isDisplaying = true;
        EnableUI();
        ResetTextbox();
        textToDisplay = text;
        if (isInstant)
        {
            InstantText(text);
        }
        else
        {
            textCouritine = StartCoroutine(TextCoroutine(text, isSlow));
        }
    }
    private void TextboxButton()
    {
        if (isScrollingText)
        {
            FinishScrollingText();
        }
        else if(isDisplaying)
        {
            FinishDisplayingText();
        }
    }
    private void FinishDisplayingText()
    {
        isDisplaying = false;
        ResetTextbox();
        TextboxDoneDisplaying?.Invoke();
    }
    private void FinishScrollingText()
    {
        StopCoroutine(textCouritine);
        textboxText.text = textToDisplay;
        isScrollingText = false;
    }
    private void AddTextToTextbox(string text)
    {

    }
    private void ResetTextbox()
    {
        textboxText.text = "";
    }
    IEnumerator TextCoroutine(string text, bool isSlow)
    {
        isScrollingText = true;
        bool waitingForSymbol = false;

        foreach (Char letter in text)
        {
            textboxText.text += letter;
            //will automatically display rich text
            if (letter == '<' || waitingForSymbol)
            {
                waitingForSymbol = true;
                if (letter == '>')
                {
                    waitingForSymbol = false;
                }
                continue;
            }
            // add stuff for character animations
            if (letter != ' ')
            {
                if (currentDialogueSound != null)
                {
                    currentDialogueSound.Play();
                }
            }
            yield return new WaitForSecondsRealtime(1 / textSpeed);
        }
        isScrollingText = false;
    }

    private void InstantText(string text)
    {
        AddTextToTextbox(text);
    } 
    private void Awake()
    {
        UIDoc = GetComponent<UIDocument>();
    }
    private void Start()
    {
        var root = UIDoc.rootVisualElement;
        textboxBase = root.Q<Button>("textbox");
        textboxBase.clicked += TextboxButton;
        textboxText = textboxBase.Q<Label>("textbox_text");
        characterPortraitUI = textboxBase.Q<VisualElement>("character_image");
    }
}
