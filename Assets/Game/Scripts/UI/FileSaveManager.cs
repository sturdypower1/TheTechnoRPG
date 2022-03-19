using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;

public class FileSaveManager : MonoBehaviour
{
    public static FileSaveManager instance;

    public event EmptyEventHandler OnBackPressed;
    public event SaveEventHandler OnStartSave;
    public SaveAndLoadUI saveAndLoadUI;

    private float gameTime = 0;
    private string SavePointName;
    public void Activate(string savePointName)
    {
        SavePointName = savePointName;
        saveAndLoadUI.Enable();
    }
    public void Deactivate()
    {
        saveAndLoadUI.Disable();
    }
    private void BackPressed()
    {
        OnBackPressed?.Invoke();
    }
    private void Save_OnSaveFilePressed(int saveFileNumber)
    {
        SaveGame(saveFileNumber);
    }
    public void SaveGame(int saveFileNubmer)
    {
        CreateSaveFiles();
        OnStartSave?.Invoke(saveFileNubmer);
        //SaveAtmosphere();
        SaveCurrentScene();
        //saving the save name and game time
        string savePointPath = Application.persistentDataPath + "/tempsave" + "/SavePointData";
        SavePointData savePointData = new SavePointData { savePointName = SavePointName, timePassed = gameTime };
        string jsonString = JsonUtility.ToJson(savePointData);
        File.WriteAllText(savePointPath, jsonString);

        // gets everything in the temp folder and pastes it into the save
        string savePath = Application.persistentDataPath + "/save" + saveFileNubmer.ToString();
        foreach (string file in Directory.GetFiles(savePath))
        {
            File.Delete(file);
        }
        foreach (string directory in Directory.GetDirectories(savePath))
        {
            foreach (string file in Directory.GetFiles(directory))
            {
                File.Delete(file);
            }
        }
        foreach (string file in Directory.GetFiles(Application.persistentDataPath + "/tempsave"))
        {
            string fileSavePath = Path.Combine(savePath, Path.GetFileName(file));
            File.Copy(file, fileSavePath);
        }
        foreach (string directory in Directory.GetDirectories(Application.persistentDataPath + "/tempsave"))
        {
            string subDirectory = directory.Substring((Application.persistentDataPath + "/tempsave").Length);
            if (!Directory.Exists(savePath + subDirectory)) Directory.CreateDirectory(savePath + subDirectory);
            foreach (string file in Directory.GetFiles(directory))
            {
                string fileSavePath = Path.Combine(savePath + subDirectory, Path.GetFileName(file));
                File.Copy(file, fileSavePath);
            }
        }
        
        saveAndLoadUI.UpdateSaveFileUI();
    }
    private void SaveCurrentScene()
    {
        string savePath = Application.persistentDataPath + "/tempsave" + "/loadedscenes";
        string jsonString = JsonUtility.ToJson(new SceneSave { sceneName = SceneManager.GetActiveScene().name });
        File.WriteAllText(savePath, jsonString);
    }
    private void CreateSaveFiles()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/tempsave")) Directory.CreateDirectory(Application.persistentDataPath + "/tempsave");
        if (!Directory.Exists(Application.persistentDataPath + "/save1")) Directory.CreateDirectory(Application.persistentDataPath + "/save1");
        if (!Directory.Exists(Application.persistentDataPath + "/save2")) Directory.CreateDirectory(Application.persistentDataPath + "/save2");
        if (!Directory.Exists(Application.persistentDataPath + "/tempsave" + "/" + SceneManager.GetActiveScene().name)) Directory.CreateDirectory(Application.persistentDataPath + "/tempsave" + "/" + SceneManager.GetActiveScene().name);
    }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        saveAndLoadUI.OnBackPressed += BackPressed;
        saveAndLoadUI.OnSaveFilePressed += Save_OnSaveFilePressed;
        string savePointPath = Application.persistentDataPath + "/tempsave" + "/SavePointData";
        if (File.Exists(savePointPath))
        {
            string jsonString = File.ReadAllText(savePointPath);
            SavePointData savePointData = JsonUtility.FromJson<SavePointData>(jsonString);
            gameTime = savePointData.timePassed;
        }
    }
    void Update()
    {
        gameTime += Time.unscaledDeltaTime;
    }
    
   
    
    
}
