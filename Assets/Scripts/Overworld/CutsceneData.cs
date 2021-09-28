using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
[System.Serializable]
public struct CutsceneData
{
    public PlayableDirector director;

    public PlayableAsset[] playables;

    public string inkPath;

}
