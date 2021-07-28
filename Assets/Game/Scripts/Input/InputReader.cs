using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using System;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, GameInput.IGameplayActions
{
    // Assign delegate{} to events to initialise them with an empty delegate
    // so we can skip the null check when we use them

    // Gameplay
    public event UnityAction<Vector2> moveEvent = delegate { };

    public event UnityAction clickToMoveEvent = delegate { };
    public event UnityAction clickToMoveCanceledEvent = delegate { };
    public event UnityAction<Vector2> trackPointerPositionEvent = delegate { };

    private GameInput gameInput;

    private bool _isPerformedToMove;
    private bool _isCancelledToMove;

    private void OnEnable()
    {
        if (gameInput == null)
        {
            gameInput = new GameInput();
            //gameInput.Menus.SetCallbacks(this);
            gameInput.Gameplay.SetCallbacks(this);
            //gameInput.Dialogues.SetCallbacks(this);
        }
    }

    private void OnDisable()
    {
        DisableAllInput();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void OnClickToMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            clickToMoveEvent.Invoke();

        if (context.phase == InputActionPhase.Canceled)
            clickToMoveCanceledEvent.Invoke();
    }

    public void OnTrackPointerPosition(InputAction.CallbackContext context)
    {
        trackPointerPositionEvent.Invoke(context.ReadValue<Vector2>());
    }

    public void EnableDialogueInput()
    {
        //gameInput.Menus.Enable();
        //gameInput.Dialogues.Enable();
        gameInput.Gameplay.Disable();
    }

    public void EnableGameplayInput()
    {
        //gameInput.Menus.Disable();
        //gameInput.Dialogues.Disable();
        gameInput.Gameplay.Enable();
    }

    public void EnableMenuInput()
    {
        //gameInput.Dialogues.Disable();
        //gameInput.Menus.Enable();
        gameInput.Gameplay.Disable();
    }

    public void DisableAllInput()
    {
        //gameInput.Menus.Disable();
        gameInput.Gameplay.Disable();
        //gameInput.Dialogues.Disable();
    }


    public void OnClick(InputAction.CallbackContext context)
    {
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
    }

    public void OnCloseInventory(InputAction.CallbackContext context)
    {
    }
    
    private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";

    public bool LeftMouseDown() => Mouse.current.leftButton.isPressed;    
}