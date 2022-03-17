using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileLoadManager : MonoBehaviour
{
    public event EmptyEventHandler OnBackPressed;

    [SerializeField] private SaveAndLoadUI saveAndLoadUI;
    public void Activate()
    {
        saveAndLoadUI.Enable();
    }
    public void Deactivate()
    {
        saveAndLoadUI.Disable();
    }

    public void ContinueGameButton(int saveFileNubmer)
    {
        AudioManager.playSound("menuselect");
        Deactivate();

        //SaveAndLoadManager.instance.LoadGame(saveFileNubmer);
        // use the save system to continue
    }

    private void Awake()
    {
        saveAndLoadUI.OnSaveFilePressed += ContinueGameButton;
        saveAndLoadUI.OnBackPressed += BackPressed;
    }

    private void BackPressed()
    {
        AudioManager.playSound("menuback");
        Deactivate();
        OnBackPressed?.Invoke();
    }
}
