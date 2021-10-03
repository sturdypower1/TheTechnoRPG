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
    /// <summary>
    /// a list of all of the ui for selecting an enemy
    /// </summary>
    public List<EnemySelectorUI> enemySelectorUI;

    public event EmptyEventHandler settingUpBattle;

    public AudioSource BattleMusic;

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
            EnemySelectorUI enemySelector = new EnemySelectorUI { ui = enemySelectionUITemplate.CloneTree() , sprite = enemy.GetComponent<SpriteRenderer>()};
            enemySelectorUI.Add(enemySelector);
            enemySelectorGroup.Add(enemySelector.ui);

            Animator animator = enemy.GetComponent<Animator>();
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            animator.SetTrigger("BattleSetup");

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
        // disable collision
        if (transformy.GetComponent<Collider2D>() != null)
        {
            Collider2D collider = transformy.GetComponent<Collider2D>();
            collider.enabled = true;
        }

        Transform transform = transformy.transform;
        Vector3 oldPosition = transform.position;
        while (transform.position != newPosition)
        {
            transform.position = Vector3.Lerp(oldPosition, newPosition, Time.deltaTime/ duration);
            yield return null;
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
}

public delegate void EmptyEventHandler();