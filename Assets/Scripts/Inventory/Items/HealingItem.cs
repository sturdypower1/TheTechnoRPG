using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// will be used to heal the player
/// </summary>
[System.Serializable]
[CreateAssetMenu(menuName = "Scriptable Objects/healing item")]
public class HealingItem : Item
{
    public int HealingAmount;
    public void UseItem()
    {
        Debug.Log("using item");
    }
    public bool GetUseability()
    {
        return true;
    }
}
