using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public abstract class Weapon : ScriptableObject, IObtainable
{
    public string WeaponType;
    public string name;
    public string description;
    public int attack;
    /// <summary>
    /// from the special properties of the sword, calculate the damage
    /// </summary>
    public abstract void CalculateDamage();
    public void Obtain()
    {
        
    }
}

[System.Serializable]
//[CreateAssetMenu(menuName = "Weapon/Special Weapon")]
public class SpecialWeapon : Weapon
{
    public override void CalculateDamage()
    {
    }
}
