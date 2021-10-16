using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class TechnobladeBattler : Battler
{
    public bool isInCarnageMode;

    public VisualEffect visualEffect;
    private void Update()
    {
        // ensures that if he is healed, that he won't be in carnage mode
        if (characterStats.stats.health > 0)
        {
            visualEffect.enabled = false;
            isInCarnageMode = false;
            animator.SetBool("isInCarnageMode", false);
        }
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

            totalDamage.damageAmount *= isInCarnageMode ? 2 : 1;
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




        Label label = new Label();
        label.text = damage.damageAmount.ToString();
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
            characterStats.stats.health -= damage.damageAmount;
            //VFX
            visualEffect.enabled = false;
            isInCarnageMode = false;
            animator.SetBool("isInCarnageMode", false);
            if (characterStats.stats.health <= 0 && characterStats.stats.points > 0)
            {
                visualEffect.enabled = true;
                isInCarnageMode = true;
                animator.SetBool("isInCarnageMode", true);
            }
        }
        else if (characterStats.stats.points > 0)
        {
            characterStats.stats.points -= damage.damageAmount;
        }
        // character should be down
        else if (characterStats.stats.points <= 0)
        {
            visualEffect.enabled = false;
            isInCarnageMode = false;
            animator.SetBool("isInCarnageMode", false);

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
