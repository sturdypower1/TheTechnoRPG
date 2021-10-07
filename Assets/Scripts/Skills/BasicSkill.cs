using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// deal regular damage
/// </summary>
[System.Serializable]
[CreateAssetMenu(menuName = "Skill/Basic Skill")]
public class BasicSkill : Skill
{
    /// <summary>
    /// how much damage the attack does
    /// </summary>
    public int damage;
    /// <summary>
    /// the name of the animation for the skill
    /// </summary>
    public AnimationClip skillAnimation;

    public DamageType damageType;

    /// <summary>
    /// attack the enemy 
    /// </summary>
    /// <param name="target"></param>
    public override void UseSkill(GameObject target, GameObject user)
    {
        Battler battler = user.GetComponent<Battler>();
        battler.useTime = 0;
        battler.maxUseTime = useTime;

        Animator animator = user.GetComponent<Animator>();
        animator.Play(skillAnimation.name);

        Damage totalDamage = new Damage();

        switch (damageType)
        {
            case DamageType.Bleeding:
                target.GetComponent<Battler>().AddBleeding(1, 10);
                totalDamage = user.GetComponent<CharacterStats>().equipedWeapon.CalculateDamage(new Damage { damageAmount = damage, damageType = DamageType.Physical }, target, user);
                break;
            case DamageType.Physical:
                totalDamage = user.GetComponent<CharacterStats>().equipedWeapon.CalculateDamage(new Damage { damageAmount = damage, damageType = damageType }, target, user);
                break;
        }

        
        
        target.GetComponent<Battler>().TakeDamage(totalDamage);
    }

}
