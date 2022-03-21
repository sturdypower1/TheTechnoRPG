using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUI : MonoBehaviour
{
    public event EmptyEventHandler OnBackPressed;
    public event EmptyEventHandler OnSettingsPressed;

    private int currentItem = 1;

    private UIDocument UIDoc;
    private VisualElement inventoryBackground;
    private Button backButton;

    private Button equipmentSelectionButton;

    private VisualElement characterSelection;
    private VisualElement skillInfo;
    private VisualElement equipmentInfo;
    private VisualElement itemInfo;

    private VisualElement skillsQuickMenu;
    private VisualElement equipmentQuickMenu;
    private VisualElement itemsQuickMenu;

    private PlayerController _currentCharacter;
    private Action OnCharacterSelectedAction;
    public void EnableUI()
    {
        PlayerPartyManager.instance.EnablePlayerInventoryUI();
        inventoryBackground.visible = true;
        equipmentSelectionButton.Focus();
    }

    public void DisableUI()
    {
        inventoryBackground.visible = false;
        PlayerPartyManager.instance.DisablePlayerInventoryUI();
    }
    private void Awake()
    {
        UIDoc = GetComponent<UIDocument>();
    }
    private void Start()
    {
        var root = UIDoc.rootVisualElement;
        inventoryBackground = root.Q<VisualElement>("inventory_background");

        backButton = inventoryBackground.Q<Button>("pause_back_button");
        backButton.clicked += BackButton;

        var UISelection = inventoryBackground.Q<VisualElement>("ui_selection");
        equipmentSelectionButton = inventoryBackground.Q<Button>("Equipment");

        UISelection.Q<Button>("Equipment").clicked += EquipmentButton;
        UISelection.Q<Button>("Item").clicked += ItemsButton;
        UISelection.Q<Button>("Skills").clicked += SkillsButton;
        UISelection.Q<Button>("Settings").clicked += PauseSettingsButton;

        itemsQuickMenu = inventoryBackground.Q<VisualElement>("items_quickmenu");
        itemsQuickMenu.Q<Button>("use").clicked += ItemUse;
        itemsQuickMenu.Q<Button>("drop").clicked += ItemDrop;
        itemsQuickMenu.Q<Button>("cancel").clicked += ItemCancel;

        equipmentInfo = inventoryBackground.Q<VisualElement>("equipment_info");
        itemInfo = inventoryBackground.Q<VisualElement>("item_selection");
        skillInfo = inventoryBackground.Q<VisualElement>("skill_selection");
        characterSelection = inventoryBackground.Q<VisualElement>("character_selection");

        equipmentQuickMenu = inventoryBackground.Q<VisualElement>("equipment_quickmenu");
        equipmentQuickMenu.Q<Button>("cancel").clicked += EquipmentCancel;

        skillsQuickMenu = inventoryBackground.Q<VisualElement>("skills_quickmenu");

        Button currentWeapon = equipmentInfo.Q<Button>("current_weapon");
        currentWeapon.clicked += () => CurrentEquipmentButton(currentWeapon, Equipment.Weapon);
        equipmentInfo.Q<Button>("current_armor").clicked += () => CurrentEquipmentButton(currentWeapon, Equipment.Armor);
    }
    private void BackButton()
    {
        DisableUI();
        RestartInventoryMenu();
        OnBackPressed?.Invoke();
    }

    public void PauseSettingsButton()
    {
        AudioManager.playSound("menuselect");
        DisableUI();
        RestartInventoryMenu();

        OnSettingsPressed?.Invoke();
    }
    public void SkillsButton()
    {
        AudioManager.playSound("menuselect");
        RestartInventoryMenu();
        OnCharacterSelectedAction = OpenSkillsTab;
        SelecteCharacter();
    }
    private void OpenSkillsTab()
    {
        RestartInventoryMenu();
        skillInfo.visible = true;
        VisualElement currentSkills = skillInfo.Q<VisualElement>("current_skills");
        ScrollView skillList = skillInfo.Q<ScrollView>("skill_list");
        Label skillDesc = skillInfo.Q<Label>("skill_desc");

        CharacterStats stats = _currentCharacter.stats;

        AudioManager.playSound("menuselect");
        skillList.Clear();
        currentSkills.visible = true;
        int i = 1;
        foreach (Skill skill in stats.skills)
        {
            Button skillButton = new Button();
            skillButton.AddToClassList("item_button");
            skillButton.name = "skill" + i.ToString();
            skillButton.focusable = true;
            if (skill.name == "")
            {
                skillButton.text = "None";
            }
            else
            {
                skillButton.text = skill.name;
            }
            // for some reason it tracks the value until its not used, so I used a value that wont change
            int z = i;
            skillButton.clicked += () => CurrentSkillButton(z);
            skillButton.RegisterCallback<FocusEvent>(ev => UpdateDescription(skillDesc, skill.description));
            skillList.Add(skillButton);
            i++;
        }

        // sets the description to the first skill
        string descToDisplay = stats.skills[0].description;
        if (descToDisplay == "")
        {
            skillDesc.text = "No description";
        }
        else
        {
            skillDesc.text = descToDisplay;
        }
    }
    private void SelecteCharacter()
    {
        var players = PlayerPartyManager.instance.players;
        if(players.Count == 1)
        {
            CharacaterSelected(players[0]);
            return;
        }

        characterSelection.visible = true;
        var playerList = characterSelection.Q<ScrollView>("character_list");
        playerList.Clear();
        foreach (PlayerController player in players)
        {
            var playerButton = new Button();
            playerButton.focusable = true;
            playerButton.text = player.stats.stats.characterName;
            playerButton.AddToClassList("item_button");
            playerButton.clicked += () => CharacaterSelected(player);
            playerList.Add(playerButton);
        }
        
    }
    private void CharacaterSelected(PlayerController player)
    {
        _currentCharacter = player;
        characterSelection.visible = false;
        OnCharacterSelectedAction?.Invoke();
    }
    private void CurrentSkillButton(int currentSkillNumber)
    {
        currentItem = currentSkillNumber;
        AudioManager.playSound("menuselect");
    }
    private void EquipmentButton()
    {
        AudioManager.playSound("menuselect");
        RestartInventoryMenu();
        OnCharacterSelectedAction = OpenEquipmentTab;
        SelecteCharacter();
    }
    private void OpenEquipmentTab()
    {
        RestartInventoryMenu();
        AudioManager.playSound("menuselect");

        VisualElement otherEquipmentBase = equipmentInfo.Q<VisualElement>("other_equipment");
        VisualElement currentEquipment = equipmentInfo.Q<VisualElement>("current_equipment");
        Button currentWeaponLabel = equipmentInfo.Q<Button>("current_weapon");

        Button currentArmorLabel = equipmentInfo.Q<Button>("current_armor");
        Button currentCharmLabel = equipmentInfo.Q<Button>("current_charm");
        Label equipmentDesc = equipmentInfo.Q<Label>("equipment_text");

        var inventory = InventoryManager.instance;
        CharacterStats stats = _currentCharacter.stats;

        currentEquipment.visible = true;
        equipmentInfo.visible = true;

        currentWeaponLabel.text = "Weapon: " + stats.equipedWeapon.name;
        currentArmorLabel.text = "Armor: " + stats.equipedArmor.name;
        //currentCharmLabel.text = "Charm: " + characterStatsList[currentCharacter].equipedCharm.name;

        equipmentInfo.Q<Button>("current_weapon").Focus();
    }
    private void CurrentEquipmentButton(Button equipmentButton, Equipment equipmentType)
    {
        AudioManager.playSound("menuchange");
        equipmentQuickMenu.visible = true;
        placeQuickMenu(equipmentButton, equipmentQuickMenu);
        switch (equipmentType)
        {
            case Equipment.Weapon:
                equipmentQuickMenu.Q<Button>("switch").clicked += WeaponSwitchButton;
                break;
            case Equipment.Armor:
                equipmentQuickMenu.Q<Button>("switch").clicked += ArmorSwitchButton;
                break;
        }
    }
    private void EquipmentCancel()
    {
        AudioManager.playSound("menuback");
        equipmentQuickMenu.visible = false;
    }
    private void ItemsButton()
    {
        AudioManager.playSound("menuselect");
        RestartInventoryMenu();
        OnCharacterSelectedAction = OpenItemsTab;
        SelecteCharacter();
    }
    private void OpenItemsTab()
    {
        CharacterStats stats = _currentCharacter.stats;

        itemInfo.visible = true;
        ScrollView itemList = itemInfo.Q<ScrollView>("item_list");
        itemList.Clear();
        Label itemDesc = itemInfo.Q<Label>("item_desc");
        AudioManager.playSound("menuselect");
        InventoryManager inventory = InventoryManager.instance;
        itemList.Focus();
        int i = 0;
        foreach (Item item in inventory.items)
        {
            if (item.name == "")
            {
                int z = i;
                Button itemButton = new Button();
                itemButton.focusable = true;
                itemButton.name = "item" + (z + 1).ToString();
                itemButton.text = "No Name";
                itemButton.AddToClassList("item_button");
                itemList.Add(itemButton);
            }
            else
            {
                int z = i;
                Button itemButton = new Button();
                itemButton.focusable = true;
                itemButton.name = "item" + (z + 1).ToString();
                itemButton.text = item.name;
                itemButton.AddToClassList("item_button");
                itemButton.clicked += () => ItemButton(z + 1);
                // once an item is in focus, change the description accordingly
                string descriptionToShow = inventory.items[z].description.ToString() == "" ? "no description" : inventory.items[z].description.ToString();
                itemButton.RegisterCallback<FocusEvent>(ev => UpdateDescription(itemDesc, descriptionToShow));
                itemList.Add(itemButton);
            }
            i++;
        }
        string itemDescriptionToShow = inventory.items.Count > 0 ? inventory.items[0].description : "";
        if (itemDescriptionToShow == "")
        {
            itemDesc.text = "no description";
        }
        else
        {
            itemDesc.text = itemDescriptionToShow;
        }
    }
    private void ItemButton(int itemNumber)
    {
        InventoryManager inventory = InventoryManager.instance;
        ScrollView itemList = itemInfo.Q<ScrollView>("item_list");
        currentItem = itemNumber;
        AudioManager.playSound("menuchange");
        itemsQuickMenu.visible = true;

        placeQuickMenu(itemList.Q<Button>("item" + itemNumber.ToString()), itemsQuickMenu);
        //all items will have these features
        itemsQuickMenu.Q<Button>("give").SetEnabled(false);
        //add the useable selectables based on the item type
        if (inventory.items[itemNumber - 1].GetUseability())
        {
            itemsQuickMenu.Q<Button>("use").SetEnabled(true);
            itemsQuickMenu.Q<Button>("use").Focus();
        }
        else
        {
            itemsQuickMenu.Q<Button>("use").SetEnabled(false);
            itemsQuickMenu.Q<Button>("cancel").Focus();
        }
    }

    private void ItemUse()
    {
        InventoryManager.instance.items[currentItem - 1].UseItem(_currentCharacter.gameObject);
        _currentCharacter.UpdateInventoryUI();

        itemsQuickMenu.visible = false;
        itemInfo.visible = false;
    }
    private void ItemCancel()
    {
        AudioManager.playSound("menuback");
        itemsQuickMenu.visible = false;
    }
    private void ItemDrop()
    {
        InventoryManager.instance.items.RemoveAt(currentItem - 1);
        //exit items menu
        itemsQuickMenu.visible = false;
        itemInfo.visible = false;
    }


    private void ArmorSwitchButton()
    {
        var inventory = InventoryManager.instance;

        equipmentQuickMenu.Q<Button>("switch").clicked -= ArmorSwitchButton;

        var otherEquipmentBase = equipmentInfo.Q<VisualElement>("other_equipment");
        var currentEquipment = equipmentInfo.Q<VisualElement>("current_equipment");
        var equipmentDesc = equipmentInfo.Q<Label>("equipment_text");
        var otherEquipmentList = otherEquipmentBase.Q<ScrollView>("equipment_list");
        otherEquipmentList.Clear();

        otherEquipmentList.Clear();
        AudioManager.playSound("menuselect");
        // makes the new equipment visable and the current equipment invisable
        otherEquipmentBase.visible = true;
        currentEquipment.visible = false;
        // reseting the switch item
        equipmentQuickMenu.visible = false;

        // show all available equipment switching what to display depending on what is selected
        for (int i = 0; i < inventory.armors.Count; i++)
        {
            int z = i;
            Armor armor = inventory.armors[i];
            Button newButton = new Button();
            newButton.clicked += () => SwitchArmor(z);
            newButton.text = armor.name.ToString();
            newButton.AddToClassList("item_button");
            otherEquipmentList.Add(newButton);
            if (i == 0)
            {
                equipmentDesc.text = armor.description.ToString();
            }
        }
    }
    private void UpdateDescription(Label label, string desc)
    {
        label.text = desc;
    }
    private void placeQuickMenu(Button item, VisualElement quickMenu)
    {
        quickMenu.style.right = item.contentRect.width + item.worldBound.x;
        quickMenu.style.top = item.worldBound.y;
    }
    private void RestartInventoryMenu()
    {
        skillInfo.visible = false;
        equipmentInfo.visible = false;
        itemInfo.visible = false;
        characterSelection.visible = false;
        equipmentInfo.Q<VisualElement>("current_equipment").visible = false;
        equipmentInfo.Q<VisualElement>("other_equipment").visible = false;
        skillsQuickMenu.visible = false;
        equipmentQuickMenu.visible = false;
        itemsQuickMenu.visible = false;

        skillInfo.Q<VisualElement>("current_skills").visible = false;
    }
    private void SwitchArmor(int newArmorNumber)
    {
        var inventory = InventoryManager.instance;

        var otherEquipmentBase = equipmentInfo.Q<VisualElement>("other_equipment");
        var currentEquipment = equipmentInfo.Q<VisualElement>("current_equipment");

        CharacterStats stats = _currentCharacter.stats;
        Armor unEquipedArmor = stats.equipedArmor;
        stats.equipedArmor = inventory.armors[newArmorNumber];
        // move the unequiped item to the weaponinventory
        inventory.armors.RemoveAt(newArmorNumber);
        inventory.armors.Insert(0, unEquipedArmor);

        // update equipment info
        // also update character stats!!!!!!!
        //equipmentDesc.text = characterStats.equipedWeapon.description.ToString();
        equipmentInfo.Q<Button>("current_armor").text = "Armor: " + stats.equipedArmor.name.ToString();

        AudioManager.playSound("menuselect");
        currentEquipment.visible = true;
        otherEquipmentBase.visible = false;
    }
    private void WeaponSwitchButton()
    {
        var inventory = InventoryManager.instance;

        equipmentQuickMenu.Q<Button>("switch").clicked -= WeaponSwitchButton;

        var otherEquipmentBase = equipmentInfo.Q<VisualElement>("other_equipment");
        var currentEquipment = equipmentInfo.Q<VisualElement>("current_equipment");
        var equipmentDesc = equipmentInfo.Q<Label>("equipment_text");
        var otherEquipmentList = otherEquipmentBase.Q<ScrollView>("equipment_list");
        otherEquipmentList.Clear();

        otherEquipmentList.Clear();
        AudioManager.playSound("menuselect");
        // makes the new equipment visable and the current equipment invisable
        otherEquipmentBase.visible = true;
        currentEquipment.visible = false;
        // reseting the switch item
        equipmentQuickMenu.visible = false;

        // show all available equipment switching what to display depending on what is selected
        for (int i = 0; i < inventory.weapons.Count; i++)
        {
            int z = i;
            Weapon weapon = inventory.weapons[i];
            Button newButton = new Button();
            newButton.clicked += () => SwitchWeapon(z);
            newButton.text = weapon.name.ToString();
            newButton.AddToClassList("item_button");
            otherEquipmentList.Add(newButton);
            if (i == 0)
            {
                equipmentDesc.text = weapon.description.ToString();
            }
        }
    }
    private void SwitchWeapon(int newWeaponNumber)
    {
        var inventory = InventoryManager.instance;

        var otherEquipmentBase = equipmentInfo.Q<VisualElement>("other_equipment");
        var currentEquipment = equipmentInfo.Q<VisualElement>("current_equipment");

        CharacterStats stats = _currentCharacter.stats;
        Weapon unEquipedWeapon = stats.equipedWeapon;
        stats.equipedWeapon = inventory.weapons[newWeaponNumber];
        // move the unequiped item to the weaponinventory
        inventory.weapons.RemoveAt(newWeaponNumber);
        inventory.weapons.Insert(0, unEquipedWeapon);

        // update equipment info
        // also update character stats!!!!!!!
        //equipmentDesc.text = characterStats.equipedWeapon.description.ToString();
        equipmentInfo.Q<Button>("current_weapon").text = "Weapon: " + stats.equipedWeapon.name.ToString();

        AudioManager.playSound("menuselect");
        currentEquipment.visible = true;
        otherEquipmentBase.visible = false;
    }
}
