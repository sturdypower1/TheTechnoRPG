using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemListHolder : MonoBehaviour
{
    Item[] items;
}

public struct ShopItem
{
    public Item item;
    public int cost;
}