using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FollowPlayer : MonoBehaviour
{
    public Vector3 currentPosition;
    Animator animator;

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

    public followingState state = followingState.following;
    public AIPath aiPath;

    Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        //movement = GetComponent<Movement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseManager.isPaused)
        {
            rb.velocity = Vector2.zero;
            aiPath.enabled = false;
            return;
        }
        aiPath.enabled = true;
        Vector2 direction = aiPath.velocity;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                animator.SetBool("IsWalking", true);
                animator.Play(RightAnimationName);
            }
            else
            {
                animator.SetBool("IsWalking", true);
                animator.Play(LeftAnimationName);
            }
        }
        else if (Mathf.Abs(direction.y) > 0)
        {
            if (direction.y > 0)
            {
                animator.SetBool("IsWalking", true);
                animator.Play(UpAnimationName);
            }
            else
            {
                animator.SetBool("IsWalking", true);
                animator.Play(DownAnimationName);
            }
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }


    private void LateUpdate()
    {/*
        if (!(follingMovement.lastDirection == Vector2.zero))
        {
            timePassed += Time.deltaTime;
            if (followPositions.Count < (int)(timeOffset / updateRate))
            {
                //Debug.Log("ello");
                if (timePassed > updateRate)
                {
                    timePassed = 0;
                    //Start moving to the next position
                    currentPosition = transform.position;
                    followPositions.Enqueue(follingMovement.rb2D.position);
                }
            }
            else
            {
                if (timePassed > updateRate)
                {
                    timePassed = timePassed - updateRate;
                    
                    currentPosition = followPositions.Dequeue();
                    //add new position
                    followPositions.Enqueue(follingMovement.transform.position);
                    
                    transform.position = Vector3.Lerp(currentPosition, followPositions.Peek(), timePassed / updateRate);
                    
                } 
                else
                {
                    transform.position = Vector3.Lerp(currentPosition, followPositions.Peek(), timePassed / updateRate);
                    
                    //rb.velocity = (followPositions.Peek() - currentPosion) / updateRate;
                }
            }
        }
        else
        {
            Debug.Log("zero");
            //rb.velocity = Vector2.zero;
        }*/
    }
}
public enum followingState
{
    stopped,
    following
}
public struct movementData
{
    public Direction direction;
    Vector3 velocity;
}
