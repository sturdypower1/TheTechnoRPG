using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
[TrackBindingType(typeof(Transform))]
[TrackClipType(typeof(MoveToTargetClip))]
public class MoveToTargetTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<MoveToTargetMixer>.Create(graph, inputCount);
    }
}
