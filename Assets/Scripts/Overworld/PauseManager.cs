using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;
    public static bool isPaused;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 1;
        PlayerInputManager.instance.DisableInput();
    }

    public void UnPause()
    {
        isPaused = false;
        //Time.timeScale = 1;
        PlayerInputManager.instance.EnableInput();
    }
}
