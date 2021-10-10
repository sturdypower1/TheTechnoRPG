using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class DamageClip : PlayableAsset
{
    public Damage damage;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DamageBehaviour>.Create(graph);

        DamageBehaviour damageBehaviour = playable.GetBehaviour();
        damageBehaviour.damage = damage;

        return playable;
    }
}
