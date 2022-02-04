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
    public override void UseSkill(Battler target, Battler user)
    {
        user.useTime = 0;
        user.maxUseTime = useTime;
        user.target = target;

        Animator animator = user.GetComponent<Animator>();
        animator.Play(skillAnimation.name);

        Damage totalDamage = new Damage();

        switch (damageType)
        {
            case DamageType.Bleeding:
                totalDamage = user.GetComponent<CharacterStats>().equipedWeapon.CalculateDamage(new Damage { damageAmount = damage, damageType = DamageType.Physical }, target, user);
                break;
            case DamageType.Physical:
                totalDamage = user.GetComponent<CharacterStats>().equipedWeapon.CalculateDamage(new Damage { damageAmount = damage, damageType = damageType }, target, user);
                break;
        }
        user.GetComponent<Battler>().DealDamage(totalDamage);

        BattleManager.instance.StartCoroutine(WaitToUnpauseBattle(useTime));
    }

    IEnumerator WaitToUnpauseBattle(float time)
    {
        yield return new WaitForSecondsRealtime(time);
    }
}
