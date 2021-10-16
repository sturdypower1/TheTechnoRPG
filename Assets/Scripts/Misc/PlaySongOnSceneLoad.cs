using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySongOnSceneLoad : MonoBehaviour
{
    // Start is called before the first frame update
    public string SongName;
    void Start()
    {
    }

    private void Update()
    {
        AudioManager.playSong(SongName);
        Destroy(this.gameObject);
    }

}
