using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public abstract class Weapon :  IObtainable
{
    public string description;
    public int attack;
    /// <summary>
    /// from the special properties of the sword, calculate the damage
    /// </summary>
    public abstract Damage CalculateDamage(Damage damage, Battler target, Battler user);
    public override void Obtain()
    {
        
    }
}

[System.Serializable]
//[CreateAssetMenu(menuName = "Weapon/Special Weapon")]
public class SpecialWeapon : Weapon
{
    public override Damage CalculateDamage(Damage damage, Battler target, Battler user)
    {
        return new Damage();
    }
}
