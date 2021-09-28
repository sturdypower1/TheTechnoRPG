using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;
    public List<GameObject> Players;
    public List<GameObject> Enemies;
    public bool isInBattle;

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
    public void SetupBattle(GameObject[] players)
    {
        
    }
}
