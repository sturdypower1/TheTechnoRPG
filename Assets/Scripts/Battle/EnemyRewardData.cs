using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRewardData : MonoBehaviour
{
    public int EXP;
    public int gold;
    public EnemyItemData itemData;
}
[System.Serializable]
public struct EnemyItemData
{
    public Item item;
    public float chance;
}
