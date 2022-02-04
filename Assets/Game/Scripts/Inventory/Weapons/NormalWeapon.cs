using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Weapon/Normal Weapon")]
public class NormalWeapon : Weapon
{
    public override Damage CalculateDamage(Damage damage, Battler target, Battler user)
    {
        damage.damageAmount += attack + user.GetComponent<CharacterStats>().stats.attack;
        return damage;
    }
}
