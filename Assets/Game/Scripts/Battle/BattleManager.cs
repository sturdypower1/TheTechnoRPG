using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;
using UnityEngine.Playables;
using DG.Tweening;
public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    public event BattleEndEventHandler OnBattleEnd;

    public float battleSetupSpeed = .5f;
    public float overworldReturnSpeed = .5f;

    public VisualTreeAsset enemySelectionUITemplate;

    [HideInInspector]public Transform userTransform;
    [HideInInspector]public Transform targetTransform;

    [HideInInspector]public bool isInBattle;

    [HideInInspector]public bool movingToPosition;

    [HideInInspector]public AudioSource BattleMusic;

    [HideInInspector]public SpriteRenderer BattleBackground;

    [HideInInspector]public List<Battler> Players;
    [HideInInspector]public List<Battler> Enemies;

    private BattleRewardData lastBattleReward;

    public void SetupBattle(Battler[] enemies, SpriteRenderer battleBackground, AudioSource battleMusic)
    {
        AudioManager.PauseCurrentSong();
        BattleBackground = battleBackground;

        BattleMusic = battleMusic;
        PauseManager.instance.Pause();

        var backgroundTween = DOVirtual.Float(0, 1, overworldReturnSpeed, v =>
        {
            SetBackgroundAlpha(v);
        });
        backgroundTween.onComplete += StartBattle;

        Camera cam = Camera.main;
        float positionRatio = 1280.0f / cam.pixelWidth;
        // transition all of the characters

        CameraController.instance.SwitchToStillCamera();

        Players.Clear();
        foreach (GameObject gameObject in PlayerPartyManager.instance.players) Players.Add(gameObject.GetComponent<Battler>());

        // need to start trasition of the new camera
        int i = 0;
        foreach (Battler player in Players)
        {
            float spawnzone = cam.scaledPixelHeight * .7f;
            float startingIncrement = cam.scaledPixelHeight * .1f;
            Vector3 tempPos = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth * .15f, ((i + 1) * (spawnzone / (Players.Count + 1)) + startingIncrement), 0));
            tempPos.z = 0;
            player.BattleSetup(tempPos);
            i++;
        }

        Enemies.Clear();

        VisualElement enemySelectorGroup = UIManager.instance.root.Q<VisualElement>("EnemySelector");
        i = 0;
        foreach (Battler enemy in enemies)
        {
            // makes it so the sprite is above the battle backgroun

            Enemies.Add(enemy);

            float spawnzone = cam.scaledPixelHeight * .7f;
            float startingIncrement = cam.scaledPixelHeight * .1f;
            Vector3 tempPos = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth * .85f, ((i + 1) * (spawnzone / (enemies.Length + 1)) + startingIncrement), 0));
            tempPos.z = 0;

            enemy.BattleSetup(tempPos);
            i++;
        }

        UIManager.instance.overworldOverlay.visible = false;
        UIManager.instance.ResetFocus();
    }

    public void InstantializeBattlePrefab(GameObject prefab, Vector2 position)
    {
        Debug.Log("transition this to battler class");
        Transform prefabTransform = Instantiate(prefab).transform;
        prefabTransform.position = position;
    }

    public BattleRewardData GetLastBattleReward()
    {
        return lastBattleReward;
    }
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
    private void Start()
    {
        InkManager.instance.OnVictoryDisplayFinish += FinishVictoryData_OnDisplayFinished;
    }

    private void Update()
    {
        if (isInBattle)
        {
            bool isPlayerVictory = true;
            foreach(Battler enemy in Enemies)
            {
                if (!enemy.isDown)
                {
                    isPlayerVictory = false;
                }
            }
            if (isPlayerVictory)
            {
                // end battle as player victory
                EndBattle(true);
                // skip everything else
                return;

            }
            else { }
            bool isPlayerLoss = true;
            foreach (Battler player in Players)
            {
                if (!player.isDown)
                {
                    isPlayerLoss = false;
                }
            }
            if (isPlayerLoss)
            {
                // end battle as player loss
                EndBattle(false);
            }

        }
    }
    private void EndBattle(bool isPlayerVictor)
    {
        AudioManager.UnpauseCurrentSong();
        if(BattleMusic != null) BattleMusic.Stop();
        isInBattle = false;

        UIManager.instance.ResetFocus();

        if (isPlayerVictor)
        {
            InitializeBattleRewards();
            foreach (Battler enemy in Enemies)
            {
                if (enemy.GetComponent<EnemyRewardData>() != null)
                {
                    AddRewards(enemy);
                }
                enemy.BattleEnd();
            }
            
            PlayerPartyManager.instance.BattleEnd(lastBattleReward.totalEXP);
            InventoryManager.instance.AddItems(lastBattleReward.items);

            InkManager.instance.DisplayVictoryData();
        }
        else
        {
            AudioManager.playSound("defeatsong");
            VisualElement losingBackground = UIManager.instance.root.Q<VisualElement>("losing_screen");
            if (Directory.GetFiles(Application.persistentDataPath + "/tempsave").Length <= 0)
            {
                losingBackground.Q<Button>("continue").SetEnabled(false);
            }
            else
            {
                losingBackground.Q<Button>("continue").SetEnabled(true);
            }

            losingBackground.visible = true;
        }
        OnBattleEnd?.Invoke(new OnBattleEndEventArgs { isPlayerVictor = isPlayerVictor });
    }
    private void AddRewards(Battler battler)
    {
        EnemyRewardData enemyRewardData = battler.GetComponent<EnemyRewardData>();
        float randomValue = UnityEngine.Random.Range(0, 1);

        lastBattleReward.totalEXP += enemyRewardData.EXP;
        lastBattleReward.totalGold += enemyRewardData.gold;
        // seeing if the player gets the item
        var obtainsItem = enemyRewardData.itemData.item != null && randomValue < enemyRewardData.itemData.chance;
        if (obtainsItem) lastBattleReward.items.Add(enemyRewardData.itemData.item);
    }
    private void InitializeBattleRewards()
    {
        lastBattleReward = new BattleRewardData();
        lastBattleReward.items = new List<Item>();
    }
    private void ResumeGameWorld()
    {
        // will reset inputs if there is no story
        InkManager.instance.ResumeStory_OnReturnToOverworld();
    }
    private void FinishVictoryData_OnDisplayFinished(object sender, System.EventArgs e)
    {
        movingToPosition = true;
        InkManager.instance.DisableTextboxUI();

        foreach (Battler player in Players)
        {
            player.ReturnToOverworld();
        }
        foreach (Battler enemy in Enemies)
        {
            enemy?.ReturnToOverworld();
        }

        var backgroundTween = DOVirtual.Float(1, 0, overworldReturnSpeed, v =>
        {
            SetBackgroundAlpha(v);
        });
        backgroundTween.onComplete += ResumeGameWorld;
    }

    private void StartBattle()
    {
        if (BattleMusic != null) BattleMusic.Play();

        isInBattle = true;

        Camera cam = FindObjectOfType<Camera>();
        float positionRatio = 1280.0f / cam.pixelWidth;

        foreach (Battler battler in Players)
        {
            battler.BattleStart();
        }
        foreach (Battler battler in Enemies)
        {
            battler.BattleStart();
        }
    }

    private void SetBackgroundAlpha(float newAlpha)
    {
        MaterialPropertyBlock myMatBlock = new MaterialPropertyBlock();
        BattleBackground.GetPropertyBlock(myMatBlock);
        myMatBlock.SetFloat("Alpha", newAlpha);
        BattleBackground.SetPropertyBlock(myMatBlock);
    }
}

public struct BattleRewardData
{
    public int totalEXP;
    public int totalGold;
    public List<Item> items;
}

public delegate void EmptyEventHandler();

public delegate void BattleEndEventHandler( OnBattleEndEventArgs e);

public class OnBattleEndEventArgs : EventArgs
{
    public bool isPlayerVictor { get; set; }
}