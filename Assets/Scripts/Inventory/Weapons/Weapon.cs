using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public abstract class Weapon : ScriptableObject, IObtainable
{
    public new string name;
    public string description;
    public int attack;
    /// <summary>
    /// from the special properties of the sword, calculate the damage
    /// </summary>
    public abstract Damage CalculateDamage(Damage damage, GameObject target, GameObject user);
    public void Obtain()
    {
        
    }
}

[System.Serializable]
//[CreateAssetMenu(menuName = "Weapon/Special Weapon")]
public class SpecialWeapon : Weapon
{
    public override Damage CalculateDamage(Damage damage, GameObject target, GameObject user)
    {
        return new Damage();
    }
}
