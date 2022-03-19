using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager instance;

    public event EmptyEventHandler OnTitleReturn;

    [SerializeField] private OverworldUI overworldUI;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private SettingsUI settingsUI;
    [SerializeField] private FileSaveManager fileSaveManager;
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private InkManager storyManager;
    public void StartBattle(Battler[] enemies, SpriteRenderer battleBackground, AudioSource battleMusic)
    {
        overworldUI.DisableUI();
        battleManager.SetupBattle(enemies, battleBackground, battleMusic);
    }
    public void TryEnableInteractable()
    {
        overworldUI.ActivateInteractable();
    }
    public bool IsInteractButtonPressed()
    {
        return overworldUI.IsInteractButtonPressed();
    }
    public void ActivateSave(string savePointName)
    {
        fileSaveManager.Activate(savePointName);
    }
    public void DisableOverworldOverlay()
    {
        overworldUI.DisableUI();
    }
    private void OnPausePressed_OpenInventory()
    {
        inventoryUI.EnableUI();
    }
    private void OnSettingsPressed_OpenSettings()
    {
        settingsUI.Activate();
    }
    private void OnInventoryBackPressed_EnableOverowrldOverlay()
    {
        PauseManager.instance.UnPause();
        PlayerInputManager.instance.EnableInput();
        overworldUI.EnableUI();
    }
    private void OnSettingsBackPressed_ReopenInventory()
    {
        inventoryUI.EnableUI();
    }
    private void OnSaveBackPressed_EnableOveworldOverlay()
    {
        overworldUI.EnableUI();
    }
    private void ResumeGameWorld_OnFinishedDisplaying()
    {
        overworldUI.EnableUI();
        PauseManager.instance.UnPause();
        CameraController.instance.SwitchToFollowCamera();
        PlayerInputManager.instance.EnableInput();
    }
    private void PauseOveworld_OnCutsceneStart()
    {
        overworldUI.DisableUI();
        PlayerInputManager.instance.DisableInput();
    }
    private void ReturnToTitle_OnTitleButtonPressed()
    {
        OnTitleReturn?.Invoke();
        SceneManager.LoadScene("TitleScreen");
        Destroy(this.gameObject);
    }
    private void Start()
    {
        overworldUI.OnPauseButtonPressed += OnPausePressed_OpenInventory;
        inventoryUI.OnBackPressed += OnInventoryBackPressed_EnableOverowrldOverlay;
        inventoryUI.OnSettingsPressed += OnSettingsPressed_OpenSettings;
        settingsUI.OnBackPressed += OnSettingsBackPressed_ReopenInventory;
        settingsUI.OnTitleButtonPressed += ReturnToTitle_OnTitleButtonPressed;

        fileSaveManager.OnBackPressed += OnSaveBackPressed_EnableOveworldOverlay;

        storyManager.OnCutsceneStarting += PauseOveworld_OnCutsceneStart;
        storyManager.OnFinishedDisplaying += ResumeGameWorld_OnFinishedDisplaying;


    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

  
}
