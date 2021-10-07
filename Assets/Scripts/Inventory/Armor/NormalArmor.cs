using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Armor/Normal Armor")]
public class NormalArmor : Armor
{
    public override Damage CalculateDamage(Damage damage)
    {
        switch (damage.damageType)
        {
            case DamageType.Bleeding:
                return damage;
                break;
        }
         
        damage.damageAmount -= defence;
        return damage;
    }
}
