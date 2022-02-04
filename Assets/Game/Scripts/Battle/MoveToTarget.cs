using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTarget : MonoBehaviour
{
    public Animator animator;

    public Vector2 oldPosition;

    public float timePassed = 0;
    public float duration = 1;

    public bool isFinished;

    private void Start()
    {
        oldPosition = this.transform.position;
    }
    private void Update()
    {
        timePassed += Time.unscaledDeltaTime / duration;
        transform.position = Vector2.Lerp(oldPosition, BattleManager.instance.targetTransform.position, timePassed);
        if(timePassed > 1 && !isFinished)
        {
            isFinished = true;
            animator.SetTrigger("ReachedTarget");
        }
    }
}
