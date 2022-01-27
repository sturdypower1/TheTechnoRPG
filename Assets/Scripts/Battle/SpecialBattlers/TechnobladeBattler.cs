using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class TechnobladeBattler : PlayerBattler
{
    public VisualEffect visualEffect;

    [HideInInspector]
    public bool isInCarnageMode = false;

    public override void Start()
    {
        base.Start();
        battleUI.targetSelected += e => UseSkill(e.target, e.skillNumber);
    }
    
    public override void DealDamage(Damage damage)
    {
        // make sure to set this, since it could record use an old value later
        if (target != null)
        {
            Damage totalDamage = new Damage();
            switch (damage.damageType)
            {
                case DamageType.Bleeding:
                    target.GetComponent<Battler>().AddBleeding(1, 10);
                    totalDamage = characterStats.equipedWeapon.CalculateDamage(new Damage { damageAmount = damage.damageAmount, damageType = DamageType.Physical }, target, this);
                    break;
                case DamageType.Physical:
                    totalDamage = characterStats.equipedWeapon.CalculateDamage(new Damage { damageAmount = damage.damageAmount, damageType = DamageType.Physical }, target, this);
                    break;
            }
            totalDamage.damageAmount *= isInCarnageMode ? 2 : 1;
            target.GetComponent<Battler>().TakeDamage(totalDamage);
        }
    }
    public override void TakeDamage(Damage damage)
    {
        Damage trueDamage = CalculateDamageTaken(damage);

        Label label = new Label();
        label.text = trueDamage.damageAmount.ToString();
        AddDamageMessage(trueDamage);
        var canPlayDamagedSound = damagedSound != null && !damagedSound.isPlaying;
        if (canPlayDamagedSound)
        {
            damagedSound.Play();
        }

        if(!isInCarnageMode)
        {
            characterStats.stats.health -= trueDamage.damageAmount;
            battleUI.UpdateHealth(characterStats.stats.health, characterStats.stats.maxHealth);
            //VFX

            var shouldActivateCarnageMode = characterStats.stats.health <= 0 && characterStats.stats.points > 0;
            if (shouldActivateCarnageMode)
            {
                ActivateCarnageMode();
            }
        }
        else if (isInCarnageMode)
        {
            characterStats.stats.points -= trueDamage.damageAmount;
        }
        
        var shouldBeDown = characterStats.stats.points <= 0 && characterStats.stats.points <= 0;
        if (shouldBeDown)
        {
            DeactivateCarnageMode();
            DownBattler();
        }
    }
    public override Damage CalculateDamageDealt(Damage damage)
    {
        var trueDamage = base.CalculateDamageDealt(damage);
        trueDamage.damageAmount *= isInCarnageMode ? 2 : 1;
        return trueDamage;
    }

    private void Update()
    {
        if (BattleManager.instance.isInBattle)
        {
            InventoryManager inventory = InventoryManager.instance;
            // there are no items, so don't let them go into the menu
            if (inventory.items.Count == 0)
            {
                battleUI.SetItemsOption(false);
            }
            else
            {
                battleUI.SetItemsOption(true);
            }

            battleUI.UpdatePoints(characterStats.stats.points, characterStats.stats.maxPoints);
        }
        // ensures that if he is healed, that he won't be in carnage mode
        if (characterStats.stats.health > 0 && isInCarnageMode)
        {
            DeactivateCarnageMode();
        }
    }
    
    private void ActivateCarnageMode()
    {
        visualEffect.enabled = true;
        isInCarnageMode = true;
        animator.SetBool("isInCarnageMode", true);
    }

    private void DeactivateCarnageMode()
    {
        visualEffect.enabled = false;
        isInCarnageMode = false;
        animator.SetBool("isInCarnageMode", false);
    }
}
