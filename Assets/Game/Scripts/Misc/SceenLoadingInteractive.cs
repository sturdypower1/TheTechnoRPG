using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceenLoadingInteractive : Interactable
{
    public SceneLoadingData sceneLoadingData;
    public override void Interact()
    {
        if (!InkManager.instance.isCurrentlyDisplaying && !PauseManager.isPaused)
        {
            SceneLoadingManager.instance.LoadScene(sceneLoadingData);
        }
    }
}

