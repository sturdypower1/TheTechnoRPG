using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour, ActionMap.IOverworldActions
{
    public static ActionMap InputActions;

    public Movement movement;

    public void OnBack(InputAction.CallbackContext context)
    {

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movement.move(context.ReadValue<Vector2>());
    }

    public void OnPause(InputAction.CallbackContext context)
    {
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
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
}
