using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField] private ShopItem[] shopItems;
    [SerializeField] private ShopUI shopUI;
    [SerializeField] private Cinemachine.CinemachineVirtualCamera shopCamera;
    public void Activate()
    {
        shopCamera.Priority = 11;

        UpdateGold();
        shopUI.EnableUI();
        shopUI.EnableItemList(shopItems);
        MainGameManager.instance.DisableOverworldOverlay();
    }
    public void Deactivate()
    {
        shopCamera.Priority = 1;
        shopUI.DisableUI();
    }

    private void ShowItems_OnBuyPressed()
    {
        shopUI.EnableItemList(shopItems);
    }

    private void PurchaseItem_OnPurchasePressed(ShopItem shopItem)
    {

        TryPurchaseItem(shopItem);
    }
    private void UpdateGold()
    {
        shopUI.SetGold(InventoryManager.instance.gold);
    }
    private void TryPurchaseItem(ShopItem shopItem)
    {
        if(InventoryManager.instance.gold >= shopItem.cost)
        {
            InventoryManager.instance.gold -= shopItem.cost;
            InventoryManager.instance.AddItem(shopItem.item);
            UpdateGold();
        }
    }
    private void ResumeGameworld_OnBackPressed()
    {
        Deactivate();
        MainGameManager.instance.ResumeGameworld();
    }
    private void Start()
    {
        shopUI.OnBackPressed += ResumeGameworld_OnBackPressed;
        shopUI.OnItemPurchaseButton += PurchaseItem_OnPurchasePressed;
    }
}
[System.Serializable]
public struct ShopItem
{
    public Item item;
    public int cost;
}