using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System.IO;

public class STEVE : PlayerController
{
    public static STEVE instance;
    public bool isInTeam = true;
    
    // Start is called before the first frame update
    private void Awake()
    {
        if(instance == null)
        {
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
        //SaveAndLoadManager.instance.OnStartSave += Save;
        //SaveAndLoadManager.instance.OnReLoadSave += Load;

        levelUpController = this.gameObject.GetComponent<LevelUpController>();
        animator = this.gameObject.GetComponent<Animator>();
        battler = this.gameObject.GetComponent<Battler>();
        stats = this.gameObject.GetComponent<CharacterStats>();
        //Load();
        if (isInTeam)
        {
            PlayerPartyManager.instance.AddPlayer("Steve");
        }
    }

    public void DestroySteve()
    {
        Destroy(this.gameObject);
    }

    public void Save(int saveFileNumber)
    {
        

        string savePath = Application.persistentDataPath + "/tempsave" + "/STEVE";
        AnimationSaveData animationSaveData = new AnimationSaveData
        {
            name = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name,
            normilizedtime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime
        };

        List<string> skillNames = new List<string>();
        foreach (Skill skill in stats.skills)
        {
            skillNames.Add(skill.name);
        }
        SteveSaveData saveData = new SteveSaveData
        {
            characterStats = new CharacterStatsSaveData
            {
                stats = stats.stats,
                armorName = stats.equipedArmor.name,
                weaponName = stats.equipedWeapon.name,
                skillNames = skillNames
            },
            animationSave = animationSaveData,
            transform = transform.position,
            levelUpSave = new LevelUpSave { currentEXP = levelUpController.currentEXP, currentLVL = levelUpController.currentLVL, LevelCap = levelUpController.LevelCAP, requiredEXP = levelUpController.requiredEXP },
            isInTeam = isInTeam
        };


        string jsonString = JsonUtility.ToJson(saveData);
        File.WriteAllText(savePath, jsonString);
    }
    public void Load()
    {
        string savePath = Application.persistentDataPath + "/tempsave" + "/STEVE";
        if (File.Exists(savePath))
        {
            Collider2D collider = GetComponent<Collider2D>();
            collider.enabled = true;

            string jsonString = File.ReadAllText(savePath);
            SteveSaveData saveData = JsonUtility.FromJson<SteveSaveData>(jsonString);
            stats.stats = saveData.characterStats.stats;

            stats.skills.Clear();
            foreach (string skillName in saveData.characterStats.skillNames)
            {
                stats.skills.Add(Resources.Load(skillName) as Skill);
            }

            stats.equipedWeapon = Resources.Load(saveData.characterStats.weaponName) as Weapon;
            stats.equipedArmor = Resources.Load(saveData.characterStats.armorName) as Armor;

            animator.Play(saveData.animationSave.name, 0, saveData.animationSave.normilizedtime);
            transform.position = saveData.transform;

            levelUpController.LevelCAP = saveData.levelUpSave.LevelCap;
            levelUpController.currentLVL = saveData.levelUpSave.currentLVL;
            levelUpController.requiredEXP = saveData.levelUpSave.requiredEXP;
            levelUpController.currentEXP = saveData.levelUpSave.currentEXP;

            isInTeam = saveData.isInTeam;
            if (isInTeam)
            {
                PlayerPartyManager.instance.AddPlayer("Steve");
            }
        }
    }
    private void OnDestroy()
    {
        PlayerPartyManager.instance.RemovePlayer("Technoblade");
        FileSaveManager.instance.OnStartSave -= Save;
        //UIManager.instance.OnTitleReturn -= DestroySteve;
        SaveAndLoadManager.instance.OnReLoadSave -= Load;
    }
}

public struct SteveSaveData
{
    public bool isInTeam;
    public CharacterStatsSaveData characterStats;
    public AnimationSaveData animationSave;
    public Vector2 transform;
    public LevelUpSave levelUpSave;
}
