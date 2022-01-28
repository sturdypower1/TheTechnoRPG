using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Playables;
using UnityEngine.Timeline;
/// <summary>
/// component that'll be used for all the battlers in a battle
/// </summary>
[RequireComponent(typeof(PlayableDirector))]
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
    /// <summary>
    /// how long the character will have to wait after using a skill
    /// </summary>
    public float maxUseTime;

    public HeadsUpUI headsUpUI;
    [HideInInspector]
    
    public Vector3 oldPosition;

    public Vector3 battlePosition;

    public AnimationSaveData animationSaveData;
    [HideInInspector]

    public bool isDown;
  
    public AudioSource damagedSound;

    public CharacterStats characterStats;
    public Animator animator;
    public PlayableDirector skillDirector;
    
    [HideInInspector]
    public Battler target;
    /// <summary>
    /// the target position 
    /// </summary>
    public Transform BattleOffset;
    [HideInInspector]
    public bool isInBattle;
    bool isUsingSkill;

    float _currentSkillWaitTime;
    bool canMakeAction = false;
    

    public virtual void Start()
    {
        characterStats = GetComponent<CharacterStats>();
        animator = GetComponent<Animator>();
        skillDirector = GetComponent<PlayableDirector>();
        skillDirector.stopped += d => WaitOnSkillFinished(_currentSkillWaitTime);

        if(BattleOffset == null)
        {
            BattleOffset = this.transform;
        }
    }
    
    public virtual void DownBattler()
    {
        isDown = true;
        animator.SetTrigger("Down");
    }
   
    public virtual void ApplyStatusEffect(StatusEffectTypes statusType)
    {
        
        var statusEffect = StatusEffect.EnumToStatus(statusType);
        Debug.Log(statusEffect == typeof(Bleeding));
        if (statusEffect != null)
        {
            if (GetComponent(statusEffect) == null)
            {
                gameObject.AddComponent(statusEffect);
            }
            else
            {
                Debug.Log("make it so it adds levels to the status effect");
            }
        }
    }
    public virtual void ApplyStatusToTarget(StatusEffectTypes statusType)
    {
        target.ApplyStatusEffect(statusType);
    }
    public virtual void TakeDamage(Damage damage) 
    {
        Damage trueDamage = CalculateDamageTaken(damage);
        AddDamageMessage(damage);

        if (characterStats.stats.health > 0)
        {
            characterStats.stats.health -= trueDamage.damageAmount;
        }
        CheckIfDown();
    }
    public virtual void DealDamage(Damage damage) 
    {
        if (target != null)
        {
            Damage trueDamage = CalculateDamageDealt(damage);
            target.GetComponent<Battler>().TakeDamage(trueDamage);
        }
    }
    public virtual Damage CalculateDamageDealt(Damage damage)
    {
        Damage trueDamage = new Damage();

        switch (damage.damageType)
        {
            case DamageType.Bleeding:
                trueDamage = characterStats.equipedWeapon.CalculateDamage(new Damage { damageAmount = damage.damageAmount, damageType = DamageType.Physical }, target, this);
                break;
            case DamageType.Physical:
                trueDamage = characterStats.equipedWeapon.CalculateDamage(new Damage { damageAmount = damage.damageAmount, damageType = DamageType.Physical }, target, this);
                break;
        }
        return trueDamage;
    }
    public virtual Damage CalculateDamageTaken(Damage damage)
    {
        Damage trueDamage = new Damage();
        if (characterStats.equipedArmor != null)
        {
            trueDamage = characterStats.equipedArmor.CalculateDamage(damage); ;
        }
        else
        {
            trueDamage = damage;
        }
        return trueDamage;
    }
    
    public virtual void CheckIfDown()
    {
        if (characterStats.stats.health <= 0)
        {
            DownBattler();
        }
    }

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

        float random = UnityEngine.Random.value * 360;
        Vector2 messageDirection = new Vector2(Mathf.Cos(random), Mathf.Sin(random));

        Message newMessage = new Message { timePassed = 0, label = label, direction = messageDirection };
        headsUpUI.messages.Add(newMessage);
    }
    
    public virtual void UseSkill(Battler target, int skillNumber)
    {
        canMakeAction = false;
        Skill skill = characterStats.skills[skillNumber];
        skill.UseSkill(target, this);
        _currentSkillWaitTime = skill.useTime;
    }
    public virtual void WaitOnSkillFinished(float timeToWait)
    {
        Invoke("Reenable", timeToWait);
    }

    public virtual void Reenable()
    {
        canMakeAction = true;
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
    
    public void AddDamageMessage(Damage damage)
    {
        switch (damage.damageType)
        {
            case DamageType.Bleeding:
                headsUpUI.AddMessage(damage.damageAmount.ToString(), MessageType.BleedingDamage);
                break;
            case DamageType.Physical:
                headsUpUI.AddMessage(damage.damageAmount.ToString(), MessageType.PhysicalDamage);
                break;
        }
    }
}
