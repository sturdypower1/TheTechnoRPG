using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SaveAndLoadManager : MonoBehaviour
{
    public static SaveAndLoadManager instance;
    // Start is called before the first frame update
    private void Awake() {
        
        //singleton pattern
        if(instance == null){
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        //initiate the save and load stuff
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// saves the game
    /// </summary>
    /// <param name="saveFileNubmer"></param>
    public void SaveGame(int saveFileNubmer){

    }
    /// <summary>
    /// update the savefile ui
    /// </summary>
    /// <param name="saveFileUI">the visual element with the savefile ui</param>
    /// <param name="isSaving">whether or not it is saving or loading</param>
    public void UpdateSaveFileUI(VisualElement saveFileUI, bool isSaving = false){
    }
    /// <summary>
    /// update the the 
    /// </summary>
    /// <param name="currentFile">the button with the save file info</param>
    /// <param name="saveFileNumber">the save file number</param>
    public void UpdateSaveFile(Button currentFile, int saveFileNumber){
    }
}
