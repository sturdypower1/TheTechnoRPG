using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShopUI : MonoBehaviour
{
    public event EmptyEventHandler OnBuyPressed;
    public event PurchanseItemEvenHandler OnItemPurchaseButton;
    public event EmptyEventHandler OnBackPressed;

    [SerializeField] private VisualTreeAsset itemButtonTemplate;

    private UIDocument _UIDoc;
    private VisualElement _background;
    private ScrollView _itemList;

    private Label _itemDescription;
    private Label _goldLabel;
    
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
        _itemList.Clear();
        foreach (var item in shopItems)
        {
            var itemTemplateCopy = itemButtonTemplate.CloneTree();
            var itemBackground = itemTemplateCopy.Q<VisualElement>("background");

            var itemName = itemBackground.Q<Label>("item_name");
            itemName.text = item.item.name;
            var itemPrice = itemBackground.Q<Label>("item_price");
            itemPrice.text = "cost: " + item.cost.ToString();

            var itemInfoButton = itemBackground.Q<Button>("Info");
            itemInfoButton.clicked += () => InfoButton(item.item.description);

            var itemBuyButton = itemBackground.Q<Button>("item_buy");
            itemBuyButton.clicked += () => PurchaseItemButton(item);
            if(item.cost > InventoryManager.instance.gold)
            {
                itemTemplateCopy.SetEnabled(false);
            }
            _itemList.Add(itemTemplateCopy);
        }
    }
    public void SetGold(int gold)
    {
        _goldLabel.text = "gold: " + gold.ToString();
    }
    private void BackButton()
    {
        DisableUI();
        OnBackPressed?.Invoke();
    }
    private void BuyButton()
    {
        OnBuyPressed?.Invoke();
    }
    private void InfoButton(string text)
    {
        _itemDescription.text = text;
    }
    private void PurchaseItemButton(ShopItem shopItem)
    {
        OnItemPurchaseButton?.Invoke(shopItem);
    }

    private void Awake()
    {
        _UIDoc = GetComponent<UIDocument>();
        var root = _UIDoc.rootVisualElement;

        _background = root.Q<VisualElement>("background");
        _itemList = root.Q<ScrollView>("item_list");

        _goldLabel = root.Q<Label>("gold");

        _itemDescription = _background.Q<Label>("description_text");
        var backButton = _background.Q<Button>("back_button");
        backButton.clicked += BackButton;
    }
}

public delegate void PurchanseItemEvenHandler(ShopItem shopItem);
