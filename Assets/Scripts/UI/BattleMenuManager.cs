using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UIElements;
using UnityEngine;
/// <summary>
/// controls the main menu during battle
/// </summary>
public class BattleMenuManager : MonoBehaviour
{
    public static BattleMenuManager instance;
    Action cachedHandler;
    public event EventHandler OnContinue;
    VisualElement battleUI;
    VisualElement losingBackground;
    VisualElement enemySelector;
    VisualElement skillSelector;
    VisualElement itemSelector;
    VisualElement previousUI;

    /// <summary>
    /// the ui for the selecting technoblade's attacks, skills, and items
    /// </summary>
    VisualElement technobladeSelectorUI;

    bool hasBattleStarted;
    private int currentPlayer;

    private int currentCharacterSelected;
    private int currentEnemySelected;
    private bool isInPlayerSelection;

    [HideInInspector]
    public int playerNumber;
    [HideInInspector]
    public bool hasMoved;
    
    //need pause manager
    //need save and load manager
    private void Awake() {
        //singleton pattern
        if(instance == null){
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start() {
        // have to set up ui here so that 
        BattleManager.instance.inBattlePositon += EnableMenu_OnTransitionEnd;
        //BattleManager.instance.settingUpBattle += WaitForTransition_OnBattleSetup;
        BattleManager.instance.OnBattleEnd += DisableMenu_OnBattleEnd;
    }

    /// <summary>
    /// activate the battle menu items menu
    /// </summary>
    /// <param name="characterNumber">the character that is using the item</param>
    public void ItemsButton(int characterNumber){
        AudioManager.playSound("menuchange");
        currentCharacterSelected = characterNumber;
        battleUI.visible = false;
        itemSelector.visible = true;

        InventoryManager inventory = InventoryManager.instance;
        int i = 0;
        int firstItemCheck = 0;
        ScrollView list1 = itemSelector.Q<ScrollView>("list1");
        ScrollView list2 = itemSelector.Q<ScrollView>("list2");
        list1.Clear();
        list2.Clear();
        // changing the names of the items
        while (i < inventory.items.Count)
        {
            int z = i;
            Item item = inventory.items[i];
            Button button = new Button();
            button.focusable = true;
            button.text = item.name.ToString();
            button.AddToClassList("item_button");
            button.clicked += () => ItemButton(z);
            button.RegisterCallback<FocusEvent>(ev => UpdateItemDescription(item.description.ToString()));
            button.RegisterCallback<PointerEnterEvent>(ev => UpdateItemDescription(item.description.ToString()));
            // if it isn't usable, don't let the player use the item
            if (item.GetUseability())
            {
                button.SetEnabled(false);
            }
            else if(i == 0)
            {
                button.Focus();
                UpdateItemDescription(item.description.ToString());
            }

            if (i % 2 == 0)
            {
                list1.Add(button);
            }
            else
            {
                list2.Add(button);
            }
            i++;
        }
    }
    /// <summary>
    /// use the currently selected item
    /// </summary>
    /// <param name="itemNumber">the currently selected item</param>
    private void ItemButton(int itemNumber){
        AudioManager.playSound("menuchange");
        GameObject currentCharacter = BattleManager.instance.Players[currentPlayer];
        InventoryManager inventory = InventoryManager.instance;

        Item item = inventory.items[itemNumber];

        // need to change this when multiple players are added
        item.UseItem(Technoblade.instance.gameObject);

        battleUI.visible = true;
        itemSelector.visible = false;
    }
    /// <summary>
    /// activate the battle menu skill tab
    /// </summary>
    /// <param name="characterNumber"> the selected character</param>
    public void SkillsButton(int characterNumber){
        AudioManager.playSound("menuchange");
        currentCharacterSelected = characterNumber;
        skillSelector.visible = true;
        battleUI.visible = false;
        UIManager.instance.ResetFocus();
        List<Button> skillButtons = new List<Button>();
        CharacterStats technoStats = Technoblade.instance.stats;
        int i = 1;
        ScrollView skillList = skillSelector.Q<ScrollView>("skill_list");
        skillList.Clear();
        while (i < technoStats.skills.Count)
        {
            Skill skill = technoStats.skills[i];
            int z = i;
            Button skillButton = new Button();
            skillButton.focusable = true;

            skillButton.text = skill.name.ToString();
            skillButton.clicked += () => SkillButton(z);
            skillButton.RegisterCallback<FocusEvent>(ev => UpdateSkillDescription(skill.description.ToString()));
            skillButton.RegisterCallback<PointerEnterEvent>(ev => UpdateSkillDescription(skill.description.ToString()));


            skillButton.AddToClassList("item_button");
            skillSelector.Q<Label>("skill_desc").text = skill.description.ToString();
            skillList.Add(skillButton);
            skillButtons.Add(skillButton);
            if (i == 1)
            {
                UpdateSkillDescription(skill.description.ToString());
                skillButton.Focus();
            }
            i++;
        }
    }
    /// <summary>
    /// open the enemy selector for the currently selected skill
    /// </summary>
    /// <param name="skillNumber">the currently selected skill</param>
    public void SkillButton(int skillNumber){
        CharacterStats technoStats = Technoblade.instance.stats;
        //checking if there have enough points to use the move
        if (technoStats.stats.points >= technoStats.skills[skillNumber].cost)
        {
            AudioManager.playSound("menuchange");
            int i = 0;
            previousUI = skillSelector;
            skillSelector.visible = false;
            enemySelector.visible = true;
            
            foreach(GameObject enemy in BattleManager.instance.Enemies)
            {
                int z = i;

                VisualElement enemySelectorUI = BattleManager.instance.enemySelectorUI[i].ui;
                Button button = enemySelectorUI.Q<Button>("Base");
                int tempNumber = skillNumber;
                cachedHandler = () => SkillEnemySelectButton(z, tempNumber, button);
                button.clicked += cachedHandler;
                i++;
            }
        }
    }
    /// <summary>
    /// attacks the selected enemy with the selected skill
    /// </summary>
    /// <param name="enemyNumber">the selected enemy</param>
    /// <param name="currentSkill">the selected skill</param>
    /// <param name="selectButton">the button that is pressed</param>
    public void SkillEnemySelectButton(int enemyNumber, int currentSkill, Button selectButton){
        // doesn't do anything if the enemy isn't selected yet
        selectButton.clicked -= cachedHandler;
        cachedHandler = null;
        AudioManager.playSound("menuselect");
        battleUI.Focus();

        technobladeSelectorUI.SetEnabled(false);

        CharacterStats technoStats = Technoblade.instance.stats;

        Skill skill = technoStats.skills[currentSkill];
        if (technoStats.stats.points >= skill.cost)
        {
            technoStats.stats.points -= skill.cost;
        }
        else
        {
            technoStats.stats.points = 0;
        }
            //deal damage to the enemy
            skill.UseSkill( BattleManager.instance.Enemies[currentEnemySelected], technoStats.gameObject);

            battleUI.visible = true;
            enemySelector.visible = false;
    }
    /// <summary>
    /// go back to the previous battle menu tab
    /// </summary>
    public void EnemySelectBack(){
        AudioManager.playSound("menuback");
        enemySelector.visible = false;
        previousUI.visible = true;
    }
    /// <summary>
    /// update the description of the skill
    /// </summary>
    /// <param name="description">the updated description</param>
    private void UpdateSkillDescription(string description){
        skillSelector.Q<Label>("skill_desc").text = description;
    }
    /// <summary>
    /// update the description of the item
    /// </summary>
    /// <param name="description"></param>
    private void UpdateItemDescription(string description){
        itemSelector.Q<Label>("item_desc").text = description;
    }
    /// <summary>
    /// go from the item's menu back to the regular battle menu
    /// </summary>
    public void ItemsBackButton(){
        AudioManager.playSound("menuback");
        itemSelector.visible = false;
        battleUI.visible = true;
    }
    /// <summary>
    /// go from the skills menu back to the regular battle menu
    /// </summary>
    public void SkillsBackButton(){
        AudioManager.playSound("menuback");
        skillSelector.visible = false;
        battleUI.visible = true;
    }
    /// <summary>
    /// use the first skill in the list of the character's skills to target an enemy
    /// </summary>
    /// <param name="characterNumber">the character </param>
    public void AttackButton(int characterNumber){
        AudioManager.playSound("menuchange");
        previousUI = battleUI;
        battleUI.visible = false;
        enemySelector.visible = true;
        currentCharacterSelected = characterNumber;

        int i = 0;

        foreach(GameObject enemy in BattleManager.instance.Enemies)
        {
            int z = i;
            VisualElement enemySelectorUI = BattleManager.instance.enemySelectorUI[currentEnemySelected].ui;
            Button button = enemySelectorUI.Q<Button>("Base");
            cachedHandler = () => AttackEnemySelectButton(z, button);
            button.clicked += cachedHandler;
            i++;
        }
    }
    /// <summary>
    /// use first skill to attack an enemy
    /// </summary>
    /// <param name="EnemyNumber">the selected enemy</param>
    /// <param name="selectButton">the button used</param>
    private void AttackEnemySelectButton(int EnemyNumber, Button selectButton){
        UIManager.instance.ResetFocus();
        CharacterStats technoStats = Technoblade.instance.stats;
        
        Skill skill = technoStats.skills[0];
        battleUI.visible = true;
        technobladeSelectorUI.SetEnabled(false);
        //makes sure that nothing is selected
        foreach(EnemySelectorUI enemySelectorUI in BattleManager.instance.enemySelectorUI)
        {
            enemySelectorUI.UnSelectUI();
        }
        // the attack button will always use the first skill
        skill.UseSkill(BattleManager.instance.Enemies[EnemyNumber], technoStats.gameObject);

        battleUI.visible = true;
        enemySelector.visible = false;
            
        selectButton.clicked -= cachedHandler;
        cachedHandler = null;
    }

    /// <summary>
    /// when the battle transition is over, resume the gameworld(unless in a cutscene)
    /// </summary>
    private void ResumeGameWorld_OnTransitionEnd(System.Object sender, System.EventArgs e){
        battleUI.visible = false;
        enemySelector.visible = false;
        skillSelector.visible = false;
        itemSelector.visible = false;
        hasBattleStarted = false;

        // unpause the game world

        InkManager.instance.ContinueStory();


    }
    /// <summary>
    /// disable all the menus that were in the battle
    /// </summary>
    private void DisableMenu_OnBattleEnd(OnBattleEndEventArgs e){
        battleUI.visible = false;
        enemySelector.visible = false;
        skillSelector.visible = false;
        itemSelector.visible = false;
        hasBattleStarted = false;

        technobladeSelectorUI.SetEnabled(true);
        if (e.isPlayerVictor)
        {
           //get ready to transition all the game objects
        }
    }
    /// <summary>
    /// after the first battle transition is over, set up the battle menus
    /// </summary>
    private void EnableMenu_OnTransitionEnd(){
            // enables all the features of the menu
            Camera cam = FindObjectOfType<Camera>();
            float positionRatio = 1280.0f / cam.pixelWidth;
        Debug.Log(positionRatio);

            VisualElement root = UIManager.instance.root;
            battleUI = root.Q<VisualElement>("BattleUI");
            enemySelector = root.Q<VisualElement>("EnemySelector");
            skillSelector = root.Q<VisualElement>("skill_selector");
            itemSelector = root.Q<VisualElement>("item_selector");
            losingBackground = root.Q<VisualElement>("losing_screen");

        technobladeSelectorUI = battleUI.Q<VisualElement>("character1");
        Technoblade.instance.technoSelectorUI = technobladeSelectorUI;

        battleUI.visible = true;
        battleUI.Focus();
            // adding the selectorUI stuff to the players
            int i = 0;
            foreach (GameObject player in BattleManager.instance.Players)
            {
                HeadsUpUI headsUpUI = player.GetComponent<Battler>().headsUpUI;
                TemplateContainer headsUpDisplay = headsUpUI.ui;

            // setting the position of the headsUpDisplay
            Vector3 camPo = cam.WorldToScreenPoint(headsUpUI.transform.position);
            Vector2 uiPosition = new Vector2(camPo.x * positionRatio, camPo.y * positionRatio);

            headsUpUI.ui.Q<VisualElement>("base").style.bottom = uiPosition.y;
            headsUpUI.ui.Q<VisualElement>("base").style.left = uiPosition.x;
            i++;
            }
            i = 0;
            foreach (GameObject enemy in BattleManager.instance.Enemies)
            {
                int z = i;

                HeadsUpUI headsUpUI = enemy.GetComponent<Battler>().headsUpUI;
                TemplateContainer headsUpDisplay = headsUpUI.ui;

            // setting the position of the headsUpDisplay

            Vector3 camPo = cam.WorldToScreenPoint(headsUpUI.transform.position);
            Vector2 uiPosition =  new Vector2(camPo.x * positionRatio, camPo.y * positionRatio);

            headsUpUI.ui.Q<VisualElement>("base").style.bottom = uiPosition.y;
            headsUpUI.ui.Q<VisualElement>("base").style.left = uiPosition.x;
        }
    }

    /// <summary>
    /// once the victory data is finished being displayed, start trasition back to the overworld
    /// </summary>
    private void FinishVictoryData_OnDisplayFinished(object sender, System.EventArgs e){

    }
    /// <summary>
    /// once the battle ends, start displaying the victory data
    /// </summary>
    private void StartVictoryData_OnBattleEnd(OnBattleEndEventArgs e){

    }
    /// <summary>
    /// after dying load the last save
    /// </summary>
    public void ContinueButton(){
    }
    /// <summary>
    /// go back to the title after losing
    /// </summary>
    public void LossReturnToTitleButton(){
    }
    /// <summary>
    /// after dying, unload the scene
    /// </summary>
    private void UnLoadScenes_OnTransitionnEnd(object sender, System.EventArgs e){
    }
    /// <summary>
    /// when the player loses, start process for the loss screen
    /// </summary>
    private void DisplayLoss_OnPlayerLoss(object sender, OnBattleEndEventArgs e){
    }
}
public class OnBattleEndEventArgs : EventArgs{
      public bool isPlayerVictor {get; set;}
}
