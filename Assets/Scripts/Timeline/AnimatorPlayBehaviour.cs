using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AnimatorPlayBehaviour : PlayableBehaviour
{
    public AnimationClip animationClip;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Animator animator = playerData as Animator;
        animator.Play(animationClip.name);
    }
}
