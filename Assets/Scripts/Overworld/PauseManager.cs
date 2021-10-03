using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager instance;

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
        Time.timeScale = 0;
        PlayerInputManager.instance.DisableInput();
    }

    public void UnPause()
    {
        Time.timeScale = 1;
        PlayerInputManager.instance.EnableInput();
    }
}
