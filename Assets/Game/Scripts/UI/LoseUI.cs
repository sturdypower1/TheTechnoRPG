using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;

public class LoseUI : MonoBehaviour
{
    public EmptyEventHandler OnReturnToTitleButtonPressed;

    private UIDocument UIDoc;
    private VisualElement loseBackground;
    public void EnableUI()
    {
        if (Directory.GetFiles(Application.persistentDataPath + "/tempsave").Length <= 0)
        {
            loseBackground.Q<Button>("continue").SetEnabled(false);
        }
        else
        {
            loseBackground.Q<Button>("continue").SetEnabled(true);
        }
        loseBackground.visible = true;
    }
    public void DisableUI()
    {

    }
    private void ReturnToTitleButton()
    {
        OnReturnToTitleButtonPressed?.Invoke();
    }
    private void Awake()
    {
        UIDoc = GetComponent<UIDocument>();
    }

    private void Start()
    {
        var root = UIDoc.rootVisualElement;
        loseBackground = root.Q<VisualElement>("losing_screen");

        var titleButton = loseBackground.Q<Button>("title");
        titleButton.clicked += ReturnToTitleButton;
    }
}
