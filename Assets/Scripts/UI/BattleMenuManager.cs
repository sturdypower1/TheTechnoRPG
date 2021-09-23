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

    bool hasBattleStarted;
    private int currentPlayer;

    private int currentCharacterSelected;
    private int currentEnemySelected;
    private bool isInPlayerSelection;

    [HideInInspector]
    public int playerNumber;
    [HideInInspector]
    public bool hasMoved;
    public VisualTreeAsset enemySelectionUITemplate;
    public VisualTreeAsset overHeadUITemplate;
    //need pause manager
    //need save and load manager

    private void Start() {
        //singleton pattern
        if(instance == null){
            instance = this;
            // set up the rest of the ui
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// activate the battle menu items menu
    /// </summary>
    /// <param name="characterNumber">the character that is using the item</param>
    public void ItemsButton(int characterNumber){
    }
    /// <summary>
    /// use the currently selected item
    /// </summary>
    /// <param name="itemNumber">the currently selected item</param>
    private void ItemButton(int itemNumber){
    }
    /// <summary>
    /// activate the battle menu skill tab
    /// </summary>
    /// <param name="characterNumber"> the selected character</param>
    public void SkillsButton(int characterNumber){
    }
    /// <summary>
    /// open the enemy selector for the currently selected skill
    /// </summary>
    /// <param name="skillNumber">the currently selected skill</param>
    public void SkillButton(int skillNumber){

    }
    /// <summary>
    /// attacks the selected enemy with the selected skill
    /// </summary>
    /// <param name="enemyNumber">the selected enemy</param>
    /// <param name="currentSkill">the selected skill</param>
    /// <param name="selectButton">the button that is pressed</param>
    public void SkillEnemySelectButton(int enemyNumber, int currentSkill, Button selectButton){
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

    }
    /// <summary>
    /// use first skill to attack an enemy
    /// </summary>
    /// <param name="EnemyNumber">the selected enemy</param>
    /// <param name="selectButton">the button used</param>
    private void AttackEnemySelectButton(int EnemyNumber, Button selectButton){
    }
    /// <summary>
    /// when the battle transition is over, resume the gameworld(unless in a cutscene)
    /// </summary>
    private void ResumeGameWorld_OnTransitionEnd(System.Object sender, System.EventArgs e){
    }
    /// <summary>
    /// disable all the menus that were in the battle
    /// </summary>
    private void DisableMenu_OnBattleEnd(System.Object sender, OnBattleEndEventArgs e){
    }
    /// <summary>
    /// after the first battle transition is over, set up the battle menus
    /// </summary>
    private void EnableMenu_OnTransitionEnd(System.Object sender, System.EventArgs e){
    }
    /// <summary>
    /// set up the transition of the battle menu
    /// </summary>
    private void WaitForTransition_OnBattleSetup(System.Object sender, System.EventArgs e){
    }
    /// <summary>
    /// once the victory data is finished being displayed, start trasition back to the overworld
    /// </summary>
    private void FinishVictoryData_OnDisplayFinished(object sender, System.EventArgs e){

    }
    /// <summary>
    /// once the battle ends, start displaying the victory data
    /// </summary>
    private void StartVictoryData_OnBattleEnd(object sender, OnBattleEndEventArgs e){
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
