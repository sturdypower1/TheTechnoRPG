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
    public event EmptyEventHandler OnFinishedDisplaying;
    public event EmptyEventHandler OnCutsceneStarting;
    /// <summary>
    /// invoked when the victory data display has been finished and the player wants to continue
    /// </summary>
    public event EventHandler OnVictoryDisplayFinish;
    /// <summary>
    /// after text is finished, continue the story. Don't skip line 
    /// </summary>
    public TextAsset inkAsset;
    public Story inkStory;
    public static InkManager instance;
    
    public CharacterPortraitReference[] characterPortraits;
    /// <summary>
    /// how fast the text goes
    /// </summary>
    public float textSpeed;

    public Action continueAction;

    [HideInInspector] public bool isCurrentlyDisplaying;
    [HideInInspector] public bool isCurrentlyPlaying;
    [HideInInspector] public CutsceneData currentCutsceneData;

    [HideInInspector] public AudioSource currentDialogueSound;
    [HideInInspector] public bool isContinuable;

    [HideInInspector] public bool isDisplayingChoices;
    private TextboxUI textboxUI;
    
    private bool isScrollingText;

    private string text;
    private bool instant;
    private bool unSkipable;
    private bool isSlow;
    private bool isFinishedPage;
    private int currentChar;
    
    
    private bool isSetup;

    private Queue<PlayerLevelUpData> levelUpDataQueue;
    private Queue<Item> rewardItemsQueue;
    public void tempContinueStory(PlayableDirector playableDirector)
    {
        isCurrentlyPlaying = false;
        ContinueStory();
    }
    public void ResumeStory_OnReturnToOverworld()
    {
        ContinueStory();
    } 
    public void ForceDisable()
    {
        textboxUI.DisableUI();
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

            OnCutsceneStarting?.Invoke();
            ContinueStory();
        }
    }
    
    /// <summary>
    /// used to make all the textbox ui invisible and non interactable
    /// </summary>
    
    /// <summary>
    /// checks if it needs to go to the next piece of dialogue or finish the current one
    /// </summary>
    public void ContinueText(){
        if (isCurrentlyDisplaying)
        {
            if (isFinishedPage)
            {
                //
                //UIManager.instance.ResetFocus();
                ContinueStory();
            }
            else if(!unSkipable){
                isFinishedPage = true;
                //UIManager.instance.textBoxUI.Q<Label>("TextBoxText").text = text;
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

        DisplayPortriat("Technoblade", "default");

        textboxUI.EnableUI();
        textboxUI.DisplayChoices(choices);
        PlayerInputManager.instance.DisableInput();
    }

    public void DisplayNewItem(string ItemName)
    {
        MainGameManager.instance.StopGameworld();

        isDisplayingChoices = false;
        var displayText = "Technoblade obtained <color=red>" + ItemName + "</color>.";
        textboxUI.DisplayText(displayText, false, false, false);
    }

    private CharacterPortraitData GetPortraitList(string characterName, string feeling)
    {
        if(characterName != "")
        {
            if (feeling == "") feeling = "default";

            foreach (CharacterPortraitReference characterPortraitref in characterPortraits)
            {
                if (characterPortraitref.characterName == characterName)
                {
                    textboxUI.SetDialogueAudio(characterPortraitref.audioSource);
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
            inkStory.Continue();
            var isBattleTrigger = inkStory.currentTags.Contains("battle");
            var isCutsceneTrigger = inkStory.currentTags.Contains("playable");

            // sometimes you can click the text box when it's not visible. This helps prevent the story from randomly continuing            
            if (inkStory.currentText == "\n" || inkStory.currentText == "")
            {
                ContinueStory();
            }
            else if (isBattleTrigger)
            {
                PauseManager.instance.Pause();
                textboxUI.DisableUI();
                //start the battle

                EnemyBattlers battle = currentCutsceneData.battles[int.Parse(inkStory.currentText)];
                BattleManager.instance.SetupBattle(battle.Enemies, battle.battleBackground, battle.battleMusic);

            }
            else if (isCutsceneTrigger)
            {
                CameraController.instance.SwitchToStillCamera();
                PauseManager.instance.Pause();
                textboxUI.DisableUI();

                isCurrentlyPlaying = true;
                Debug.Log(inkStory.currentText);
                var cutsceneNumber = int.Parse(inkStory.currentText);
                SetUpCutsceneDirector(cutsceneNumber);
                
            }
            // is textbox trigger
            else
            {
                PauseManager.instance.Pause();

                SetTags();
                textboxUI.DisplayText(inkStory.currentText, instant, unSkipable, isSlow);
                text = inkStory.currentText;
                isCurrentlyDisplaying = true;
            }
        }
        //Todo: possible fix battle transition edge case where this breaks if the battle background is transitioning
        else if (canEnd)
        {
            if (inkStory.currentFlowName == "victory")
            {
                textboxUI.DisableUI();
                inkStory.SwitchToDefaultFlow();
                OnVictoryDisplayFinish?.Invoke(this, EventArgs.Empty);

            }
            else if (inkStory.currentFlowName != "battle")
            {
                isCurrentlyDisplaying = false;
                textboxUI.DisableUI();
                OnFinishedDisplaying?.Invoke();
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
            textboxUI.SetDialogueAudio(null);
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
        textboxUI = GetComponent<TextboxUI>();
        textboxUI.TextboxDoneDisplaying += ContinueStory;
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

    /// <summary>
    /// display the next character portrait
    /// </summary>
    /// <param name="characterName">the name of the character portrait</param>
    /// <param name="feeling">the type of portrait that should be displayed</param>
    private void DisplayPortriat(string characterName, string feeling)
    {

        if (GetPortraitList(characterName, feeling).portraits != null)
        {
            textboxUI.SetCharacterPortrait(GetPortraitList(characterName, feeling));
        }
        else
        {
            Debug.Log("didn't find character portrait");
        }

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
