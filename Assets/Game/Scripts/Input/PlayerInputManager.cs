using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour, ActionMap.IOverworldActions
{
    public static ActionMap InputActions;

    public Movement movement;

    public static PlayerInputManager instance;
    private void Awake()
    {
        if (instance == null || instance == this)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void OnBack(InputAction.CallbackContext context)
    {

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!PauseManager.isPaused)
        {
            movement.move(context.ReadValue<Vector2>());
        }
        else
        {
            movement.move(new Vector2(0,0));
        }
        
    }

    public void OnPause(InputAction.CallbackContext context)
    {
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        movement.activateSprint(context.ReadValue<float>() > 0);
    }

    private void Start()
    {
        
        // setting up the input to use the callbacks here
        InputActions = new ActionMap();
        InputActions.Overworld.SetCallbacks(this);
        InputActions.Enable();
    }

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// activate overworld input
    /// </summary>
    public void EnableInput()
    {
        InputActions.Overworld.Enable();
    }
    /// <summary>
    /// disable overworld input
    /// </summary>
    public void DisableInput()
    {
        // won't accept release input, so it needs to be set to 0,0
        movement.move(new Vector2(0,0));
        InputActions.Overworld.Disable();
    }
    private void OnDestroy()
    {
        InputActions.Overworld.Disable();
    }
}
