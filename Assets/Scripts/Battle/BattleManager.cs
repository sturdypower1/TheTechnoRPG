using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public List<GameObject> Players;
    public List<GameObject> Enemies;
    public bool isInBattle;

    public VisualTreeAsset enemySelectionUITemplate;

    public event EmptyEventHandler inBattlePositon;

    public event EmptyEventHandler inOverworldPosition;
    /// <summary>
    /// a list of all of the ui for selecting an enemy
    /// </summary>
    public List<EnemySelectorUI> enemySelectorUI = new List<EnemySelectorUI>();

    public event EmptyEventHandler settingUpBattle;

    public AudioSource BattleMusic;

    public event BattleEndEventHandler OnBattleEnd;

    public BattleRewardData battleRewardData;

    public SpriteRenderer BattleBackground;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            inBattlePositon += StartBattle;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        InkManager.instance.OnVictoryDisplayFinish += FinishVictoryData_OnDisplayFinished;
        inOverworldPosition += ResumeGameWorld;
    }

    private void Update()
    {
        if (isInBattle)
        {
            foreach(EnemySelectorUI enemySelector in enemySelectorUI)
            {
                enemySelector.Update();
            }

            bool isPlayerVictory = true;
            foreach(GameObject enemy in Enemies)
            {
                if (!enemy.GetComponent<Battler>().isDown)
                {
                    isPlayerVictory = false;
                }
            }
            if (isPlayerVictory)
            {
                // end battle as player victory
                EndBattle(true);
            }

        }
    }
    /// <summary>
    /// triggered when either all the players or enemies are down
    /// </summary>
    /// <param name="isPlayerVictor"></param>
    public void EndBattle(bool isPlayerVictor)
    {
        BattleMusic.Stop();
        isInBattle = false;

        


        foreach (GameObject player in Players)
        {
            CharacterStats stats = player.GetComponent<CharacterStats>();
            Battler battler = player.GetComponent<Battler>();
            battler.isDown = false;
            if (stats.stats.health <= 0)
            {
                stats.stats.health = isPlayerVictor ? 1 : stats.stats.maxHealth;
            }
        }
        foreach (GameObject enemy in Players)
        {
            Battler battler = enemy.GetComponent<Battler>();
            battler.isDown = false;
        }

        if (isPlayerVictor)
        {
            // play old song
            battleRewardData = new BattleRewardData();
            battleRewardData.items = new List<Item>();
            //add total exp, total gold and items
            foreach (GameObject enemy in Enemies)
            {
                if (enemy.GetComponent<EnemyRewardData>() != null)
                {
                    EnemyRewardData enemyRewardData = enemy.GetComponent<EnemyRewardData>();
                    float randomValue = UnityEngine.Random.Range(0, 1);
                    battleRewardData.totalEXP += enemyRewardData.EXP;
                    battleRewardData.totalGold += enemyRewardData.gold;
                    // seeing if the player gets the item
                    if (enemyRewardData.itemData.item != null && randomValue < enemyRewardData.itemData.chance) battleRewardData.items.Add(enemyRewardData.itemData.item);
                }
                enemy.GetComponent<Animator>().SetTrigger("BattleEnd");
            }
            foreach (GameObject player in Players)
            {
                LevelUpController levelUpController = player.GetComponent<LevelUpController>();
                levelUpController.currentEXP += battleRewardData.totalEXP;

                player.GetComponent<Animator>().SetTrigger("BattleEnd");
            }

            InkManager.instance.DisplayVictoryData();

            OnBattleEnd?.Invoke(new OnBattleEndEventArgs { isPlayerVictor = isPlayerVictor });
        }
    }
    private void ResumeGameWorld()
    {
        InkManager.instance.ContinueStory();
    }
    /// <summary>
    /// what happens when the ink story is done displaying the victory data
    /// </summary>
    private void FinishVictoryData_OnDisplayFinished(object sender, System.EventArgs e)
    {
        InkManager.instance.DisableTextboxUI();
        // start transitioning the background
        int i = 0;
        while(i < Enemies.Count)
        {
            GameObject enemy = Enemies[i];
            EnemyRewardData enemyRewardData = enemy.GetComponent<EnemyRewardData>();
            if (enemyRewardData.destroyOnDefeat)
            {
                Destroy(enemy);
                Enemies.Remove(enemy);
            }
            else
            {
                i++;
            }
        }
        i = 0;
        // transition back once the writer is done
        foreach (GameObject player in Players)
        {
            StartCoroutine(TransitionToOriginalPositions(player, player.GetComponent<Battler>().oldPosition, i == 0, .5f));
            i++;
        }
        foreach (GameObject enemy in Enemies)
        {
            StartCoroutine(TransitionToOriginalPositions(enemy, enemy.GetComponent<Battler>().oldPosition, false, .5f));
        }
        StartCoroutine(TransitionBackgroundAlpha(1, 0, BattleBackground, .5f));

        
    }
    public void UnpauseBattler(GameObject battler)
    {
        Animator animator = battler.GetComponent<Animator>();
        // time scaled time will be set to 0, so to play animations it needs to be in unscaled time
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }
    public void StartBattle()
    {

        BattleMusic.Play();
        isInBattle = true;

        Camera cam = FindObjectOfType<Camera>();
        float positionRatio = 1280.0f / cam.pixelWidth;
        
        foreach (GameObject battler in Players)
        {
            Animator animator = battler.GetComponent<Animator>();
            animator.SetTrigger("BattleStart");

            // set up heads up display
            Battler battledata = battler.GetComponent<Battler>();
            TemplateContainer headsupUI = battledata.headsUpUI.ui;        
        }
        foreach (GameObject battler in Enemies)
        {
            Animator animator = battler.GetComponent<Animator>();
            animator.SetTrigger("BattleStart");
        }
    }
    public void SetupBattle(GameObject[] enemies, SpriteRenderer battleBackground, AudioSource battleMusic)
    {
        BattleBackground = battleBackground;

        BattleMusic = battleMusic;
        PauseManager.instance.Pause();
        // pause the game
        Time.timeScale = 0;

        StartCoroutine(TransitionBackgroundAlpha(0, 1, battleBackground, .5f));

        Camera cam = FindObjectOfType<Camera>();
        float positionRatio = 1280.0f / cam.pixelWidth;
        // transition all of the characters

        CameraController.instance.ToBattleCamera();

        Players.Clear();
        Players.Add(Technoblade.instance.gameObject);
        int i = 0;

        // need to start trasition of the new camera

        foreach (GameObject player in Players)
        {
            Animator animator = player.GetComponent<Animator>();
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            animator.SetTrigger("BattleSetup");

            // makes it so the sprite is above the battle background
            SpriteRenderer sprite = player.GetComponent<SpriteRenderer>();
            sprite.sortingLayerName = "Battlers";

            bool triggerEvent = false;
            if(i == 0)
            {
                triggerEvent = true;
            }
            // sets its layer to battler
            player.layer = 3;

            Battler battler = player.GetComponent<Battler>();
            battler.oldPosition = player.transform.position;

            Vector3 tempPos = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth * .15f, ((i + 1) * (cam.pixelHeight / enemies.Length)) - cam.pixelHeight / (enemies.Length * 2), 0));
            StartCoroutine(TransitionToBattlePosition(player, tempPos, triggerEvent, .5f));
            i++;
        }

        Enemies.Clear();
        enemySelectorUI.Clear();
        i = 0;

        VisualElement enemySelectorGroup = UIManager.instance.root.Q<VisualElement>("EnemySelector");
        foreach (GameObject enemy in enemies)
        {
            // makes it so the sprite is above the battle background
            SpriteRenderer sprite = enemy.GetComponent<SpriteRenderer>();
            sprite.sortingLayerName = "Battlers";

            Enemies.Add(enemy);
            EnemySelectorUI enemySelector = new EnemySelectorUI { ui = enemySelectionUITemplate.CloneTree() , sprite = enemy.GetComponent<SpriteRenderer>(), enemy = enemy.GetComponent<Battler>()};
            enemySelectorUI.Add(enemySelector);
            enemySelectorGroup.Add(enemySelector.ui);

            //making it so when it's focused, the enemy glows
            enemySelector.ui.Q<Button>("Base").RegisterCallback<FocusEvent>(ev => enemySelector.SelectUI());
            // making it so when it's un focused, it no longer glows
            enemySelector.ui.Q<Button>("Base").RegisterCallback<FocusOutEvent>(ev => enemySelector.UnSelectUI());

            Animator animator = enemy.GetComponent<Animator>();
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            animator.SetTrigger("BattleSetup");

            Battler battler = enemy.GetComponent<Battler>();
            battler.oldPosition = enemy.transform.position;

            Vector3 tempPos = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth * .85f, ((i + 1) * (cam.pixelHeight / enemies.Length)) - cam.pixelHeight / (enemies.Length * 2), 0));
            
            StartCoroutine(TransitionToBattlePosition(enemy, tempPos, false, .5f));
            i++;
        }

        UIManager.instance.overworldOverlay.visible = false;
        UIManager.instance.ResetFocus();


    }
    IEnumerator TransitionToBattlePosition(GameObject transformy, Vector3 newPosition, bool triggerEvent, float duration)
    {
        // disable collision
        if(transformy.GetComponent<Collider2D>() != null)
        {
            Collider2D collider = transformy.GetComponent<Collider2D>();
            collider.enabled = false;
        }

        Transform transform = transformy.transform;
        Vector3 oldPosition = transform.position;
        newPosition.z = 0;
        float timePassed = 0;
        while(transform.position != newPosition)
        {
            timePassed += Time.unscaledDeltaTime;
            transform.position = Vector3.Lerp(oldPosition, newPosition, timePassed/ duration);
            yield return null;
        }
        if (triggerEvent)
        {
            inBattlePositon?.Invoke();
        }
    }

    IEnumerator TransitionToOriginalPositions(GameObject transformy, Vector3 newPosition, bool triggerEvent, float duration)
    {
        

        Transform transform = transformy.transform;
        Vector3 oldPosition = transform.position;

        float totalTime = 0;
        
        while (transform.position != newPosition)
        {
            totalTime += Time.unscaledDeltaTime;
            transform.position = Vector3.Lerp(oldPosition, newPosition, totalTime/ duration);
            yield return null;
        }
        // enable collision
        if (transformy.GetComponent<Collider2D>() != null)
        {
            Collider2D collider = transformy.GetComponent<Collider2D>();
            collider.enabled = true;
        }

        inOverworldPosition?.Invoke();
    }

    IEnumerator TransitionBackgroundAlpha(float a, float b, SpriteRenderer background, float duration)
    {
        float timePassed = 0;
        while(timePassed < 1)
        {
            timePassed += Time.unscaledDeltaTime;
            MaterialPropertyBlock myMatBlock = new MaterialPropertyBlock();
            background.GetPropertyBlock(myMatBlock);
            myMatBlock.SetFloat("Alpha", Mathf.Lerp(a, b, timePassed/ duration));
            background.SetPropertyBlock(myMatBlock);
            yield return null;
        }
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