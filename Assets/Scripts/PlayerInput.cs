using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private AnimalRunInput animalRunInput;

    private void Awake()
    {
        animalRunInput = new AnimalRunInput();

        animalRunInput.Player.Touch.started += TouchStarted;
        animalRunInput.Player.Touch.canceled += TouchEnded;
    }

    private void OnEnable()
    {
        animalRunInput.Enable();
    }

    private void OnDisable()
    {
        animalRunInput.Disable();
    }

    private void TouchStarted(InputAction.CallbackContext ctx)
    {
        var screenPosition = animalRunInput.Player.TouchPosition.ReadValue<Vector2>();
        EventManager.OnStartTouch(screenPosition, (float) ctx.startTime);
    }

    private void TouchEnded(InputAction.CallbackContext ctx)
    {
        var screenPosition = animalRunInput.Player.TouchPosition.ReadValue<Vector2>();
        EventManager.OnEndTouch(screenPosition, (float) ctx.time);
    }
}