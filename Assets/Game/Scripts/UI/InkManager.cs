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
    /// <summary>
    /// after text is finished, continue the story. Don't skip line 
    /// </summary>
    //bool isContinuingAfter = false;

    public TextAsset inkAsset;
    public Story inkStory;
    public static InkManager instance;
    public bool isDisplayingChoices;
    public bool isScrollingText;

    public Action continueAction;

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

    private Queue<PlayerLevelUpData> levelUpDataQueue;
    private Queue<Item> rewardItemsQueue;
    /// <summary>
    /// display the victory data of the battle
    /// </summary>
    public void ResumeStory_OnReturnToOverworld()
    {
        ContinueStory();
    } 

    public void DisplayVictoryData(){
        levelUpDataQueue = new Queue<PlayerLevelUpData>(PlayerPartyManager.instance.GetLastLevelUps());
        rewardItemsQueue = new Queue<Item>(BattleManager.instance.GetLastBattleReward().items);

        inkStory.SwitchFlow("victory");
        inkStory.ChoosePathString("victory");
        ContinueStory();
    }
    /// <summary>
    /// start reading the text from the ink story
    /// </summary>
    public void StartCutScene(CutsceneData cutsceneData){
        if (!isCurrentlyDisplaying || isDisplayingChoices)
        {
            DisplayPortriat("empty", "default");
            currentCutsceneData = cutsceneData;
            isDisplayingChoices = false;
            PlayerInputManager.instance.DisableInput();
            UIManager.instance.overworldOverlay.visible = false;

            // ensures that if it was added, it won't be done twice
            if (currentCutsceneData.director == null)
            {
                // set up default
            }
            else
            {
                currentCutsceneData.director.stopped -= tempContinueStory;
                currentCutsceneData.director.stopped += tempContinueStory;
            }
            

            inkStory.ChoosePathString(cutsceneData.inkPath);
            ContinueStory();
        }
    }
    IEnumerator TextCoroutine(Label textBoxText)
    {
        isScrollingText = true;
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
                    if(currentDialogueSound != null)
                    {
                        currentDialogueSound.Play();
                    }
                }
                yield return new WaitForSecondsRealtime(1 / textSpeed);

            }
            else
            {
                break;
            }
        }
        isScrollingText = false;
        isFinishedPage = true;
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
        string displayText = "Technoblade obtained <color=red>" + ItemName + "</color>.";
        isDisplayingChoices = false;
        PlayerInputManager.instance.DisableInput();
        UIManager.instance.overworldOverlay.visible = false;

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
    /// <summary>
    /// called to read the next line of text for the story
    /// </summary>
    private void ContinueStory()
    {
        var canContinue = inkStory.canContinue && !isDisplayingChoices && !isCurrentlyPlaying && !BattleManager.instance.movingToPosition && !isScrollingText;
        var canEnd = !isDisplayingChoices && !isCurrentlyPlaying && !isScrollingText;
        if (canContinue)
        {
            var isBattleTrigger = inkStory.currentTags.Contains("battle");
            var isCutsceneTrigger = inkStory.currentTags.Contains("playable");
            // sometimes you can click the text box when it's not visible. This helps prevent the story from randomly continuing
            inkStory.Continue();
            if (inkStory.currentText == "\n" || inkStory.currentText == "")
            {
                ContinueStory();
            }
            else if (isBattleTrigger)
            {
                PauseManager.instance.Pause();
                DisableTextboxUI();
                //start the battle

                EnemyBattlers battle = currentCutsceneData.battles[int.Parse(inkStory.currentText)];
                BattleManager.instance.SetupBattle(battle.Enemies, battle.battleBackground, battle.battleMusic);

            }
            else if (isCutsceneTrigger)
            {
                CameraController.instance.SwitchToStillCamera();
                PauseManager.instance.Pause();
                DisableTextboxUI();

                isCurrentlyPlaying = true;
                var cutsceneNumber = int.Parse(inkStory.currentText);
                SetUpCutsceneDirector(cutsceneNumber);
                
            }
            else
            {
                PauseManager.instance.Pause();
                text = inkStory.currentText;
                isCurrentlyDisplaying = true;
                SetTags();
               
                UpdateTextBox();
            }
        }
        //Todo: possible fix battle transition edge case where this breaks if the battle background is transitioning
        else if (canEnd)
        {
            if (inkStory.currentFlowName == "victory")
            {
                DisableTextboxUI();
                inkStory.SwitchToDefaultFlow();
                OnVictoryDisplayFinish?.Invoke(this, EventArgs.Empty);

            }
            else if (inkStory.currentFlowName != "battle")
            {
                PauseManager.instance.UnPause();
                CameraController.instance.SwitchToFollowCamera();
                PlayerInputManager.instance.EnableInput();
                isCurrentlyDisplaying = false;

                UIManager.instance.overworldOverlay.visible = true;
                UIManager.instance.isInteractiveEnabled = false;
                ResetTextBox();
                DisableTextboxUI();
            }
        }
    }
    private void SetUpCutsceneDirector(int cutsceneNumber)
    {
        currentCutsceneData.director.playableAsset = currentCutsceneData.playables[cutsceneNumber];
        foreach (var output in currentCutsceneData.director.playableAsset.outputs)
        {
            if (output.streamName.StartsWith("Techno"))
            {
                currentCutsceneData.director.SetGenericBinding(output.sourceObject, Technoblade.instance.gameObject.GetComponent(output.outputTargetType));
            }

        }

        currentCutsceneData.director.Play();
    }
    private void SetTags()
    {
        if (inkStory.currentTags.Contains("soundless"))
        {
            currentDialogueSound = null;
        }
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
    }
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            for (int i = 0; i < characterPortraits.Length; i++)
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
    private void Start()
    {
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
            DisplayNextLevelUp();
        });
        inkStory.BindExternalFunction("updateEXPGold", () => {
            UpdateEXPGold();
        });
        inkStory.BindExternalFunction("GetNextBattleItem", () => {
            DisplayNextBattleItem();
        });
        inkStory.BindExternalFunction("healPlayers", () =>
        {
            PlayerPartyManager.instance.HealPlayers();
        });

    }

    private void DisplayNextLevelUp()
    {
        if (levelUpDataQueue.Count > 0)
        {
            PlayerLevelUpData levelUpRewardData = levelUpDataQueue.Dequeue();

            LevelReward levelReward = levelUpRewardData.levelReward;
            if (levelReward.skill == null)
            {
                inkStory.EvaluateFunction("updateLevelInfo", levelUpRewardData.playerName, "", levelReward.attackBonus, levelReward.defenceBonus, levelReward.pointsBonus, levelReward.healthBonus);
            }
            else
            {
                inkStory.EvaluateFunction("updateLevelInfo", levelUpRewardData.playerName, levelReward.skill.name, levelReward.attackBonus, levelReward.defenceBonus, levelReward.pointsBonus, levelReward.healthBonus);
            }
        }
        else
        {
            // no more level ups
            inkStory.EvaluateFunction("updateLevelInfo", "", "", 0, 0, 0, 0);
        }
    }
    private void UpdateEXPGold()
    {
        // update the EXP and Gold in the ink story
        BattleRewardData battleRewardData = BattleManager.instance.GetLastBattleReward();
        inkStory.EvaluateFunction("updateEXPandGold", battleRewardData.totalEXP, battleRewardData.totalGold);
    }
    /// <summary>
    /// gets next item won from battle
    /// </summary>
    private void DisplayNextBattleItem()
    {
        if (rewardItemsQueue.Count > 0)
        {
            Item item = rewardItemsQueue.Dequeue();
            inkStory.EvaluateFunction("updateItemInfo", item.name);
        }
        else
        {
            inkStory.EvaluateFunction("updateItemInfo", "");
        }
    }
    private void SetDialogueSound(string name)
    {
        // mearly for the fact that it was here previously, want to change it so sound is recorded with the character portrait
    }

    private void UpdateTextBox()
    {
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
