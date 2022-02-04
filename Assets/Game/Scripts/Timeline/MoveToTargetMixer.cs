using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class MoveToTargetMixer : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Transform transform = (Transform)playerData;

        if (!transform) return;

        int inputCount = playable.GetInputCount();
        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);

            if (inputWeight > 0f)
            {
                ScriptPlayable<MoveToTargetBehaviour> inputPlayable = (ScriptPlayable<MoveToTargetBehaviour>)playable.GetInput(i);

                MoveToTargetBehaviour input = inputPlayable.GetBehaviour();

                transform.position = Vector3.Lerp(transform.gameObject.GetComponent<Battler>().battlePosition, transform.gameObject.GetComponent<Battler>().target.transform.position + input.offset, inputWeight);

            }
        }
    }
}
