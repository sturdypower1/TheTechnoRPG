using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopUI : MonoBehaviour
{
    public event EmptyEventHandler OnBuyPressed;

    private UIDocument _UIDoc;
    private VisualElement _background;
    private ScrollView _itemList;
    private VisualTreeAsset _itemButtonTemplate;
    public void EnableUI()
    {
        _background.visible = true;
    }
    public void DisableUI()
    {
        _background.visible = false;
    }
    public void EnableItemList(ShopItem[] shopItems)
    {
        _itemList.visible = true;
        _itemList.Clear();
        foreach (var item in shopItems)
        {
            var itemTemplateCopy = _itemButtonTemplate.CloneTree();
            var itemBackground = itemTemplateCopy.Q<VisualElement>("background");

            var itemName = itemBackground.Q<Label>("item_name");
            itemName.text = item.item.name;
            var itemPrice = itemBackground.Q<Label>("item_price");
            itemPrice.text = item.cost.ToString();
            if(item.cost > InventoryManager.instance.gold)
            {
                itemTemplateCopy.SetEnabled(false);
            }

            _itemList.Add(itemTemplateCopy);
        }
    }
    private void BuyButton()
    {
        OnBuyPressed?.Invoke();
    }

    private void PurchaseItemButton(int cost)
    {

    }

}
