using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovementAI : MonoBehaviour
{
    public Movement movement;
    public Direction currentDirection;
    public float followDistance;
    public float waitTime;
    public RandomMovementState state;

    // Update is called once per frame
    void Update()
    {
        

    }
    private void FixedUpdate()
    {
        if (PauseManager.isPaused)
        {
            movement.move(new Vector2(0, 0));
            return;
        }
        Transform playerPositon = PlayerInputManager.instance.transform;
        switch (state)
        {
            case RandomMovementState.RandomDirection:
                movement.move(new Vector2(0, 0));
                if (waitTime > 0)
                {
                    waitTime -= Time.fixedDeltaTime;
                }
                else
                {
                    currentDirection = (Direction)Random.Range(1, 4);
                    state = RandomMovementState.RandomMovement;
                    // how long it will walk for
                    waitTime = Random.Range(1f, 2f);
                }
                break;
            case RandomMovementState.RandomMovement:
                if (waitTime > 0)
                {
                    waitTime -= Time.fixedDeltaTime;
                    switch (currentDirection)
                    {
                        case Direction.Up:
                            movement.move(new Vector2(0, 1));
                            break;
                        case Direction.Down:
                            movement.move(new Vector2(0, -1));
                            break;
                        case Direction.Right:
                            movement.move(new Vector2(1, 0));
                            break;
                        case Direction.Left:
                            movement.move(new Vector2(-1, 0));
                            break;
                    }

                }
                else
                {
                    waitTime = Random.Range(1f, 3f);
                    state = RandomMovementState.RandomDirection;
                }
                break;
            case RandomMovementState.Following:
                movement.move(-1 * (this.transform.position - playerPositon.position).normalized);
                break;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            state = RandomMovementState.Following;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            state = RandomMovementState.RandomDirection;
        }
    }

}

public enum RandomMovementState
{
    RandomDirection,
    RandomMovement,
    Following
}

