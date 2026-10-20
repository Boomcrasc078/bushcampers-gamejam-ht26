using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour, IShooting
{
    [SerializeField] InputActionReference moveInput;
    [SerializeField] InputActionReference attackInput;

    public Action Shoot { get; set; }

    public event Action<InputAction.CallbackContext> Attack;

    void OnEnable() => attackInput.action.started += OnAttackStarted;
    void OnDisable() => attackInput.action.started -= OnAttackStarted;

    void OnAttackStarted(InputAction.CallbackContext context) => InvokeShoot(context);

    void InvokeShoot(InputAction.CallbackContext context)
    {
        Shoot.Invoke();
    }

    void Update()
    {
    }

    public Vector2 GetMoveInput()
    {
        return moveInput.action.ReadValue<Vector2>();
    }


}