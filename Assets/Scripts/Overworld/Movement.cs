using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rigidbody2D;
    /// <summary>
    /// the animation name for moving down
    /// </summary>
    public string DownAnimationName;
    /// <summary>
    /// the animation name for moving up
    /// </summary>
    public string UpAnimationName;
    /// <summary>
    /// the animation name for moving left
    /// </summary>
    public string LeftAnimationName;
    /// <summary>
    /// the animation name for moving right
    /// </summary>
    public string RightAnimationName;

    /// <summary>
    /// the velocity at which the character moves
    /// </summary>
    public float speed;
    /// <summary>
    /// how much faster the character moves when sprinting
    /// </summary>
    public float sprintBoost;
    // Start is called before the first frame update

    public void move(Vector2 direction){
        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if(direction.x > 0)
            {
                animator.Play(RightAnimationName);
            }
            else
            {
                animator.Play(LeftAnimationName);
            }
        }
        else if(Mathf.Abs(direction.y) > Mathf.Abs(direction.x))
        {
            if(direction.y > 0)
            {
                animator.Play(UpAnimationName);
            }
            else
            {
                animator.Play(DownAnimationName);
            }
        }
        rigidbody2D.velocity = direction* speed;
    }
}
