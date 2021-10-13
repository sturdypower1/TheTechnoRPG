using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Playables;
using UnityEngine.Timeline;
/// <summary>
/// component that'll be used for all the battlers in a battle
/// </summary>
public abstract class Battler : MonoBehaviour
{
    /// <summary>
    /// when true, disables all battle ai and other actions
    /// </summary>
    public bool isPaused;
    /// <summary>
    /// how much long it has left before it can do another attack
    /// </summary>
    public float useTime;

    public float maxUseTime;

    public HeadsUpUI headsUpUI;
    [HideInInspector]

    public Vector3 oldPosition;
    [HideInInspector]

    public bool isDown;

    public AudioSource damagedSound;

    public CharacterStats characterStats;
    public Animator animator;
    [HideInInspector]
    public GameObject target;
    /// <summary>
    /// the target position 
    /// </summary>
    public Transform BattleOffset;
    [HideInInspector]
    public bool isInBattle;
    private void Start()
    {
        characterStats = GetComponent<CharacterStats>();
        animator = GetComponent<Animator>();
        if(BattleOffset == null)
        {
            BattleOffset = this.transform;
        }
    }

    public abstract void TakeDamage(Damage damage);

    public abstract void DealDamage(Damage damage);

    public virtual void AddBleeding(int levelGain, int Limit)
    {
        if(GetComponent<Bleeding>() != null)
        {
            // add bleeding to the object, if it already has it increase its level
            Bleeding bleeding = GetComponent<Bleeding>();
            if(bleeding.level + levelGain >= Limit)
            {
                // if the current level of bleeding is greater 
                bleeding.level = bleeding.level > Limit ? bleeding.level : Limit;
            }
            else
            {
                bleeding.level += levelGain;
            }
            
        }
        else
        {
            this.gameObject.AddComponent<Bleeding>();
        }
    }
}
