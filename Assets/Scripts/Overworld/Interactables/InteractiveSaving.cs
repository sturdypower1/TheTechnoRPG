using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;
[RequireComponent(typeof(Interactable))]
public class InteractiveSaving : MonoBehaviour
{

    Interactable interactable;
    public int id;
    void Start()
    {
        interactable = GetComponent<Interactable>();
        Load();
        SaveAndLoadManager.instance.OnStartSave += Save;
    }

    public void Save(int saveFileNumber)
    {
        string savePath = Application.persistentDataPath + "/tempsave" + "/" + SceneManager.GetActiveScene().name + " /interactive" + id.ToString();
        TriggerSave triggerSave = new TriggerSave { isEnabled = interactable.IsEnabled };

        string jsonString = JsonUtility.ToJson(triggerSave);
        File.WriteAllText(savePath, jsonString);
    }
    public void Load()
    {
        string savePath = Application.persistentDataPath + "/tempsave" + "/" + SceneManager.GetActiveScene().name + "/interactive" + id.ToString();
        if (File.Exists(savePath))
        {
            string jsonString = File.ReadAllText(savePath);
            interactable.IsEnabled = JsonUtility.FromJson<TriggerSave>(savePath).isEnabled;
        }
    }

    private void OnDestroy()
    {
        SaveAndLoadManager.instance.OnStartSave -= Save;
    }
}
[System.Serializable]
public struct TriggerSave{
    public bool isEnabled;
}
