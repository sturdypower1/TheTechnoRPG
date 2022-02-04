using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
[RequireComponent(typeof(ChestInteractive))]
public class ChestSave : ISaveable
{
    ChestInteractive chest;
    public Animator animator;

    public override void Start()
    {
        chest = GetComponent<ChestInteractive>();
        animator = GetComponent<Animator>();
        base.Start();
    }
    public override void Load()
    {
        string savePath = Application.persistentDataPath + "/tempsave" + "/" + SceneManager.GetActiveScene().name + "/chest" + id.ToString();
        if (File.Exists(savePath))
        {
            string jsonString = File.ReadAllText(savePath);
            ChestSaveData save = JsonUtility.FromJson<ChestSaveData>(jsonString);
            animator.Play(save.animationSave.name, 0, save.animationSave.normilizedtime);
            chest.IsEnabled = save.IsEnabled;
        }
        
    }

    public override void Save(int saveFileNumber)
    {
        string savePath = Application.persistentDataPath + "/tempsave" + "/" + SceneManager.GetActiveScene().name + "/chest" + id.ToString();
        AnimationSaveData animationSave = new AnimationSaveData {
            name = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name,
            normilizedtime = animator.GetCurrentAnimatorStateInfo(0).normalizedTime
        };
        ChestSaveData save = new ChestSaveData { IsEnabled = chest.IsEnabled, animationSave = animationSave };

        string jsonString = JsonUtility.ToJson(save);
        File.WriteAllText(savePath, jsonString);
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}
[System.Serializable]
public struct ChestSaveData
{
    public AnimationSaveData animationSave;
    public bool IsEnabled;
}
