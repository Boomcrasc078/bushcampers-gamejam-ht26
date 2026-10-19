using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour {
    [SerializeField] InputActionReference moveInput;
    [SerializeField] InputActionReference attackInput;
    public event Action<InputAction.CallbackContext> Attack;

    void OnEnable() => attackInput.action.started += OnAttackStarted;
    void OnDisable() => attackInput.action.started -= OnAttackStarted;

    void OnAttackStarted(InputAction.CallbackContext context) => Attack?.Invoke(context);

    void Update()
    {
    }

    public Vector2 GetMoveInput()
    {
        return moveInput.action.ReadValue<Vector2>();
    }
 

}