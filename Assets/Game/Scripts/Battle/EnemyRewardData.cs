using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRewardData : MonoBehaviour
{
    public int EXP;
    public int gold;
    public EnemyItemData itemData;
    /// <summary>
    /// destroys this enemy after it's defeated
    /// </summary>
    public bool destroyOnDefeat;
}
[System.Serializable]
public struct EnemyItemData
{
    public Item item;
    public float chance;
}
