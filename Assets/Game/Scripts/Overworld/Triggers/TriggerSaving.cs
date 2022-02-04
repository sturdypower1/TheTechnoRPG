using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;
[RequireComponent(typeof(TriggerCutscene))]
public class TriggerSaving : MonoBehaviour
{
    public TriggerCutscene triggerCutscene;

    public int id;
    void Start()
    {
        triggerCutscene = GetComponent<TriggerCutscene>();
        Load();
        SaveAndLoadManager.instance.OnStartSave += Save;
    }

    public void Save(int saveFileNumber)
    {
        string savePath = Application.persistentDataPath + "/tempsave" + "/" + SceneManager.GetActiveScene().name + "/trigger" + id.ToString();
        TriggerSave triggerSave = new TriggerSave { isEnabled = triggerCutscene.isEnabled };

        string jsonString = JsonUtility.ToJson(triggerSave);
        File.WriteAllText(savePath, jsonString);
    }
    public void Load()
    {
        string savePath = Application.persistentDataPath + "/tempsave" + "/" + SceneManager.GetActiveScene().name + "/trigger" + id.ToString();
        if (File.Exists(savePath))
        {
            string jsonString = File.ReadAllText(savePath);
            triggerCutscene.isEnabled = JsonUtility.FromJson<TriggerSave>(jsonString).isEnabled;
        }
    }

    private void OnDestroy()
    {
        SaveAndLoadManager.instance.OnStartSave -= Save;
    }
}
