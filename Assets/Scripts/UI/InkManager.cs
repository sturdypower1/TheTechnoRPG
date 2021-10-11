using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using UnityEngine.UIElements;
using Cinemachine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
public class InkManager : MonoBehaviour
{
    /// <summary>
    /// invoked when the victory data display has been finished and the player wants to continue
    /// </summary>
    public event EventHandler OnVictoryDisplayFinish;

    public void tempContinueStory(PlayableDirector playableDirector){
        isCurrentlyPlaying = false;
        ContinueStory();
    }

    public TextAsset inkAsset;
    public Story inkStory;
    public static InkManager instance;
    public bool isDisplayingChoices;

    public CharacterPortraitReference[] characterPortraits;

    CinemachineVirtualCamera overoworldCinemachine;

    CinemachineVirtualCamera frozenCinemachine;

    /// <summary>
    /// how fast the text goes
    /// </summary>
    public float textSpeed;

    public string text;
    public bool instant;
    public bool unSkipable;
    public bool isSlow;
    public bool isFinishedPage;
    public int currentChar;
    
    public bool isContinuable;
    public bool isCurrentlyDisplaying;
    public bool isCurrentlyPlaying;
    [HideInInspector]
    public CutsceneData currentCutsceneData;

    [HideInInspector]
    public AudioSource currentDialogueSound;

