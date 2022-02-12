using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Playables;
[CreateAssetMenu(menuName = "Skill/Basic Cutscene Skill")]
public class BasicCutsceneSkill : Skill
{
    public PlayableAsset playable;
    /// <summary>
    /// the tracks that the user uses in the timeline
    /// </summary>
    public List<string> userTracks;
    /// <summary>
    /// the tracks that the target uses in the timeline
    /// </summary>
    public List<string> targetTracks;

    public override void UseSkill(Battler target, Battler user)
    {
        user.target = target;

        PlayableDirector director = user.skillDirector;
        director.playableAsset = playable;


        // fixes all of the bindings in the timeline playable
        foreach(var playableAssetOutput in director.playableAsset.outputs)
        {
            if (userTracks.Contains(playableAssetOutput.streamName))
            {
                director.SetGenericBinding(playableAssetOutput.sourceObject, user.GetComponent(playableAssetOutput.outputTargetType));
            }
            else if (targetTracks.Contains(playableAssetOutput.streamName))
            {
                director.SetGenericBinding(playableAssetOutput.sourceObject, target.GetComponent(playableAssetOutput.outputTargetType));
            }
            else if (playableAssetOutput.streamName.StartsWith("user"))
            {
                director.SetGenericBinding(playableAssetOutput.sourceObject, user.GetComponent(playableAssetOutput.outputTargetType));
            }
            else if (playableAssetOutput.streamName.StartsWith("target"))
            {
                director.SetGenericBinding(playableAssetOutput.sourceObject, target.GetComponent(playableAssetOutput.outputTargetType));
            }
        }

        director.Play();


        director.stopped += UnpauseOnCutSceneFinish;
    }

    public void UnpauseOnCutSceneFinish(PlayableDirector aDirector)
    {
        aDirector.stopped -= UnpauseOnCutSceneFinish;
    }
}
