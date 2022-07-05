using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager instance;

    public event EmptyEventHandler OnTitleReturn;

    [SerializeField] private OverworldUI overworldUI;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] private LoseUI loseUI;
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
    public void ResumeGameworld()
    {
        PauseManager.instance.UnPause();
        PlayerInputManager.instance.EnableInput();
        overworldUI.EnableUI();
    }
    public void StopGameworld()
    {
        DisableOverworldOverlay();
        PlayerInputManager.instance.DisableInput();
        PauseManager.instance.Pause();
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
        ResumeGameworld();
    }
    private void OnSettingsBackPressed_ReopenInventory()
    {
        inventoryUI.EnableUI();
    }
    private void OnSaveBackPressed_ResumeGameworld()
    {
        ResumeGameworld();
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
        ReturnToTitle();
    }
    private void SetupLoseUI_OnPlayerLoss(OnBattleEndEventArgs e)
    {
        if (!e.isPlayerVictor)
        {
            AudioManager.playSound("defeatsong");
            PlayerPartyManager.instance.BattleLose();
            loseUI.EnableUI();
            LoadTempWorld();
        }
    }
    private void LoadTempWorld()
    {
        SceneManager.LoadScene("Temp");
    }
    private void ReturnToTitle_OnReturnToTitleButtonPressed()
    {
        AudioManager.stopSound("defeatsong");
        ReturnToTitle();
    }
    private void LoadWorld(string sceneName)
    {
        SaveAndLoadManager.instance.
        SaveAndLoadManager.instance.
        SceneManager.LoadScene();
    }
    private void OnTechnoNeverDies_RevivePlayerAndReload(string )
    {
        PlayerPartyManager.instance.ReviveParty();
        LoadWorld
    }
    
    private void ReturnToTitle()
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

        fileSaveManager.OnBackPressed += OnSaveBackPressed_ResumeGameworld;

        storyManager.OnCutsceneStarting += PauseOveworld_OnCutsceneStart;
        storyManager.OnFinishedDisplaying += ResumeGameWorld_OnFinishedDisplaying;

        battleManager.OnBattleEnd += SetupLoseUI_OnPlayerLoss;

        loseUI.OnReturnToTitleButtonPressed += ReturnToTitle_OnReturnToTitleButtonPressed;

        PauseManager.instance.UnPause();
    }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

  
}