    public bool isSetup;
    private void Awake() {
        if(instance == null){
            instance = this;

            for(int i = 0; i < characterPortraits.Length; i++)
            {
                characterPortraits[i].audioSource = gameObject.AddComponent<AudioSource>();
                characterPortraits[i].audioSource.clip = characterPortraits[i].audioClip;
            }
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

    public void LevelUp(){
        //level up the character and change the data in the ink story to reflect that
        LevelUpController levelData = Technoblade.instance.gameObject.GetComponent<LevelUpController>();

        LevelUpRewardData levelUpRewardData = levelData.LevelUpRewardData;

        if (levelData.currentEXP >= ((levelData.currentLVL * 20) + ((levelData.currentLVL - 1) * 10)))
        {

            // leveled up
            levelData.currentEXP -= ((levelData.currentLVL * 20) + ((levelData.currentLVL - 1) * 10));
            levelData.currentLVL += 1;
            levelData.requiredEXP = ((levelData.currentLVL * 20) + ((levelData.currentLVL - 1) * 10));


            LevelReward levelReward = levelUpRewardData.levelRewards[levelData.currentLVL - levelUpRewardData.startingLevel - 1];
            CharacterStats characterStats = Technoblade.instance.gameObject.GetComponent<CharacterStats>();
            characterStats.stats.attack += levelReward.attackBonus;
            characterStats.stats.defence += levelReward.defenceBonus;
            characterStats.stats.maxPoints += levelReward.pointsBonus;
            characterStats.stats.maxHealth += levelReward.healthBonus;

            if (levelReward.skill == null)
            {
                inkStory.EvaluateFunction("updateLevelInfo", "Technoblade", "", levelReward.attackBonus, levelReward.defenceBonus, levelReward.pointsBonus, levelReward.healthBonus);
            }
            else
            {
                characterStats.skills.Insert(1, levelReward.skill);
                inkStory.EvaluateFunction("updateLevelInfo", "Technoblade", levelReward.skill.name, levelReward.attackBonus, levelReward.defenceBonus, levelReward.pointsBonus, levelReward.healthBonus);
            }


        }
        else
        {
            // no more level ups
            inkStory.EvaluateFunction("updateLevelInfo", "", "", 0, 0, 0, 0);
        }
    }
    public void UpdateEXPGold(){
        // update the EXP and Gold in the ink story

        inkStory.EvaluateFunction("updateEXPandGold", BattleManager.instance.battleRewardData.totalEXP, BattleManager.instance.battleRewardData.totalGold);
    }
    /// <summary>
    /// gets next item won from battle
    /// </summary>
    public void GetNextBattleItem(){
        BattleRewardData battleRewardData = BattleManager.instance.battleRewardData;

        if (battleRewardData.items.Count > 0)
        {
            inkStory.EvaluateFunction("updateItemInfo", battleRewardData.items[0].name);

            InventoryManager.instance.items.Add(battleRewardData.items[0]);
            battleRewardData.items.RemoveAt(0);
        }
        else
        {
            inkStory.EvaluateFunction("updateItemInfo", "");
        }
    }
    public void SetDialogueSound(string name){
        // mearly for the fact that it was here previously, want to change it so sound is recorded with the character portrait
    }

    public void UpdateTextBox(){
        // update the text box

        //TODO: add pause
        Button textBoxUI = UIManager.instance.textBoxUI;
        Label textBoxText = textBoxUI.Q<Label>("TextBoxText");
        if (!isSetup)
        {
            
            textBoxUI.visible = true;
            textBoxUI.Focus();

            VisualElement playerChoiceUI = textBoxUI.Q<VisualElement>("player_choices");
            playerChoiceUI.Clear();
        }
        if (instant)
        {
            textBoxText.text = text;
            isFinishedPage = true;
        }
        else
        {
            textBoxText.text = "";
            isFinishedPage = false;
            StartCoroutine(TextCoroutine(textBoxText));
        }

    }
    /// <summary>
    /// display the victory data of the battle
    /// </summary>
    public void DisplayVictoryData(){
        inkStory.SwitchFlow("victory");
        inkStory.ChoosePathString("victory");
        ContinueStory();
    }
    /// <summary>
    /// start reading the text from the ink story
    /// </summary>
    /// <param name="startPoint">the point in the ink asset to read the text from</param>
    public void StartCutScene(CutsceneData cutsceneData){
        if (!isCurrentlyDisplaying)
        {
            DisplayPortriat("empty", "default");
            currentCutsceneData = cutsceneData;
            isDisplayingChoices = false;
            PlayerInputManager.instance.DisableInput();
            UIManager.instance.overworldOverlay.visible = false;

            // ensures that if it was added, it won't be done twice
            currentCutsceneData.director.stopped -= tempContinueStory;
            currentCutsceneData.director.stopped += tempContinueStory;

            inkStory.ChoosePathString(cutsceneData.inkPath);
            ContinueStory();
        }
    }
    IEnumerator TextCoroutine(Label textBoxText)
    {
        bool waitingForSymbol = false;
        foreach(Char letter in text)
        {
            if (!isFinishedPage)
            {
                textBoxText.text += letter;
                //will automatically display rich text
                if (letter == '<' || waitingForSymbol)
                {
                    waitingForSymbol = true;
                    if(letter == '>')
                    {
                        waitingForSymbol = false;
                    }
                    continue;
                }
                // add stuff for character animations
                if(letter != ' ')
                {
                    currentDialogueSound.Play();
                }
                yield return new WaitForSecondsRealtime(1 / textSpeed);

            }
            else
            {
                break;
            }
        }
        isFinishedPage = true;
    }
    /// <summary>
    /// called to read the next line of text for the story
    /// </summary>
    public void ContinueStory(){
        if(inkStory.canContinue && !isDisplayingChoices && !isCurrentlyPlaying)
        {
            // sometimes you can click the text box when it's not visible. This helps prevent the story from randomly continuing
            inkStory.Continue();
            if(inkStory.currentText == "\n" || inkStory.currentText == "")
            {
                ContinueStory();
            }
            else if (inkStory.currentTags.Contains("battle"))
            {
                PauseManager.instance.Pause();
                //start the battle
            }
            else if (inkStory.currentTags.Contains("playable"))
            {
                PauseManager.instance.Pause();
                DisableTextboxUI();

                isCurrentlyPlaying = true ;

                currentCutsceneData.director.playableAsset = currentCutsceneData.playables[0];
                currentCutsceneData.director.Play();
            }
            else
            {
                PauseManager.instance.Pause();
                text = inkStory.currentText;
                isCurrentlyDisplaying = true;
                if (inkStory.currentTags.Contains("unskipable"))
                {
                    unSkipable = true;
                }
                else
                {
                    unSkipable = false;
                }
                if (inkStory.currentTags.Contains("instant"))
                {
                    isSlow = true;
                }
                else
                {
                    isSlow = false;
                }
                if (inkStory.currentTags.Contains("instant"))
                {
                    instant = true;
                }
                else
                {
                    instant = false;
                }
                UpdateTextBox();
            
            }
        }
        else if (!isDisplayingChoices && !isCurrentlyPlaying)
        {
            PlayerInputManager.instance.EnableInput();
            if(inkStory.currentFlowName == "victory")
            {
                inkStory.SwitchToDefaultFlow();
                OnVictoryDisplayFinish?.Invoke(this, EventArgs.Empty);
                
            }
            else if(inkStory.currentFlowName != "battle")
            {
                PauseManager.instance.UnPause();

                CameraController.instance.ToOverworldCamera();
                // is completely finished
                PlayerInputManager.instance.EnableInput();
                isCurrentlyDisplaying = false;
                  

                UIManager.instance.overworldOverlay.visible = true;
                UIManager.instance.isInteractiveEnabled = false;
                ResetTextBox();
                DisableTextboxUI();
            }
        }
    }
    /// <summary>
    /// reset all the stuff about the text box
    /// </summary>
    public void ResetTextBox()
    {

    }
    /// <summary>
    /// used to make all the textbox ui invisible and non interactable
    /// </summary>
    public void DisableTextboxUI(){
        // unpause
        UIManager.instance.ResetFocus();
        UIManager.instance.textBoxUI.visible = false;

        //isCurrentlyDisplaying = false;
        isSetup = false;
    }
    /// <summary>
    /// display the next character portrait
    /// </summary>
    /// <param name="characterName">the name of the character portrait</param>
    /// <param name="feeling">the type of portrait that should be displayed</param>
    public void DisplayPortriat(string characterName, string feeling)
    {

        if (GetPortraitList(characterName, feeling).portraits != null)
        {
            //CharacterPortraitReference portraitReference = GetPortraitList(characterName, feeling);
            UIManager.instance.textBoxUI.Q<VisualElement>("character_base").style.backgroundImage = Background.FromSprite(GetPortraitList(characterName, feeling).portraits[0]);
        }
        else
        {
            Debug.Log("didn't find character portrait");
        }
        
    }
    /// <summary>
    /// gets the images for the character portrait and sets the appropriate character dialogue sound
    /// </summary>
    /// <param name="characterName">the name of the character portrait</param>
    /// <param name="feeling">the type of portrait that should be displayed</param>
    /// <returns>the characater portriat</returns>
    /// <summary>
    /// checks if it needs to go to the next piece of dialogue or finish the current one
    /// </summary>
    public void ContinueText(){
        if (isCurrentlyDisplaying)
        {
            if (isFinishedPage)
            {
                UIManager.instance.ResetFocus();
                ContinueStory();
            }
            else if(!unSkipable){
                isFinishedPage = true;
                UIManager.instance.textBoxUI.Q<Label>("TextBoxText").text = text;
            }
        }
    }
    /// <summary>
    /// display the current choices the player has
    /// </summary>
    /// <param name="choices">the choices the player has</param>
    public void DisplayChoices(Button[] choices){
        isDisplayingChoices = true;
        isCurrentlyDisplaying = true;
        Label textBoxText = UIManager.instance.textBoxUI.Q<Label>("TextBoxText");
        textBoxText.text = "";

        // updating the portrait
        DisplayPortriat("Technoblade", "default");
  
        Button textBoxUI = UIManager.instance.textBoxUI;
        textBoxUI.visible = true;
        PlayerInputManager.instance.DisableInput();
        VisualElement playerChoiceUI = textBoxUI.Q<VisualElement>("player_choices");
        playerChoiceUI.Clear();
        foreach (Button choice in choices)
        {
            playerChoiceUI.Add(choice);
            choice.focusable = true;
            choice.Focus();
            
        }
    }

    public void DisplayNewItem(string ItemName)
    {
        PauseManager.instance.Pause();
        Button textBoxUI = UIManager.instance.textBoxUI;
        Label textBoxText = textBoxUI.Q<Label>("TextBoxText");
        string displayText = "Technoblade obtained <color=green>" + ItemName + "</color>.";

        isCurrentlyDisplaying = true;
        instant = false;
        unSkipable = false;
        isSlow = false;
        text = displayText;
        DisplayPortriat("empty", "default");
        UpdateTextBox();
        //StartCoroutine(TextCoroutine(textBoxText));
    }
    public CharacterPortraitData GetPortraitList(string characterName, string feeling)
    {
        if(characterName != "")
        {
            if (feeling == "") feeling = "default";

            foreach (CharacterPortraitReference characterPortraitref in characterPortraits)
            {
                if (characterPortraitref.characterName == characterName)
                {
                    currentDialogueSound = characterPortraitref.audioSource;
                    foreach (CharacterPortraitData characterPortraitData in characterPortraitref.characterPortraits)
                    {
                        if (characterPortraitData.name == feeling)
                        {
                            return characterPortraitData;
                        }
                    }

                }
            }
            Debug.Log("feeling not found");
            if (feeling != "default")
            {
                return GetPortraitList(characterName, "default");
            }
            Debug.Log("defualt not found");
        }
        else
        {

        }

        return new CharacterPortraitData();
    }
    

}

[System.Serializable]
public struct CharacterPortraitReference
{
    public string characterName;
    public AudioClip audioClip;
    [HideInInspector]
    public AudioSource audioSource;
    public List<CharacterPortraitData> characterPortraits;
}
[System.Serializable]
public struct CharacterPortraitData
{
    public string name;
    public Sprite[] portraits;
    
}
