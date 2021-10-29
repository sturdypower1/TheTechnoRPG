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

    public AnimationSaveData animationSaveData;
    [HideInInspector]

    public bool isDown;

    /// <summary>
    /// whether or not the character is defending
    /// </summary>
    public bool isDefending;

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
    bool isUsingSkill;

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

    public virtual void Heal(int recovery)
    {
        characterStats.stats.health += recovery;
        if(characterStats.stats.health > characterStats.stats.maxHealth)
        {
            characterStats.stats.health = characterStats.stats.maxHealth;
        }
        Label label = new Label();
        label.text = recovery.ToString();
        label.AddToClassList("message_green");
        headsUpUI.ui.Q<VisualElement>("messages").Add(label);

        float random = Random.value * 360;
        Vector2 messageDirection = new Vector2(Mathf.Cos(random), Mathf.Sin(random));

        Message newMessage = new Message { timePassed = 0, label = label, direction = messageDirection };
        headsUpUI.messages.Add(newMessage);
    }

    public virtual void Defend()
    {
        isDefending = true;
        animator.SetBool("Defending", true);

    }

    IEnumerator WaitCouroutine(float waitTime)
    {
        Debug.Log(waitTime);
        if (isUsingSkill)
        {
            yield break;
        }
        isUsingSkill = true;
        
        maxUseTime = waitTime;
        useTime = 0;
        while(useTime < maxUseTime && !isDown)
        {
            UpdateMenu();
            yield return null;
            // only move if your not in a skill
            if (!BattleManager.instance.IsWaitingForSkill)
            {
                useTime += Time.deltaTime;
            }
            
            
        }
        // just ensures that the sound only plays if the player is in battle
        if (BattleManager.instance.isInBattle)
        {
            ReEnableMenu();
        }
        isUsingSkill = false;
    }
    public void StartWaitCouroutine(float waitTime)
    {
        StartCoroutine(WaitCouroutine(waitTime));
    }
    public virtual void ReEnableMenu()
    {

    }
    public virtual void UpdateMenu()
    {

    }
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
