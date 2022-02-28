using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PrefabSpawnTrackMixer : PlayableBehaviour
{
    public bool wasSpawned = false;
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        Transform spawnPosition = playerData as Transform;

        if (!spawnPosition) return;

        bool temp = true;
        int inputCount = playable.GetInputCount();
        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);

            if (inputWeight > 0f)
            {
                if (!wasSpawned)
                {
                    ScriptPlayable<PrefabSpawnBehaviour> inputPlayable = (ScriptPlayable<PrefabSpawnBehaviour>)playable.GetInput(i);

                    PrefabSpawnBehaviour input = inputPlayable.GetBehaviour();
                    if (BattleManager.instance != null) {
                        BattleManager.instance.InstantializeBattlePrefab(input.prefab, spawnPosition.position);
                    }
                    
                }
                wasSpawned = true;
                temp = false;


            }
        }
        if (temp)
        {
            wasSpawned = false;
        }
    }
}
