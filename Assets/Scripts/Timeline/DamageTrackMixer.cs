using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class DamageTrackMixer : PlayableBehaviour
{
    public bool wasDamaged = false;
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Battler battler = (Battler) playerData;

        if (!battler) return;

        bool temp = true;
        int inputCount = playable.GetInputCount();
        for(int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            
            if(inputWeight > 0f)
            {
                if (!wasDamaged)
                {
                    ScriptPlayable<DamageBehaviour> inputPlayable = (ScriptPlayable<DamageBehaviour>)playable.GetInput(i);

                    DamageBehaviour input = inputPlayable.GetBehaviour();
                    battler.DealDamage(input.damage);
                    
                }
                wasDamaged = true;
                temp = false;
                
                
            }
        }
        if (temp)
        {
            wasDamaged = false;
        }
    }
}
