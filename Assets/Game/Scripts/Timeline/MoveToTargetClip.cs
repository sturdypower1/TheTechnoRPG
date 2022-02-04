using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MoveToTargetClip : PlayableAsset
{
    public Vector3 offset;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<MoveToTargetBehaviour>.Create(graph);

        MoveToTargetBehaviour moveBehaviour = playable.GetBehaviour();
        moveBehaviour.offset = offset;

        return playable;
    }
}
