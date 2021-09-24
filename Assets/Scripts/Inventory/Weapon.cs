using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public abstract class Weapon : IObtainable
{
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
public class NormalWeapon : Weapon
{
    public override void CalculateDamage()
    {
    }
}
