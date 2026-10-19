using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour {
    [SerializeField] InputActionReference moveInput;

    void Update()
    {
    }

    public Vector2 GetMoveInput()
    {
        return moveInput.action.ReadValue<Vector2>();
    }
}