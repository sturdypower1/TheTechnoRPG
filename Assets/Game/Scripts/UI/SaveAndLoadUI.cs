using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class SaveAndLoadUI : MonoBehaviour
{
    public event SaveEventHandler OnSaveFilePressed;
    public event EmptyEventHandler OnBackPressed;

    private UIDocument _UIDoc;
    private VisualElement _background;

    
    public void Enable()
    {
        _background.visible = true;
        UpdateSaveFileUI();
    }

    public void Disable()
    {
        _background.visible = false;
    }
    public void UpdateSaveFileUI(bool isSaving = false)
    {
        bool selectedFile = false;
        for (int i = 1; i <= 2; i++)
        {
            TemplateContainer fileContainer = _background.Q<TemplateContainer>("save_file" + i.ToString());
            Button currentFile = fileContainer.Q<Button>("background");
            if (File.Exists(Application.persistentDataPath + "/save" + i.ToString() + "/SavePointData") || (isSaving))
            {
                currentFile.SetEnabled(true);

                string savePath = Application.persistentDataPath + "/save" + i.ToString() + "/SavePointData";
                string jsonString = File.ReadAllText(savePath);

                SavePointData savePointData = JsonUtility.FromJson<SavePointData>(jsonString);
                Label location = currentFile.Q<Label>("location");
                location.text = savePointData.savePointName.ToString();

                UpdateSaveFileTime(currentFile, i, savePointData.timePassed);

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
    private void Awake()
    {
        _UIDoc = GetComponent<UIDocument>();
        var root = _UIDoc.rootVisualElement;
        _background = root.Q<VisualElement>("file_select");

        TemplateContainer fileContainer1 = _background.Q<TemplateContainer>("save_file1");
        TemplateContainer fileContainer2 = _background.Q<TemplateContainer>("save_file2");
        var _fileUI1 = fileContainer1.Q<Button>("background");
        var _fileUI2 = fileContainer2.Q<Button>("background");
        _fileUI1.clicked += () => SaveFileButtonPressed(1);
        _fileUI2.clicked += () => SaveFileButtonPressed(2);

        var backButton = _background.Q<Button>("load_back_button");
        backButton.clicked += BackButton;
    }
    
    private void UpdateSaveFileTime(VisualElement currentFile, int fileNumber, float timePassed)
    {
        Label currentTime = currentFile.Q<Label>("time");

        float remainder = timePassed;
        int hours = (int)remainder / 3600;
        remainder -= (hours * 3600);
        int minutes = (int)remainder / 60;
        remainder -= minutes * 60;
        int seconds = (int)remainder;

        currentTime.text = "Time: " + hours.ToString() + " : " + minutes.ToString() + " : " + seconds.ToString();
    }
    private void SaveFileButtonPressed(int saveFileNumber)
    {
        OnSaveFilePressed?.Invoke(saveFileNumber);
    }
    private void BackButton()
    {
        OnBackPressed?.Invoke();
    }
}
