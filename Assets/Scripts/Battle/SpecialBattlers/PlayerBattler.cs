using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattler : Battler
{
    public CharacterBattleUI battleUI;
    public float defendTime;
    public bool isDefending;

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

    public override void WaitOnSkillFinished(float timeToWait)
    {
        base.WaitOnSkillFinished(timeToWait);
        isDefending = false;
        battleUI.StartRecoveryBarTween(timeToWait);
    }
    public override void Reenable()
    {
        base.Reenable();
        AudioManager.playSound("menuavailable");
        battleUI.
    }

    public override Damage CalculateDamageTaken(Damage damage)
    {
        var trueDamage = base.CalculateDamageTaken(damage);
        if (isDefending)
        {
            trueDamage.damageAmount = (int)(((float)trueDamage.damageAmount * .5));
        }
        return trueDamage;
    }
    public virtual void Defend()
    {
        isDefending = true;
        animator.SetBool("Defending", true);
        StartWaitCouroutine(defendTime);
    }
}
