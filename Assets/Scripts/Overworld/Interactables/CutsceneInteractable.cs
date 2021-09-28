using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneInteractable : Interactable
{
    public CutsceneData cutsceneData;
    public override void Interact()
    {
        if (!InkManager.instance.isCurrentlyDisplaying)
        {
            
            UIManager.instance.ResetFocus();
            InkManager.instance.StartCutScene(cutsceneData);
        }
        
    }
}
