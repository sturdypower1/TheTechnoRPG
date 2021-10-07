using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AnimatorPlayClip : PlayableAsset
{
    public AnimationClip animationClip;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<AnimatorPlayBehaviour>.Create(graph);

        AnimatorPlayBehaviour animatorPlayBehaviour = playable.GetBehaviour();
        animatorPlayBehaviour.animationClip = animationClip;

        return playable;
    }
}
