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

    public Vector3 battlePosition;

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
    public Battler target;
    /// <summary>
    /// the target position 
    /// </summary>
    public Transform BattleOffset;
    [HideInInspector]
    public bool isInBattle;
    bool isUsingSkill;

    public virtual void Start()
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
    
    public virtual void UseSkill(Battler target, int skillNumber)
    {
        Skill skill = characterStats.skills[skillNumber];
        skill.UseSkill(target, this);
    }

    public virtual void BattleSetup(Vector2 newPosition)
    {
        animator.SetTrigger("BattleSetup");

        // makes it so the sprite is above the battle background
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.sortingLayerName = "Battlers";

        // sets its layer to battler

        oldPosition = transform.position;
        animationSaveData = new AnimationSaveData
        {
            name = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name,
            normilizedtime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime
        };
        

        battlePosition = newPosition;
        StartCoroutine(TransitionToBattlePosition( newPosition, .5f));
    }
    public virtual void BattleStart()
    {
        animator.SetTrigger("BattleStart");
        isInBattle = true;
    }
    public virtual void BattleEnd()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.sortingLayerName = "Characters";
        animator.Play(animationSaveData.name, 0, animationSaveData.normilizedtime);
        StartCoroutine(TransitionToOriginalPositions(oldPosition, .5f));
    }
    
    public virtual TemplateContainer GetSelectionTemplate()
    {
        return new TemplateContainer();
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
    IEnumerator TransitionToBattlePosition(Vector3 newPosition, float duration)
    {
        // disable collision
        if (GetComponent<Collider2D>() != null)
        {
            Collider2D collider = GetComponent<Collider2D>();
            collider.enabled = false;
        }

        Vector3 oldPosition = transform.position;
        newPosition.z = 0;
        float timePassed = 0;
        while (transform.position != newPosition)
        {
            timePassed += Time.unscaledDeltaTime;
            transform.position = Vector3.Lerp(oldPosition, newPosition, timePassed / duration);
            yield return null;
        }
        Debug.Log("need to add in position invoke");
    }
    IEnumerator TransitionToOriginalPositions(Vector3 newPosition, float duration)
    {
        Vector3 oldPosition = transform.position;

        float totalTime = 0;

        while (transform.position != newPosition)
        {
            totalTime += Time.unscaledDeltaTime;
            transform.position = Vector3.Lerp(oldPosition, newPosition, totalTime / duration);
            yield return null;
        }
        // enable collision
        if (GetComponent<Collider2D>() != null)
        {
            Collider2D collider = GetComponent<Collider2D>();
            collider.enabled = true;
        }
        Debug.Log("need to add in position invoke");
            //inOverworldPosition?.Invoke();
    }
}
