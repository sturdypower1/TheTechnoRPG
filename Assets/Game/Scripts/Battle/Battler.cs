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
    public HeadsUpUI headsUpUI;
    public AnimationSaveData animationSaveData;
    public Transform BattleOffset;
    public AudioSource damagedSound;

    /// <summary>
    /// how long the character will have to wait after using a skill
    /// </summary>
    [HideInInspector]
    public float maxUseTime;
    /// <summary>
    /// how much long it has left before it can do another attack
    /// </summary>
    [HideInInspector]
    public float useTime;
    /// <summary>
    /// when true, disables all battle ai and other actions
    /// </summary>
    [HideInInspector]
    public Vector3 battlePosition;
    [HideInInspector]
    public bool isPaused;
    [HideInInspector]
    public Vector3 oldPosition;
    [HideInInspector] public bool isDown;
    [HideInInspector] public CharacterStats characterStats;
    [HideInInspector] public Animator animator;
    [HideInInspector] public PlayableDirector skillDirector;
    /// <summary>
    /// the target position 
    /// </summary>
    [HideInInspector]
    public Battler target;
    
    [HideInInspector]
    public bool isInBattle;
    private bool isUsingSkill;

    private float _currentSkillWaitTime;
    private bool canMakeAction = false;
    

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
    public virtual void ApplyStatusEffect(StatusEffectTypes statusType)
    {
        Type statusEffect = StatusEffect.EnumToStatus(statusType);
        if (statusEffect != null)
        {
            if (GetComponent(statusEffect) == null)
            {
                gameObject.AddComponent(statusEffect);
                headsUpUI.SetStatus(1, statusType);
                gameObject.GetComponent<StatusEffect>().OnLevelChange += OnStatusLevelChange_UpdateUI;
            }
            else
            {
                StatusEffect currentEffect = GetComponent(statusEffect) as StatusEffect;
                currentEffect.TryApplyNextLevel();
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
    public virtual void Heal(int recovery)
    {
        characterStats.stats.health += recovery;
        if (characterStats.stats.health > characterStats.stats.maxHealth)
        {
            characterStats.stats.health = characterStats.stats.maxHealth;
        }
        if (BattleManager.instance.isInBattle)
        {
            headsUpUI.AddMessage(recovery.ToString(), MessageType.Healing);
        }
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
        StartCoroutine(TransitionToBattlePosition(newPosition, .5f));
    }
    public virtual void BattleStart()
    {
        animator.SetTrigger("BattleStart");
        isInBattle = true;
    }
    public virtual void BattleEnd()
    {
        
    }
    public virtual void ReturnToOverworld()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.sortingLayerName = "Characters";
        animator.Play(animationSaveData.name, 0, animationSaveData.normilizedtime);
        StartCoroutine(TransitionToOriginalPositions(oldPosition, .5f));
    }
    protected virtual void DownBattler()
    {
        headsUpUI.DisableUI();

        isDown = true;
        animator.SetTrigger("Down");
    }
    protected virtual Damage CalculateDamageDealt(Damage damage)
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
    protected virtual Damage CalculateDamageTaken(Damage damage)
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
    protected virtual void CheckIfDown()
    {
        if (characterStats.stats.health <= 0)
        {
            DownBattler();
        }
    }
    protected virtual void UseSkill(Battler target, int skillNumber)
    {
        canMakeAction = false;
        Skill skill = characterStats.skills[skillNumber];
        skill.UseSkill(target, this);
        _currentSkillWaitTime = skill.useTime;
    }
    protected virtual void WaitOnSkillFinished(float timeToWait)
    {
        Invoke("Reenable", timeToWait);
    }
    protected virtual void Reenable()
    {
        canMakeAction = true;
    }
    protected void AddDamageMessage(Damage damage)
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

    private void OnStatusLevelChange_UpdateUI(StatusEffect statusEffect, int newLevel)
    {
        headsUpUI.SetStatus(newLevel, StatusEffect.StatusToEnum(statusEffect));
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
    }
    protected IEnumerator TransitionToOriginalPositions(Vector3 newPosition, float duration)
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
    }
    
    
}
