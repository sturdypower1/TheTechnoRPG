using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattler : Battler
{
    public CharacterBattleUI battleUI;
    public float defendTime;
    public float defenceRecovery;
    public bool isDefending;

    public override void Start()
    {
        base.Start();
        battleUI.targetSelected += e => UseSkill(e.target, e.skillNumber);
    }
    public override void BattleSetup(Vector2 newPosition)
    {
        base.BattleSetup(newPosition);
        battleUI.UpdateHealth(characterStats.stats.health, characterStats.stats.maxHealth);
    }
    public override void BattleStart()
    {
        base.BattleStart();
        battleUI.EnableUI();
    }
    public override void BattleEnd()
    {
        base.BattleEnd();
        battleUI.DisableUI();
    }
    public virtual void Defend()
    {
        isDefending = true;
        animator.SetBool("Defending", true);
        Invoke("WaitOnDefendFinished", defendTime);
    }
    // is invoked later by defend
    public virtual void WaitOnDefendFinished()
    {
        isDefending = false;
        animator.SetBool("Defending", false);
        Invoke("Reenable", defenceRecovery);
    }
    public virtual void PlayerLose()
    {
        battleUI.DisableUI();
        headsUpUI.DisableUI();
        StartCoroutine(TransitionToOriginalPositions(oldPosition, .5f));
    }
    protected override void WaitOnSkillFinished(float timeToWait)
    {
        base.WaitOnSkillFinished(timeToWait);
        battleUI.StartRecoveryBarTween(timeToWait);
    }
    protected override void Reenable()
    {
        base.Reenable();
        AudioManager.playSound("menuavailable");
        battleUI.GoToStartingState();
    }
    protected override Damage CalculateDamageTaken(Damage damage)
    {
        var trueDamage = base.CalculateDamageTaken(damage);
        if (isDefending)
        {
            trueDamage.damageAmount = (int)(((float)trueDamage.damageAmount * .5));
        }
        return trueDamage;
    }
    
}
