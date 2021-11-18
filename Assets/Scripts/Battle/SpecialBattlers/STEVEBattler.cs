using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class STEVEBattler : Battler
{
    public float defendTime;
    public VisualElement SteveSelectorUI;
    private void Update()
    {
        if (BattleManager.instance.isInBattle)
        {
            
            InventoryManager inventory = InventoryManager.instance;
            // there are no items, so don't let them go into the menu
            if (inventory.items.Count == 0)
            {
                SteveSelectorUI.Q<Button>("items").SetEnabled(false);
            }
            else
            {
                SteveSelectorUI.Q<Button>("items").SetEnabled(true);
            }

            Label healthText = SteveSelectorUI.Q<Label>("health_text");
            VisualElement healthBarBase = SteveSelectorUI.Q<VisualElement>("health_bar_base");
            VisualElement healthBar = SteveSelectorUI.Q<VisualElement>("health_bar");

            Label mpText = SteveSelectorUI.Q<Label>("mp_text");
            VisualElement mpBarBase = SteveSelectorUI.Q<VisualElement>("mp_bar_base");
            VisualElement mpBar = SteveSelectorUI.Q<VisualElement>("mp_bar");

            healthBar.style.width = healthBarBase.contentRect.width * ((float)characterStats.stats.health / (float)characterStats.stats.maxHealth);
            healthText.text = "HP: " + characterStats.stats.health.ToString() + "/" + characterStats.stats.maxHealth.ToString();

            mpBar.style.width = mpBarBase.contentRect.width * ((float)characterStats.stats.points / characterStats.stats.maxPoints);
            mpText.text = "MP: " + characterStats.stats.points.ToString() + "/" + characterStats.stats.maxPoints.ToString();
        }
        if (PlayerPartyManager.instance.HasPlayer(this.gameObject))
        {
            SteveSelectorUI.visible = SteveSelectorUI.parent.visible;
        }
    }
    public override void ReEnableMenu()
    {
        animator.SetBool("Defending", false);
        isDefending = false;
        base.ReEnableMenu();


        AudioManager.playSound("menuavailable");
        SteveSelectorUI.SetEnabled(true);
    }
    public override void UpdateMenu()
    {
        base.UpdateMenu();
        VisualElement useBar = SteveSelectorUI.Q<VisualElement>("use_bar");

        useBar.style.width = SteveSelectorUI.contentRect.width * ((useTime) / maxUseTime);
    }
    public override void Defend()
    {
        base.Defend();
        isDefending = true;
        StartWaitCouroutine(defendTime);
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
                    totalDamage = characterStats.equipedWeapon.CalculateDamage(new Damage { damageAmount = damage.damageAmount, damageType = DamageType.Physical }, target, this.gameObject);
                    break;
                case DamageType.Physical:
                    totalDamage = characterStats.equipedWeapon.CalculateDamage(new Damage { damageAmount = damage.damageAmount, damageType = DamageType.Physical }, target, this.gameObject);
                    break;
            }

            target.GetComponent<Battler>().TakeDamage(totalDamage);
        }
    }
    public override void TakeDamage(Damage damage)
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


        if (isDefending)
        {
            trueDamage.damageAmount = 0;
            trueDamage.damageAmount = (int)(((float)trueDamage.damageAmount * .5));
        }

        Label label = new Label();
        label.text = trueDamage.damageAmount.ToString();
        switch (damage.damageType)
        {
            case DamageType.Bleeding:
                label.AddToClassList("message_red");
                Technoblade.instance.AddBlood(damage.damageAmount);
                break;
            case DamageType.Physical:
                if (damagedSound != null)
                {
                    damagedSound.Play();
                }
                label.AddToClassList("message_white");
                break;
        }
        headsUpUI.ui.Q<VisualElement>("messages").Add(label);

        float random = Random.value * 360;
        Vector2 messageDirection = new Vector2(Mathf.Cos(random), Mathf.Sin(random));

        Message newMessage = new Message { timePassed = 0, label = label, direction = messageDirection };
        headsUpUI.messages.Add(newMessage);
        if (characterStats.stats.health > 0)
        {
            characterStats.stats.health -= trueDamage.damageAmount;
        }
        // character should be down
        else if (characterStats.stats.health <= 0)
        {

            isDown = true;
            animator.SetTrigger("Down");

            foreach (Message message in headsUpUI.messages)
            {
                headsUpUI.ui.Q<VisualElement>("messages").Remove(message.label);
            }

            headsUpUI.messages.Clear();
        }


    }
}
