using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    [SerializeField] private OverworldUI overworldUI;
    [SerializeField] private InventoryUI inventoryUI;

    private void OnPausePressed_OpenInventory()
    {
        inventoryUI.EnableUI();
    }
    
    private void OnInventoryBackPressed_EnableOverowrldOverlay()
    {
        PauseManager.instance.UnPause();
        PlayerInputManager.instance.EnableInput();
        overworldUI.EnableUI();
    }
    private void Start()
    {
        overworldUI.OnPauseButtonPressed += OnPausePressed_OpenInventory;
        inventoryUI.OnBackPressed += OnInventoryBackPressed_EnableOverowrldOverlay;

        overworldUI.EnableUI();
    }
}
