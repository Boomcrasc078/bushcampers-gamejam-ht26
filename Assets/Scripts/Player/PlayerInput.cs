using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour, IShooting
{
    [SerializeField] InputActionReference moveInput;
    [SerializeField] InputActionReference attackInput;

    public bool IsShooting()
    {
        return GetShootInput();
    }
    void Update()
    {
    }

    public bool GetShootInput()
    {
        return attackInput.action.IsPressed();
    }

    public Vector2 GetMoveInput()
    {
        return moveInput.action.ReadValue<Vector2>();
    }


}