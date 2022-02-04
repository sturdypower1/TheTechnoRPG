using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
[TrackBindingType(typeof(Transform))]
[TrackClipType(typeof(PrefabSpawnClip))]
public class PrefabSpawnTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<PrefabSpawnTrackMixer>.Create(graph, inputCount);
    }
}
