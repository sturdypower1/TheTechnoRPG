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

    public event EventHandler inBattlePositon;
    /// <summary>
    /// a list of all of the ui for selecting an enemy
    /// </summary>
    public List<EnemySelectorUI> enemySelectorUI;

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

    public void UnpauseBattler(GameObject battler)
    {
        Animator animator = battler.GetComponent<Animator>();
        // time scaled time will be set to 0, so to play animations it needs to be in unscaled time
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }
    public void StartBattle()
    {
        foreach(GameObject battler in Players)
        {
            Animator animator = battler.GetComponent<Animator>();
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            animator.SetTrigger("BattleStart");
        }
        foreach (GameObject battler in Enemies)
        {
            Animator animator = battler.GetComponent<Animator>();
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            animator.SetTrigger("BattleStart");
        }
    }
    public void SetupBattle(GameObject[] enemies)
    {
        // pause the game
        Time.timeScale = 0;

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
            player.layer = 3;

            bool triggerEvent = false;
            if(i == 0)
            {
                triggerEvent = true;
            }
            // sets its layer to battler
            player.layer = 3;

            Vector3 tempPos = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth * .1f, ((i + 1) * (cam.pixelHeight / enemies.Length)) - cam.pixelHeight / (enemies.Length * 2), 0));
            StartCoroutine(TransitionToBattlePosition(player, tempPos, triggerEvent));
            i++;
        }

        i = 0;
        foreach (GameObject enemy in enemies)
        {
            // sets its layer to battler
            enemy.layer = 3;

            Vector3 tempPos = cam.ScreenToWorldPoint(new Vector3(cam.pixelWidth * .9f, ((i + 1) * (cam.pixelHeight / enemies.Length)) - cam.pixelHeight / (enemies.Length * 2), 0));
            
            StartCoroutine(TransitionToBattlePosition(enemy, tempPos, false));
            i++;
        }

        UIManager.instance.overworldOverlay.visible = false;
        UIManager.instance.ResetFocus();


    }
    IEnumerator TransitionToBattlePosition(GameObject transformy, Vector3 newPosition, bool triggerEvent)
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
            transform.position = Vector3.Lerp(oldPosition, newPosition, timePassed);
            yield return null;
        }
        if (triggerEvent)
        {
            inBattlePositon?.Invoke(this, EventArgs.Empty);
        }
    }

    IEnumerator TransitionToOriginalPositions(GameObject transformy, Vector3 newPosition, bool triggerEvent)
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
            transform.position = Vector3.Lerp(oldPosition, newPosition, Time.deltaTime);
            yield return null;
        }

    }
}
