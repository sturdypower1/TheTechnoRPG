using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField] private ShopItem[] shopItems;
    [SerializeField] private ShopUI shopUI;

    public void Activate()
    {
        shopUI.EnableUI();
    }
    public void Deactivate()
    {
        shopUI.DisableUI();
    }

    private void ShowItems_OnBuyPressed()
    {
        shopUI.EnableItemList(shopItems);
    }

    private void Start()
    {
        shopUI.OnBuyPressed += ShowItems_OnBuyPressed;
    }
}
[System.Serializable]
public struct ShopItem
{
    public Item item;
    public int cost;
}