using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Weapon/Normal Weapon")]
public class NormalWeapon : Weapon
{
    public override Damage CalculateDamage(Damage damage, GameObject target, GameObject user)
    {
        damage.damageAmount += attack;
        return damage;
    }
}
