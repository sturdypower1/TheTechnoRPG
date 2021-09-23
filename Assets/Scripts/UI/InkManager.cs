using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UIElements;
public class InkManager : MonoBehaviour
{
    public TextAsset inkAsset;
    public Story inkStory;
    public static InkManager instance;
    private void Awake() {
        if(instance == null){
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start() {
        //singleton pattern
        
        //set up 
        // see if there is a save file of it first
        inkStory = new Story(inkAsset.text);
        //initiate functions
        inkStory.BindExternalFunction("playSong", (string name) => {
                AudioManager.playSong(name);
            });
            inkStory.BindExternalFunction("playSound", (string name) => {
                AudioManager.playSound(name);
            });
            inkStory.BindExternalFunction("displayPortrait", (string characterName, string feeling) => {
                DisplayPortriat(characterName, feeling);
            });
            inkStory.BindExternalFunction("setTextSound", (string name) => {
                SetDialogueSound(name);
            });
            inkStory.BindExternalFunction("levelUp", () => {
                LevelUp();
            });
            inkStory.BindExternalFunction("updateEXPGold", () => {
                UpdateEXPGold();
            });
            inkStory.BindExternalFunction("GetNextBattleItem", () => {
                GetNextBattleItem();
            });
        
    }
    private void Update() {
        // update textbox text
    }

    public void LevelUp(){
        //level up the character and change the data in the ink story to reflect that
    }
    public void UpdateEXPGold(){
        // update the EXP and Gold in the ink story
    }
    /// <summary>
    /// gets next item won from battle
    /// </summary>
    public void GetNextBattleItem(){
    }
    public void SetDialogueSound(string name){
        // mearly for the fact that it was here previously, want to change it so sound is recorded with the character portrait
    }

    public void UpdateTextBox(){
        // update the text box
    }
    /// <summary>
    /// display the victory data of the battle
    /// </summary>
    public void DisplayVictoryData(){
        
    }
    /// <summary>
    /// start reading the text from the ink story
    /// </summary>
    /// <param name="startPoint">the point in the ink asset to read the text from</param>
    public void StartCutScene(string startPoint){
    }
    /// <summary>
    /// called to read the next line of text for the story
    /// </summary>
    public void ContinueStory(){
    }
    /// <summary>
    /// used to make all the textbox ui invisible and non interactable
    /// </summary>
    public void DisableTextboxUI(){
    }
    /// <summary>
    /// display the next character portrait
    /// </summary>
    /// <param name="characterName">the name of the character portrait</param>
    /// <param name="feeling">the type of portrait that should be displayed</param>
    public void DisplayPortriat(string characterName, string feeling){
    }
    /// <summary>
    /// gets the images for the character portrait and sets the appropriate character dialogue sound
    /// </summary>
    /// <param name="characterName">the name of the character portrait</param>
    /// <param name="feeling">the type of portrait that should be displayed</param>
    /// <returns>the characater portriat</returns>
    public CharacterPortraitData GetPortriatList(string characterName, string feeling){
        return null;
    }
    /// <summary>
    /// checks if it needs to go to the next piece of dialogue or finish the current one
    /// </summary>
    public void ContinueText(){

    }
    /// <summary>
    /// display the current choices the player has
    /// </summary>
    /// <param name="choices">the choices the player has</param>
    public void DisplayChoices(Button[] choices){
    }

}

public class CharacterPortraitData
{
    public string name;
    public Sprite[] portraits;
    
    public Sprite[] eyeAnimations;
    
    public Sprite[] mouthAnimations;
}
