using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb2D;

    public Direction Direction;
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

    bool isSprinting = false;

    public Vector2 lastDirection = new Vector2(0,0);

    public AudioSource footStepSound;
    // Start is called before the first frame update
    public void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    public void activateSprint(bool shouldSprint)
    {
        isSprinting = shouldSprint;
        move(lastDirection);
    }
    public void move(Vector2 direction)
    {
        lastDirection = direction;
        if(Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if(direction.x > 0)
            {
                animator.SetBool("IsWalking", true);
                animator.Play(RightAnimationName);
                Direction = Direction.Right;
            }
            else
            {
                animator.SetBool("IsWalking", true);
                animator.Play(LeftAnimationName);
                Direction = Direction.Left;
            }
        }
        else if(Mathf.Abs(direction.y) > 0)
        {
            if(direction.y > 0)
            {
                animator.SetBool("IsWalking", true);
                animator.Play(UpAnimationName);
                Direction = Direction.Up;
            }
            else
            {
                animator.SetBool("IsWalking", true);
                animator.Play(DownAnimationName);
                Direction = Direction.Down;
            }
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
        rb2D.velocity = isSprinting ? direction * speed + direction *  sprintBoost : direction* speed;

    }

    public void playFootStep()
    {
        //footStepSound.Play();
    }
}
public enum Direction
{
    Up,
    Down,
    Left,
    Right
}
