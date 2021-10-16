using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class TriggerCutscene : MonoBehaviour
{
    public CutsceneData cutsceneData;
    public bool isSingleUse;
    public bool isEnabled = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player" && isEnabled)
        {
            InkManager.instance.StartCutScene(cutsceneData);
            if (isSingleUse)
            {
                isEnabled = false;
            }
        }
        
    }
}
