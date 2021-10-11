using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.IO;

public class SaveAndLoadManager : MonoBehaviour
{
    public static SaveAndLoadManager instance;
    // Start is called before the first frame update

    public event SaveEventHandler OnStartSave;
    public event EmptyEventHandler OnLoadSave;

    float gameTime = 0;
    string SavePointName;
    private void Awake() {
        
        //singleton pattern
        if(instance == null){
            instance = this;
            // ensure is empty when you start the game
            foreach (string file in Directory.GetFiles(Application.persistentDataPath + "/tempsave"))
            {
                File.Delete(file);
            }
            foreach(string directory in Directory.GetDirectories(Application.persistentDataPath + "/tempsave"))
            {
                foreach (string file in Directory.GetFiles(directory))
                {
                    File.Delete(file);
                }
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {

        //initiate the save and load stuff
        SceneManager.sceneLoaded += InvokeLoadSave;
    }
    public void InvokeLoadSave(Scene scene, LoadSceneMode loadSceneMode)
    {
        OnLoadSave?.Invoke();
       
    }

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.unscaledDeltaTime;
    }
    /// <summary>
    /// saves the game
    /// </summary>
    /// <param name="saveFileNubmer"></param>
    public void SaveGame(int saveFileNubmer){
        CreateSaveFiles();
        OnStartSave?.Invoke(saveFileNubmer);
        // To do save everything
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
    }
    public void LoadSaveUI(string savePointName)
    {
        InkManager.instance.isDisplayingChoices = false;
        InkManager.instance.DisableTextboxUI();
        UIManager.instance.overworldOverlay.visible = false;
        SavePointName = savePointName;
        // pause the world

        PauseManager.instance.Pause();

        VisualElement root = UIManager.instance.root;
        VisualElement fileSelectUI = root.Q<VisualElement>("overworld_file_select");

        UpdateSaveFileUI(fileSelectUI, true);
        fileSelectUI.visible = true;
    }

    /// <summary>
    /// saves the current scene that the player is in
    /// </summary>
    public void SaveCurrentScene()
    {
        string savePath = Application.persistentDataPath + "/tempsave" + "/loadedscenes";
        string jsonString = JsonUtility.ToJson(new SceneSave { sceneName = SceneManager.GetActiveScene().name });
        File.WriteAllText(savePath, jsonString);
    }

    /// <summary>
    /// update the savefile ui
    /// </summary>
    /// <param name="saveFileUI">the visual element with the savefile ui</param>
    /// <param name="isSaving">whether or not it is saving or loading</param>
    public void UpdateSaveFileUI(VisualElement saveFileUI, bool isSaving = false){
        saveFileUI.visible = true;
        bool selectedFile = false;
        for (int i = 1; i <= 2; i++)
        {
            TemplateContainer fileContainer = saveFileUI.Q<TemplateContainer>("save_file" + i.ToString());
            Button currentFile = fileContainer.Q<Button>("background");
            if (File.Exists(Application.persistentDataPath + "/save" + i.ToString() + "/SavePointData") || (isSaving))
            {
                currentFile.SetEnabled(true);
                Label currentTime = currentFile.Q<Label>("time");

                string savePath = Application.persistentDataPath + "/save" + i.ToString() + "/SavePointData";
                string jsonString = File.ReadAllText(savePath);
                SavePointData savePointData = JsonUtility.FromJson<SavePointData>(jsonString);

                float remainder = savePointData.timePassed;
                int hours = (int)remainder / 3600;
                remainder -= (hours * 3600);
                int minutes = (int)remainder / 60;
                remainder -= minutes * 60;
                int seconds = (int)remainder;

                currentTime.text = "Time: " + hours.ToString() + " : " + minutes.ToString() + " : " + seconds.ToString();

                Label location = currentFile.Q<Label>("location");
                location.text = savePointData.savePointName.ToString();

                if (!selectedFile)
                {
                    currentFile.Focus();
                }
            }
            else
            {
                currentFile.SetEnabled(false);
            }

        }
    }
    /// <summary>
    /// update the the 
    /// </summary>
    /// <param name="currentFile">the button with the save file info</param>
    /// <param name="saveFileNumber">the save file number</param>
    public void UpdateSaveFile(Button currentFile, int saveFileNumber){
        AudioManager.playSound("menuselect");
        Label currentTime = currentFile.Q<Label>("time");
        string savePath = Application.persistentDataPath + "/save" + saveFileNumber.ToString() + "/SavePointData";
        string jsonString = File.ReadAllText(savePath);
        SavePointData savePointData = JsonUtility.FromJson<SavePointData>(jsonString);

        float remainder = savePointData.timePassed;
        int hours = (int)remainder / 3600;
        remainder -= (hours * 3600);
        int minutes = (int)remainder / 60;
        remainder -= minutes * 60;
        int seconds = (int)remainder;

        currentTime.text = "Time: " + hours.ToString() + " : " + minutes.ToString() + " : " + seconds.ToString();

        Label location = currentFile.Q<Label>("location");
        location.text = savePointData.savePointName.ToString();
    }

    public void LoadGame(int saveFileNumber)
    {
        CreateSaveFiles();
        if (saveFileNumber != 0)
        {
            string savePath = Application.persistentDataPath + "/save" + saveFileNumber.ToString();

            foreach (string file in Directory.GetFiles(Application.persistentDataPath + "/tempsave"))
            {
                File.Delete(file);
            }
            foreach (string directory in Directory.GetDirectories(Application.persistentDataPath + "/tempsave"))
            {
                foreach (string file in Directory.GetFiles(directory))
                {
                    File.Delete(file);
                }
            }
            // load the temp file up with the data
            foreach (string file in Directory.GetFiles(savePath))
            {
                string fileSavePath = Path.Combine(Application.persistentDataPath + "/tempsave", Path.GetFileName(file));
                File.Copy(file, fileSavePath);
            }
            foreach (string directory in Directory.GetDirectories(savePath))
            {
                string subDirectory = directory.Substring((savePath).Length);
                if (!Directory.Exists(Application.persistentDataPath + "/tempsave" + subDirectory)) Directory.CreateDirectory(Application.persistentDataPath + "/tempsave" + subDirectory);
                foreach (string file in Directory.GetFiles(directory))
                {
                    string fileSavePath = Path.Combine(Application.persistentDataPath + "/tempsave" + subDirectory, Path.GetFileName(file));
                    File.Copy(file, fileSavePath);
                }
            }
        }
        string savePointPath = Application.persistentDataPath + "/tempsave" + "/SavePointData";
        string jsonString = File.ReadAllText(savePointPath);
        SavePointData savePointData = JsonUtility.FromJson<SavePointData>(jsonString);
        gameTime = savePointData.timePassed;
        // clear the temp file


        LoadCurrentSubscenes();

        //SceneManager.sceneLoaded += InvokeLoadSave;
        //OnLoadSave?.Invoke();
        //LoadAtmosphere();
    }
    public void LoadCurrentSubscenes()
    {
        string savePath = Application.persistentDataPath + "/tempsave" + "/loadedscenes";
        string jsonString = File.ReadAllText(savePath);
        SceneSave subScenesData = JsonUtility.FromJson<SceneSave>(jsonString);
        
        SceneManager.LoadSceneAsync(subScenesData.sceneName);
    }
    /// <summary>
    /// ensures that all the folders that need to exist to save do in fact exist
    /// </summary>
    public void CreateSaveFiles()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/tempsave")) Directory.CreateDirectory(Application.persistentDataPath + "/tempsave");
        if (!Directory.Exists(Application.persistentDataPath + "/save1")) Directory.CreateDirectory(Application.persistentDataPath + "/save1");
        if (!Directory.Exists(Application.persistentDataPath + "/save2")) Directory.CreateDirectory(Application.persistentDataPath + "/save2");
        if (!Directory.Exists(Application.persistentDataPath + "/tempsave" + "/" + SceneManager.GetActiveScene().name)) Directory.CreateDirectory(Application.persistentDataPath + "/tempsave" + "/" + SceneManager.GetActiveScene().name);
    }
}
public delegate void SaveEventHandler(int saveFileNumber);

[System.Serializable]
public struct AnimationSaveData
{
    public string name;
    public float normilizedtime;
}
[System.Serializable]
public struct SavePointData
{
    public string savePointName;
    public float timePassed;
}
[System.Serializable]
public struct SceneSave
{
    public string sceneName;
}
