using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CreditsUI : MonoBehaviour
{
    public event EmptyEventHandler OnBackPressed;

    private UIDocument UIDoc;
    private VisualElement creditsBackground;
    public void Enable()
    {
        creditsBackground.visible = true;
    }
    public void Disable()
    {
        creditsBackground.visible = false;
    }
    
    private void BackButton()
    {
        AudioManager.playSound("menuback");
        Disable();
        OnBackPressed?.Invoke();
    }
    private void Awake()
    {
        UIDoc = GetComponent<UIDocument>();
        var root = UIDoc.rootVisualElement;

        creditsBackground = root.Q<VisualElement>("credits_background");
        var backButton = creditsBackground.Q<Button>("credits_back_button");
        backButton.clicked += BackButton;

        Button technoYoutubeButton = root.Q<Button>("techno_youtube");
        Button technoTwitterButton = root.Q<Button>("techno_twitter");

        Button tommyYoutubeButton = root.Q<Button>("tommy_youtube");
        Button tommyTwitterButton = root.Q<Button>("tommy_twitter");
        Button tommyTwitchButton = root.Q<Button>("tommy_twitch");

        Button wilburYoutubeButton = root.Q<Button>("wilbur_youtube");
        Button wilburTwitterButton = root.Q<Button>("wilbur_twitter");
        Button wilburTwitchButton = root.Q<Button>("wilbur_twitch");
        technoYoutubeButton.RegisterCallback<ClickEvent>(ev => LinkSender.sendToTechno(websites.youtube));
        technoTwitterButton.RegisterCallback<ClickEvent>(ev => LinkSender.sendToTechno(websites.twitter));

        tommyYoutubeButton.RegisterCallback<ClickEvent>(ev => LinkSender.sendToTommy(websites.youtube));
        tommyTwitterButton.RegisterCallback<ClickEvent>(ev => LinkSender.sendToTommy(websites.twitter));
        tommyTwitchButton.RegisterCallback<ClickEvent>(ev => LinkSender.sendToTommy(websites.twitch));

        wilburYoutubeButton.RegisterCallback<ClickEvent>(ev => LinkSender.sendToWilbur(websites.youtube));
        wilburTwitterButton.RegisterCallback<ClickEvent>(ev => LinkSender.sendToWilbur(websites.twitter));
        wilburTwitchButton.RegisterCallback<ClickEvent>(ev => LinkSender.sendToWilbur(websites.twitch));
    }
}
