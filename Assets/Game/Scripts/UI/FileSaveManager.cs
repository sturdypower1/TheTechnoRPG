using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileSaveManager : MonoBehaviour
{
    public event EmptyEventHandler OnBackPressed;
    public SaveAndLoadUI saveAndLoadUI;
    public void Activate()
    {
        saveAndLoadUI.Enable();
    }
    public void Deactivate()
    {
        saveAndLoadUI.Disable();
    }
    private void BackPressed()
    {
        OnBackPressed?.Invoke();
    }
    private void Awake()
    {
        saveAndLoadUI.OnBackPressed += BackPressed;
    }
}
