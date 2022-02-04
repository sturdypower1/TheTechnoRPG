using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PrefabSpawnClip : PlayableAsset
{
    public GameObject prefab;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<PrefabSpawnBehaviour>.Create(graph);

        PrefabSpawnBehaviour prefabSpawnBehaviour = playable.GetBehaviour();
        prefabSpawnBehaviour.prefab = prefab;

        return playable;
    }
}
