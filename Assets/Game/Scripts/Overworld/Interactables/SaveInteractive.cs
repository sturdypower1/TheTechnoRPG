using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SaveInteractive : Interactable
{
    public string savePointName;
    [SerializeField]
    public CutsceneData ringBellCutscene;
    public override void Interact()
    {
        if (!InkManager.instance.isCurrentlyDisplaying)
        {
            //UIManager.instance.ResetFocus();

            // restore player hp
            UIManager.instance.overworldOverlay.visible = false;
            Button ringButton = new Button();
            ringButton.AddToClassList("player_choice");
            ringButton.text = "ring the bell";
            //ringButton.focusable = true;
            // when the ring button is pressed, display a cutscene to ring the bell
            ringButton.clicked += () => { InkManager.instance.StartCutScene(ringBellCutscene); };

            Button saveButton = new Button();
            saveButton.AddToClassList("player_choice");
            saveButton.text = "save progress";
            //saveButton.focusable = true;
            saveButton.clicked += () => SaveAndLoadManager.instance.LoadSaveUI(savePointName);

            InkManager.instance.DisplayChoices(new Button[]{ ringButton, saveButton});
        }
    }

    
}
