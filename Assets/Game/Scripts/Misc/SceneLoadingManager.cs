using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class SceneLoadingManager : MonoBehaviour
{
    public static SceneLoadingManager instance;
    public VisualElement BlackScreen;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {
        
    }
    IEnumerator LoadSceneCouroutine(SceneLoadingData sceneData)
    {
        Vector3 spawnPosition = sceneData.PlayerSpawnPosition.position;
        float timePassed = 0;
        while(timePassed < .2)
        {
            yield return null;
            timePassed += Time.deltaTime;
            BlackScreen.style.opacity = new StyleFloat(timePassed / .1f);
        }

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneData.sceneName);
        // sets the position of all the players to desired position
        
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        foreach (PlayerController player in PlayerPartyManager.instance.players)
        {
            
            player.transform.position = spawnPosition;
            Debug.Log(player.transform.position);
        }
        // loading finished
        timePassed = 0;
        while(timePassed < .2)
        {
            yield return null;
            timePassed += Time.deltaTime;
            BlackScreen.style.opacity = new StyleFloat(1 - (timePassed / .1f));
        }
        BlackScreen.visible = false;
        PauseManager.instance.UnPause();
    }
    // Start is called before the first frame update
    public void LoadScene(SceneLoadingData sceneData)
    {
        //BlackScreen = UIManager.instance.root.Q<VisualElement>("black_screen");
        BlackScreen.visible = true;
        PauseManager.instance.Pause();
        StartCoroutine(LoadSceneCouroutine(sceneData));
    }
}
[System.Serializable]
public struct SceneLoadingData
{
    public string sceneName;
    public Transform PlayerSpawnPosition;
}