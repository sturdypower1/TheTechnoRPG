using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    /// <summary>
    /// the currently selected item or skill
    /// </summary>
    int currentItem = 1;
    /// <summary>
    /// triggered when starting the game
    /// </summary>
    public delegate void StartGameEventHandler(object sender, StartGameEventArgs e);

    public static UIManager instance;
    public UIDocument UIDoc;
    // will be used to enable/ disable interactive button
    [HideInInspector]
    public bool isInteractiveEnabled;
    public bool isStartingOnTitle;
    
    public VisualElement root;
    public Button textBoxUI;

    public Button interactiveButton;
    public bool isInteractivePressed;

    //used to make sure things that shouldn't be focused aren't focused on
    [HideInInspector]
    VisualElement nullFocus;
    // reference to the titlebackground
    [HideInInspector]
    public VisualElement titleBackground;
    
    VisualElement creditsBackground;
    VisualElement fileSelectBackground;
    VisualElement settingsBackground;
    VisualElement pauseBackground;
    Button settingsBackButton;

    VisualElement itemsQuickMenu;
    VisualElement equipmentQuickMenu;
    VisualElement skillsQuickMenu;
    VisualElement equipmentInfo;
    VisualElement itemInfo;
    [HideInInspector]
    public VisualElement overworldOverlay;
    VisualElement skillInfo;
    VisualElement overworldSaveFileSelect;

    // need reference to battleMenuSystem
    //need refernce to inkdisplaySystem
    // need reference to save system
    private void Awake() {
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        
        // setting up the ui
        AudioManager.playSong("menuMusic");
            root = UIDoc.rootVisualElement;
            nullFocus = root.Q<VisualElement>("null_focus");
            // for the title screen
            {
            titleBackground = root.Q<VisualElement>("title_background");
            creditsBackground = root.Q<VisualElement>("credits_background");
            fileSelectBackground = root.Q<VisualElement>("file_select");

            titleBackground.Q<Button>("Start").clicked += StartButton;
            titleBackground.Q<Button>("Continue").clicked += continueButton;
            titleBackground.Q<Button>("Options").clicked += OptionsButton;
            titleBackground.Q<Button>("Credits").clicked += CreditsButton;
            titleBackground.Q<Button>("Exit").clicked += ExitButton;

            creditsBackground.Q<Button>("credits_back_button").clicked += CreditsBackButton;

            fileSelectBackground.Q<TemplateContainer>("save_file1").Q<Button>("background").clicked += () => ContinueGameButton(1);
            fileSelectBackground.Q<TemplateContainer>("save_file2").Q<Button>("background").clicked += () => ContinueGameButton(2);

            fileSelectBackground.Q<Button>("load_back_button").clicked += LoadSaveFileBackButton;
            }
            //for the credits menu, links all the items to the links
            {
                Button technoYoutubeButton = root.Q<Button>("techno_youtube");
                Button technoTwitterButton = root.Q<Button>("techno_twitter");

                Button tommyYoutubeButton = root.Q<Button>("tommy_youtube");
                Button tommyTwitterButton = root.Q<Button>("tommy_twitter");
                Button tommyTwitchButton = root.Q<Button>("tommy_twitch");

                Button wilburYoutubeButton = root.Q<Button>("wilbur_youtube");
                Button wilburTwitterButton = root.Q<Button>("wilbur_twitter");
                Button wilburTwitchButton = root.Q<Button>("wilbur_twitch");
                technoYoutubeButton.RegisterCallback<ClickEvent>(ev => LinkSender.sendToTechno(websites.youtube));
                technoTwitterButton.RegisterCallback<ClickEvent>(ev => LinkSender.sendToTechno(websites.twitter));

                tommyYoutubeButton.RegisterCallback<ClickEvent>(ev => LinkSender.sendToTommy(websites.youtube));
                tommyTwitterButton.RegisterCallback<ClickEvent>(ev => LinkSender.sendToTommy(websites.twitter));
                tommyTwitchButton.RegisterCallback<ClickEvent>(ev => LinkSender.sendToTommy(websites.twitch));

                wilburYoutubeButton.RegisterCallback<ClickEvent>(ev => LinkSender.sendToWilbur(websites.youtube));
                wilburTwitterButton.RegisterCallback<ClickEvent>(ev => LinkSender.sendToWilbur(websites.twitter));
                wilburTwitchButton.RegisterCallback<ClickEvent>(ev => LinkSender.sendToWilbur(websites.twitch));
                }
            // for the settings menu
            {
                settingsBackground = root.Q<VisualElement>("settings_background");

                settingsBackButton = settingsBackground.Q<Button>("settings_back_button");

                Button volumeButton = settingsBackground.Q<Button>("volume_button");
                volumeButton.RegisterCallback<FocusEvent>(ev => ActivateSettingsTab(settingsBackground.Q<VisualElement>("volume_controls")));
                Button bindingsButton = settingsBackground.Q<Button>("bindings_button");
                bindingsButton.SetEnabled(false);
                bindingsButton.RegisterCallback<FocusEvent>(ev => ActivateSettingsTab(settingsBackground.Q<VisualElement>("bindings_controls")));
                Button othersButton = settingsBackground.Q<Button>("others_button");
                othersButton.RegisterCallback<FocusEvent>(ev => ActivateSettingsTab(settingsBackground.Q<VisualElement>("other_controls")));
                settingsBackground.Q<Slider>("volume_slider").RegisterValueChangedCallback(ev => ChangeVolume(ev.newValue));
                settingsBackground.Q<Button>("title_return_button").clicked += SettingsReturnToTitleButton;
                }
            // for pause menu
            {
                pauseBackground = root.Q<VisualElement>("pause_background");

                pauseBackground.Q<Button>("pause_back_button").clicked += PauseBackButton;
                
                pauseBackground.Q<Button>("Equipment").clicked += EquipmentButton;
                pauseBackground.Q<Button>("Item").clicked += PauseItemsButton;
                pauseBackground.Q<Button>("Skills").clicked += PauseSkillsButton;
                pauseBackground.Q<Button>("Settings").clicked += PauseSettingsButton;

                itemsQuickMenu = pauseBackground.Q<VisualElement>("items_quickmenu");
                itemsQuickMenu.Q<Button>("use").clicked += ItemUse;
                itemsQuickMenu.Q<Button>("drop").clicked += ItemDrop;
                itemsQuickMenu.Q<Button>("cancel").clicked += ItemCancel;

                equipmentInfo = pauseBackground.Q<VisualElement>("equipment_info");
                itemInfo = pauseBackground.Q<VisualElement>("item_selection");
                skillInfo = pauseBackground.Q<VisualElement>("skill_selection");

                equipmentQuickMenu = pauseBackground.Q<VisualElement>("equipment_quickmenu");
                equipmentQuickMenu.Q<Button>("cancel").clicked += EquipmentCancel;

                skillsQuickMenu = pauseBackground.Q<VisualElement>("skills_quickmenu");
                //skillsQuickMenu.Q<Button>("switch").clicked += SkillSwitchButton;
                skillsQuickMenu.Q<Button>("cancel").clicked += SkillCancel;

                Button currentWeapon = equipmentInfo.Q<Button>("current_weapon");
                currentWeapon.clicked += () => CurrentEquipmentButton(currentWeapon, Equipment.Weapon);
                equipmentInfo.Q<Button>("current_armor").clicked += () => CurrentEquipmentButton(currentWeapon, Equipment.Armor);
                }
            // for overworld overlay
            {
                overworldOverlay = root.Q<VisualElement>("overworld_overlay");
                overworldOverlay.Q<Button>("pause_button").clicked += ActivatePauseMenu;

                overworldOverlay.Q<Button>("interactive_item_check").SetEnabled(false);
                overworldOverlay.Q<Button>("interactive_item_check").clicked += InteractButton;}
            // for battle menu
            {
            VisualElement battleBackground = root.Q<VisualElement>("battle_background");
            VisualElement battleUI = battleBackground.Q<VisualElement>("BattleUI");
            VisualElement losingBackground = root.Q<VisualElement>("losing_screen");
            losingBackground.Q<Button>("continue").clicked += BattleMenuManager.instance.ContinueButton;
            losingBackground.Q<Button>("title").clicked += BattleMenuManager.instance.LossReturnToTitleButton;
            
            // should be technoblade
            Button technobladeBattleUI = battleUI.Q<Button>("character1");
            technobladeBattleUI.Q<Button>("fight").clicked += () => BattleMenuManager.instance.AttackButton(0);
            technobladeBattleUI.Q<Button>("skills").clicked += () => BattleMenuManager.instance.SkillsButton(0);
            technobladeBattleUI.Q<Button>("items").clicked += () => BattleMenuManager.instance.ItemsButton(0);

            battleBackground.Q<Button>("selector_back_button").clicked += BattleMenuManager.instance.EnemySelectBack;
            battleBackground.Q<Button>("skills_back_button").clicked += BattleMenuManager.instance.SkillsBackButton;
            battleBackground.Q<Button>("items_back_button").clicked += BattleMenuManager.instance.ItemsBackButton;
            }
            overworldSaveFileSelect = root.Q<VisualElement>("overworld_file_select");
            TemplateContainer fileContainer1 = overworldSaveFileSelect.Q<TemplateContainer>("save_file1");
            fileContainer1.Q<Button>("background").clicked += () =>  SaveAndLoadManager.instance.SaveGame(1);
            fileContainer1.Q<Button>("background").clicked += () =>  SaveAndLoadManager.instance.UpdateSaveFile(fileContainer1.Q<Button>("background"), 1);

             interactiveButton = overworldOverlay.Q<Button>("interactive_item_check");

        TemplateContainer fileContainer2 = overworldSaveFileSelect.Q<TemplateContainer>("save_file2");
            fileContainer2.Q<Button>("background").clicked += () =>  SaveAndLoadManager.instance.SaveGame(2);
            fileContainer2.Q<Button>("background").clicked += () =>  SaveAndLoadManager.instance.UpdateSaveFile(fileContainer2.Q<Button>("background"), 2);
            overworldSaveFileSelect.Q<Button>("save_back_button").clicked += SaveBackButton;

            textBoxUI = root.Q<Button>("textbox");
            textBoxUI.clicked += InkManager.instance.ContinueText;
            if(!isStartingOnTitle){
                titleBackground.visible = false;
                overworldOverlay.visible = true;
            }
            

    }

    // Update is called once per frame
    void Update()
    {
        // disables the interactive button when there isn't any near by interactives
        
    }
    private void FixedUpdate()
    {
        if (isInteractiveEnabled)
        {
            isInteractiveEnabled = false;
        }
        else
        {
            isInteractivePressed = false;
            interactiveButton.SetEnabled(false);
        }
    }

    /// <summary>
    /// set the focus to the null focus. prevents some glitches
    /// </summary>
    public void ResetFocus(){
        nullFocus.Focus();
    }
    /// <summary>
    /// the functionality for the back button of the save files, used in the title screen
    /// </summary>
    private void LoadSaveFileBackButton(){
        AudioManager.playSound("menuback");
        titleBackground.visible = true;
        fileSelectBackground.visible = false;
    }
    /// <summary>
    /// the functionality for the back button of the save files, used in the overworld
    /// </summary>
    private void SaveBackButton(){
        //TODO: unpause

        textBoxUI.visible = false;
        PlayerInputManager.instance.EnableInput();
        overworldOverlay.visible = true;
        overworldSaveFileSelect.visible = false;
    }
    /// <summary>
    /// set the volume
    /// </summary>
    /// <param name="newVolume">the new volume</param> 
    private void ChangeVolume(float newVolume){
        AudioListener.volume = newVolume;
    }
    /// <summary>
    /// interact with any close interactable object
    /// </summary>
    private void InteractButton(){
        // check if it is in the overworld first
        if (!InkManager.instance.isCurrentlyDisplaying&& isInteractiveEnabled)
        {
            isInteractivePressed = true;
        }
    }
    /// <summary>
    /// allows the interactable button to be enabled
    /// </summary>
    public void EnableInteractive(){
        isInteractiveEnabled = true;
        interactiveButton.SetEnabled(true);
    }
    /// <summary>
    /// activate the pause menu, setting all the character values
    /// </summary>
    private void ActivatePauseMenu(){
        // add pause to the game
        PlayerInputManager.instance.DisableInput();

        overworldOverlay.visible = false;
        pauseBackground.visible = true;

        VisualElement currentCharacterUI = pauseBackground.Q<VisualElement>("character" + (1).ToString());

        Label healthBarText = currentCharacterUI.Q<Label>("health_text");
        VisualElement healthBarBase = currentCharacterUI.Q<VisualElement>("health_bar_base");
        VisualElement healthBar = currentCharacterUI.Q<VisualElement>("health_bar");

        VisualElement bloodBar = currentCharacterUI.Q<VisualElement>("blood_bar");
        VisualElement bloodBarBase = currentCharacterUI.Q<VisualElement>("blood_bar_base");
        Label bloodBarText = currentCharacterUI.Q<Label>("blood_text");

        Label currentLevel = currentCharacterUI.Q<Label>("current_level");
        Label currentAttack = currentCharacterUI.Q<Label>("current_attack");
        Label currentDefence = currentCharacterUI.Q<Label>("current_defence");
        Label neededEXP = currentCharacterUI.Q<Label>("EXP");

        CharacterStats technoStats = Technoblade.instance.stats;

        //currentLevel.text = "LVL " + levelData.currentLVL.ToString();
        currentAttack.text = "ATK: " + (technoStats.stats.attack + technoStats.equipedWeapon.attack);
        currentDefence.text = "DEF: " + (technoStats.stats.defence + technoStats.equipedArmor.defence);
        //neededEXP.text = "EXP needed: " + (levelData.requiredEXP - levelData.currentEXP).ToString();

        healthBar.style.width = healthBarBase.contentRect.width * ((float)technoStats.stats.health / (float)technoStats.stats.maxHealth);
        healthBarText.text = "HP: " + technoStats.stats.health.ToString() + "/" + technoStats.stats.maxHealth.ToString();

        bloodBar.style.width = bloodBarBase.contentRect.width * ((float)technoStats.stats.points / (float)technoStats.stats.maxPoints);
        bloodBarText.text = "Blood: " + technoStats.stats.points.ToString() + "/" + technoStats.stats.maxPoints.ToString();
    }
    /// <summary>
    /// start the game
    /// </summary>
    public void StartButton(){
        titleBackground.visible = false;
        overworldOverlay.visible = true;
        // start new game from load and save system
        SceneManager.LoadSceneAsync("BeforeThePyramid");
    }
    /// <summary>
    /// open up the save ui to load a game
    /// </summary>
    public void continueButton(){
        AudioManager.playSound("menuselect");
        // load ui using the save and load system

    }
    /// <summary>
    /// open up the settings menu
    /// </summary>
    public void OptionsButton(){
        AudioManager.playSound("menuselect");

        settingsBackButton.clicked += ToTitleSettingsBackButton;

        titleBackground.visible = false;
        settingsBackground.visible = true;
    }
    /// <summary>
    /// from the settings, go back to the title screen
    /// </summary>
    public void ToTitleSettingsBackButton(){
        AudioManager.playSound("menuback");
        DeActivateSettingsTabs();
        settingsBackground.visible = false;
        titleBackground.visible = true;
        titleBackground.Focus();
        settingsBackButton.clicked -= ToTitleSettingsBackButton;
    }

    /// <summary>
    /// go to the credits menu
    /// </summary>
    public void CreditsButton(){
        AudioManager.playSound("menuchange");
        creditsBackground.visible = true;
        titleBackground.visible = false;
        creditsBackground.Q<Button>("credits_back_button").Focus();
    }
    /// <summary>
    /// quit the application
    /// </summary>
    public void ExitButton(){

    }
    /// <summary>
    /// from the credits menu, return to the tile screen
    /// </summary>
    public void CreditsBackButton(){
        AudioManager.playSound("menuback");
        creditsBackground.visible = false;
        titleBackground.visible = true;
    }
    /// <summary>
    /// load the selected save file number
    /// </summary>
    /// <param name="saveFileNubmer">the save file number</param>
    public void ContinueGameButton(int saveFileNubmer){
        overworldOverlay.visible = true;
        titleBackground.visible = false;
        fileSelectBackground.visible = false;
        // use the save system to continue
    }
    /// <summary>
    /// disable all the other tabs, and activate this one
    /// </summary>
    /// <param name="tab"></param>
    public void ActivateSettingsTab(VisualElement tab){
        AudioManager.playSound("menuchange");
        DeActivateSettingsTabs();
        tab.visible = true;
    }
    /// <summary>
    /// deactivate all setting tabs
    /// </summary>
    public void DeActivateSettingsTabs(){
        settingsBackground.Q<VisualElement>("volume_controls").visible = false;
        settingsBackground.Q<VisualElement>("bindings_controls").visible = false;
        settingsBackground.Q<VisualElement>("other_controls").visible = false;
    }
    /// <summary>
    /// load all of the equiment info of technoblade
    /// </summary>
    public void EquipmentButton(){
        RestartPauseMenu();
        AudioManager.playSound("menuselect");

        VisualElement otherEquipmentBase = equipmentInfo.Q<VisualElement>("other_equipment");
        VisualElement currentEquipment = equipmentInfo.Q<VisualElement>("current_equipment");
        Button currentWeaponLabel = equipmentInfo.Q<Button>("current_weapon");
        Button currentArmorLabel = equipmentInfo.Q<Button>("current_armor");
        Button currentCharmLabel = equipmentInfo.Q<Button>("current_charm");
        Label equipmentDesc = equipmentInfo.Q<Label>("equipment_text");

        var inventory = InventoryManager.instance;
        CharacterStats technoStats = Technoblade.instance.gameObject.GetComponent<CharacterStats>();

        equipmentInfo.visible = true;

        currentWeaponLabel.text = "Weapon: " + technoStats.equipedWeapon.name;
        currentArmorLabel.text = "Armor: " + technoStats.equipedArmor.name;
        //currentCharmLabel.text = "Charm: " + characterStatsList[currentCharacter].equipedCharm.name;

        equipmentInfo.Q<Button>("current_weapon").Focus();

    }
    /// <summary>
    /// open up the quickmenu for the currently selected equipment
    /// </summary>
    /// <param name="equipmentButton"></param>
    /// <param name="equipmentType"></param>
    private void CurrentEquipmentButton(Button equipmentButton, Equipment equipmentType){
        equipmentQuickMenu.visible = true;
        placeQuickMenu(equipmentButton, equipmentQuickMenu);
        switch(equipmentType){
            case Equipment.Weapon:
                equipmentQuickMenu.Q<Button>("switch").clicked += WeaponSwitchButton;
            break;
            case Equipment.Armor:
                equipmentQuickMenu.Q<Button>("switch").clicked += ArmorSwitchButton;
            break;
        }
    }
    /// <summary>
    /// exit the equipment quickmenu
    /// </summary>
    private void EquipmentCancel(){
        AudioManager.playSound("menuback");
        equipmentQuickMenu.visible = false;
    }
    /// <summary>
    /// set up the switching tab for armor
    /// </summary>
    private void ArmorSwitchButton(){
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
    /// <summary>
    /// switch the selected armor with the current armor peice
    /// </summary>
    /// <param name="newArmorNumber"></param>
    private void SwitchArmor(int newArmorNumber){
        var inventory = InventoryManager.instance;

        var otherEquipmentBase = equipmentInfo.Q<VisualElement>("other_equipment");
        var currentEquipment = equipmentInfo.Q<VisualElement>("current_equipment");

        CharacterStats technoStats = Technoblade.instance.gameObject.GetComponent<CharacterStats>();
        Armor unEquipedArmor = technoStats.equipedArmor;
        technoStats.equipedArmor = inventory.armors[newArmorNumber];
        // move the unequiped item to the weaponinventory
        inventory.armors.RemoveAt(newArmorNumber);
        inventory.armors.Insert(0, unEquipedArmor);

        // update equipment info
        // also update character stats!!!!!!!
        //equipmentDesc.text = characterStats.equipedWeapon.description.ToString();
        equipmentInfo.Q<Button>("current_armor").text = "Armor: " + technoStats.equipedArmor.name.ToString();

        AudioManager.playSound("menuselect");
        currentEquipment.visible = true;
        otherEquipmentBase.visible = false;
    }
    /// <summary>
    /// set up the switching tab for weapons
    /// </summary>
    private void WeaponSwitchButton(){
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
    /// <summary>
    /// switch the current weapon with the selected weapon
    /// </summary>
    /// <param name="newWeaponNumber">the selected weapon number</param>
    private void SwitchWeapon(int newWeaponNumber){
        var inventory = InventoryManager.instance;

        var otherEquipmentBase = equipmentInfo.Q<VisualElement>("other_equipment");
        var currentEquipment = equipmentInfo.Q<VisualElement>("current_equipment");

        CharacterStats technoStats = Technoblade.instance.gameObject.GetComponent<CharacterStats>();
        Weapon unEquipedWeapon = technoStats.equipedWeapon;
        technoStats.equipedWeapon = inventory.weapons[newWeaponNumber];
        // move the unequiped item to the weaponinventory
        inventory.weapons.RemoveAt(newWeaponNumber);
        inventory.weapons.Insert(0, unEquipedWeapon);

        // update equipment info
        // also update character stats!!!!!!!
        //equipmentDesc.text = characterStats.equipedWeapon.description.ToString();
        equipmentInfo.Q<Button>("current_weapon").text = "Weapon: " + technoStats.equipedWeapon.name.ToString();

        AudioManager.playSound("menuselect");
        currentEquipment.visible = true;
        otherEquipmentBase.visible = false;
    }
    /// <summary>
    /// open up the tab for items
    /// </summary>
    public void PauseItemsButton(){

    }
    /// <summary>
    /// update a label's description
    /// </summary>
    /// <param name="label"></param>
    /// <param name="desc"></param>
    private void UpdateDescription(Label label, string desc){
        label.text = desc;
    }
    /// <summary>
    /// open the skill's tab on the pause menu
    /// </summary>
    public void PauseSkillsButton(){     
    }
    /// <summary>
    /// disable the skill quickmenu
    /// </summary>
    private void SkillCancel(){
        AudioManager.playSound("menuback");
        skillsQuickMenu.visible = false;
    }
    /// <summary>
    /// activated when selecting a skill, sets current item
    /// </summary>
    /// <param name="currentSkillNumber"></param>
    private void CurrentSkillButton(int currentSkillNumber){
        currentItem = currentSkillNumber;
        AudioManager.playSound("menuselect");
}
    /// <summary>
    /// go to the settings menu from the pause menu
    /// </summary>
    public void PauseSettingsButton(){
        AudioManager.playSound("menuselect");
        //select the ui
        settingsBackground.visible = true;
        settingsBackButton.clicked += ReturnToPauseMenu;
        pauseBackground.visible = false;

        RestartPauseMenu();
    }
    /// <summary>
    /// return to the pause menu from the settings menu
    /// </summary>
    public void ReturnToPauseMenu(){
        DeActivateSettingsTabs();
        settingsBackground.visible = false;
        pauseBackground.visible = true;
        settingsBackButton.clicked -= ReturnToPauseMenu;
    }
    /// <summary>
    /// from the pause menu, return to the overworld
    /// </summary>
    private void PauseBackButton(){
    }
    /// <summary>
    /// make all of the pause menu tabs invisable
    /// </summary>
    public void RestartPauseMenu(){
        skillInfo.visible = false;
        equipmentInfo.visible = false;
        itemInfo.visible = false;
        equipmentInfo.Q<VisualElement>("current_equipment").visible = false;
        equipmentInfo.Q<VisualElement>("other_equipment").visible = false;
        skillsQuickMenu.visible = false;
        equipmentQuickMenu.visible = false;
        itemsQuickMenu.visible = false;

        skillInfo.Q<VisualElement>("current_skills").visible = false;
    }
    /// <summary>
    /// open up the quick menu for the current item
    /// </summary>
    /// <param name="itemNumber"></param>
    public void PauseItemButton(int itemNumber){
    }
    /// <summary>
    /// place a quickmenu next to the currently selected box, should change to mouse position
    /// </summary>
    /// <param name="item"></param>
    /// <param name="quickMenu"></param>
    private void placeQuickMenu(Button item, VisualElement quickMenu){
        quickMenu.style.right = item.contentRect.width + item.worldBound.x;
        quickMenu.style.top = item.worldBound.y;
    }
    /// <summary>
    /// when in the overworld and on the settings menu, return to the title
    /// </summary>
    private void SettingsReturnToTitleButton(){
    }
    /// <summary>
    /// use the currently selected item
    /// </summary>
    private void ItemUse(){
    }
    /// <summary>
    /// unload the item quickmenu
    /// </summary>
    private void ItemCancel(){
        AudioManager.playSound("menuback");
        itemsQuickMenu.visible = false;
    } 
    /// <summary>
    /// drop the currently selected item
    /// </summary>
    private void ItemDrop(){
    }
    /// <summary>
    /// when a stat changes, change the ui to match
    /// </summary>
    private void UpdateCharacterInfo_OnStatsUpdate(System.Object sender, System.EventArgs e){
    }

}
public class StartGameEventArgs: EventArgs{
    public int saveFileNumber{get; set;}
}
public enum Equipment{
    Weapon,
    Armor
}
public class LinkSender : MonoBehaviour
{
    public static void sendToTechno(websites website){
        switch(website){
            case websites.youtube:
                Application.OpenURL("https://www.youtube.com/user/technothepig");
                break;
            case websites.twitter:
                Application.OpenURL("https://twitter.com/Technothepig?ref_src=twsrc%5Egoogle%7Ctwcamp%5Eserp%7Ctwgr%5Eauthor");
                break;
            case websites.twitch:
                Application.OpenURL("https://www.twitch.tv/technoblade");
                break;
        }
    }

    public static void sendToTommy(websites website){
        switch(website){
            case websites.youtube:
                Application.OpenURL("https://www.youtube.com/channel/UC5p_l5ZeB_wGjO_yDXwiqvw");
                break;
            case websites.twitter:
                Application.OpenURL("https://twitter.com/tommyinnit");
                break;
            case websites.twitch:
                Application.OpenURL("https://www.twitch.tv/tommyinnit");
                break;
        }
    }

    public static void sendToWilbur(websites website){
        switch(website){
            case websites.youtube:
                Application.OpenURL("https://www.youtube.com/channel/UC1n_PfsVqxllCcnMPlxBIjw");
                break;
            case websites.twitter:
                Application.OpenURL("https://twitter.com/WilburSoot");
                break;
            case websites.twitch:
                Application.OpenURL("https://www.twitch.tv/wilbursoot");
                break;
        }
    }

}
public enum websites{
    youtube,
    twitter,
    twitch
}
