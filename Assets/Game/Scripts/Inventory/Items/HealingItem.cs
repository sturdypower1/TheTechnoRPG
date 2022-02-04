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
    public override void UseItem(GameObject target)
    {
        AudioManager.playSound("Heal");
        Battler battler = target.GetComponent<Battler>();
        battler.Heal(HealingAmount);

        InventoryManager.instance.RemoveItem(this);
    }
    public override bool GetUseability()
    {
        return true;
    }
}
