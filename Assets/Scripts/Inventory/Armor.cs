using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public abstract class Armor : IObtainable
{
    public string ArmorType;
    public string name;
    public string description;
    public int defence;
    public void Obtain()
    {
       
    }
    public abstract void CalculateDamage();
}
[System.Serializable]
public class NormalArmor : Armor
{
    public override void CalculateDamage()
    {
        
    }
}
