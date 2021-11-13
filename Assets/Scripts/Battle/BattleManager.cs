using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UIElements;
using UnityEngine.Playables;
[RequireComponent(typeof(PlayableDirector))]
public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public List<GameObject> Players;
    public List<GameObject> Enemies;
    public bool isInBattle;

    public bool movingToPosition;

    public PlayableDirector director;

    public VisualTreeAsset enemySelectionUITemplate;

    public event EmptyEventHandler inBattlePositon;

    public event EmptyEventHandler inOverworldPosition;
    /// <summary>
    /// a list of all of the ui for selecting an enemy
    /// </summary>
    public List<EnemySelectorUI> enemySelectorUI = new List<EnemySelectorUI>();

    //public event EmptyEventHandler settingUpBattle;

    [HideInInspector]
    public AudioSource BattleMusic;
    
    public event BattleEndEventHandler OnBattleEnd;

    public BattleRewardData battleRewardData;
    [HideInInspector]
    public SpriteRenderer BattleBackground;

    public bool IsWaitingForSkill;

    public Transform userTransform;
    public Transform targetTransform;

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

        director = GetComponent<PlayableDirector>();
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
                // skip everything else
                return;

            }
            else { }
            bool isPlayerLoss = true;
            foreach (GameObject player in Players)
            {
                if (!player.GetComponent<Battler>().isDown)
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
    /// <summary>
    /// triggered when either all the players or enemies are down
    /// </summary>
    /// <param name="isPlayerVictor"></param>
    public void EndBattle(bool isPlayerVictor)
    {
        AudioManager.UnpauseCurrentSong();
        if(BattleMusic != null) BattleMusic.Stop();

        isInBattle = false;

        UIManager.instance.ResetFocus();


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
                Battler battledata = enemy.GetComponent<Battler>();
                battledata.isInBattle = false;
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
                Battler battledata = player.GetComponent<Battler>();
                battledata.isInBattle = false;

                LevelUpController levelUpController = player.GetComponent<LevelUpController>();
                levelUpController.currentEXP += battleRewardData.totalEXP;

                

                player.GetComponent<Animator>().SetTrigger("BattleEnd");
            }

            InkManager.instance.DisplayVictoryData();

            OnBattleEnd?.Invoke(new OnBattleEndEventArgs { isPlayerVictor = isPlayerVictor });
        }
        else
        {
            OnBattleEnd?.Invoke(new OnBattleEndEventArgs { isPlayerVictor = isPlayerVictor });

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
    }
    private void ResumeGameWorld()
    {
        InkManager.instance.ContinueStory();
    }
    public void PauseBattle(string attackName, string userName, string targetName)
    {
        Label battleText = UIManager.instance.root.Q<Label>("battle_text");
        battleText.visible = true;
        battleText.text = userName + " used " + attackName + " on " + targetName;

        IsWaitingForSkill = true;
        BattleMenuManager.instance.battleUI.SetEnabled(false);
        BattleMenuManager.instance.skillSelector.SetEnabled(false);
        BattleMenuManager.instance.enemySelector.SetEnabled(false);
        BattleMenuManager.instance.itemSelector.SetEnabled(false);
    }
    public void UnPauseBattle()
    {
        Label battleText = UIManager.instance.root.Q<Label>("battle_text");
        battleText.visible = false;


        IsWaitingForSkill = false;
        BattleMenuManager.instance.battleUI.SetEnabled(true);
        BattleMenuManager.instance.skillSelector.SetEnabled(true);
        BattleMenuManager.instance.enemySelector.SetEnabled(true);
        BattleMenuManager.instance.itemSelector.SetEnabled(true);
    }
    /// <summary>
    /// what happens when the ink story is done displaying the victory data
    /// </summary>
    private void FinishVictoryData_OnDisplayFinished(object sender, System.EventArgs e)
    {
        movingToPosition = true;
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
            SpriteRenderer sprite = player.GetComponent<SpriteRenderer>();
            sprite.sortingLayerName = "Characters";
            Battler battler = player.GetComponent<Battler>();
            player.GetComponent<Animator>().Play(battler.animationSaveData.name, 0, battler.animationSaveData.normilizedtime);
            StartCoroutine(TransitionToOriginalPositions(player, player.GetComponent<Battler>().oldPosition, i == 0, .5f));
            i++;
        }
        foreach (GameObject enemy in Enemies)
        {
            SpriteRenderer sprite = enemy.GetComponent<SpriteRenderer>();
            sprite.sortingLayerName = "Characters";
            Battler battler = enemy.GetComponent<Battler>();
            enemy.GetComponent<Animator>().Play(battler.animationSaveData.name, 0, battler.animationSaveData.normilizedtime);
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
        if(BattleMusic != null) BattleMusic.Play();

        isInBattle = true;
        

        Camera cam = FindObjectOfType<Camera>();
        float positionRatio = 1280.0f / cam.pixelWidth;
        
        foreach (GameObject battler in Players)
        {
            Animator animator = battler.GetComponent<Animator>();
            animator.SetTrigger("BattleStart");

            // set up heads up display
            Battler battledata = battler.GetComponent<Battler>();
            battledata.isInBattle = true;
            TemplateContainer headsupUI = battledata.headsUpUI.ui;        
        }
        foreach (GameObject battler in Enemies)
        {
            Battler battledata = battler.GetComponent<Battler>();
            battledata.isInBattle = true;

            Animator animator = battler.GetComponent<Animator>();
            animator.SetTrigger("BattleStart");
        }
    }
    public void SetupBattle(GameObject[] enemies, SpriteRenderer battleBackground, AudioSource battleMusic)
    {
        AudioManager.PauseCurrentSong();
        BattleBackground = battleBackground;

        BattleMusic = battleMusic;
        PauseManager.instance.Pause();

        StartCoroutine(TransitionBackgroundAlpha(0, 1, battleBackground, .5f));

        Camera cam = Camera.main;
        float positionRatio = 1280.0f / cam.pixelWidth;
        // transition all of the characters

        CameraController.instance.ToBattleCamera();

        Players.Clear();
        foreach (GameObject gameObject in PlayerPartyManager.instance.players) Players.Add(gameObject);
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

            Battler battler = player.GetComponent<Battler>();
            battler.oldPosition = player.transform.position;
            battler.animationSaveData = new AnimationSaveData
            {
                name = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name,
                normilizedtime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime
            };

            float spawnzone = cam.scaledPixelHeight * .7f;
            float startingIncrement = cam.scaledPixelHeight * .1f;
            Vector3 tempPos = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth * .15f, ((i + 1) * (spawnzone / (Players.Count + 1)) + startingIncrement), 0));
            tempPos.z = 0;

            battler.battlePosition = tempPos;


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
            battler.animationSaveData = new AnimationSaveData
            {
                name = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name,
                normilizedtime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime
            };

            float spawnzone = cam.scaledPixelHeight * .7f;
            float startingIncrement = cam.scaledPixelHeight * .1f;
            Vector3 tempPos = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth * .85f, ((i + 1) * (spawnzone / (enemies.Length + 1)) + startingIncrement), 0));
            
            StartCoroutine(TransitionToBattlePosition(enemy, tempPos, false, .5f));
            i++;
        }

        UIManager.instance.overworldOverlay.visible = false;
        UIManager.instance.ResetFocus();


    }
    IEnumerator TransitionToBattlePosition(GameObject transformy, Vector3 newPosition, bool triggerEvent, float duration)
    {
        movingToPosition = true;
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
        movingToPosition = false;
    }

    IEnumerator TransitionToOriginalPositions(GameObject transformy, Vector3 newPosition, bool triggerEvent, float duration)
    {
        movingToPosition = true;

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
        movingToPosition = false;
        if (triggerEvent)
        {
            inOverworldPosition?.Invoke();
        }
        
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

    public void InstantialBattlePrefab(GameObject prefab, Vector2 position)
    {
        Transform prefabTransform = Instantiate(prefab).transform;
        prefabTransform.position = position;
    }
    public void SetUserTargetTransforms(Transform target, Transform user)
    {
        targetTransform.position = target.position;
        userTransform.position = user.position;
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