using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class FileLoadManager : MonoBehaviour
{
    public event EmptyEventHandler OnBackPressed;

    [SerializeField] private SaveAndLoadUI saveAndLoadUI;
    public void Activate()
    {
        saveAndLoadUI.Enable();
    }
    public void Deactivate()
    {
        saveAndLoadUI.Disable();
    }

    public void ContinueGameButton(int saveFileNubmer)
    {
        AudioManager.playSound("menuselect");
        Deactivate();

        LoadGame(saveFileNubmer);
    }
    private void LoadGame(int saveFileNumber)
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
                Debug.Log("copying file");
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
        


        LoadCurrentSubscenes();

        //SceneManager.sceneLoaded += InvokeLoadSave;
        //OnLoadSave?.Invoke();
        //LoadAtmosphere();
    }
    private void LoadCurrentSubscenes()
    {
        string savePath = Application.persistentDataPath + "/tempsave" + "/loadedscenes";
        string jsonString = File.ReadAllText(savePath);
        SceneSave subScenesData = JsonUtility.FromJson<SceneSave>(jsonString);

        SceneManager.LoadSceneAsync(subScenesData.sceneName);
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
        saveAndLoadUI.OnSaveFilePressed += ContinueGameButton;
        saveAndLoadUI.OnBackPressed += BackPressed;
    }
    private void ClearTempFiles()
    {
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
    }
    private void Start()
    {
        ClearTempFiles();
    }
    private void BackPressed()
    {
        AudioManager.playSound("menuback");
        Deactivate();
        OnBackPressed?.Invoke();
    }
}
