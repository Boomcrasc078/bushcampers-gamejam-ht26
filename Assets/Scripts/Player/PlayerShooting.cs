using System;
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(PlayerInput))]
public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    private PlayerInput playerInput;
    [SerializeField] private Transform playerRotationTransform;
    [SerializeField] private Transform fireTransform;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }

    void Start()
    {
        playerInput.Attack += Shoot;

    }

    private void Shoot(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        var bullet = GameObject.Instantiate(bulletPrefab, fireTransform.position, playerRotationTransform.rotation);
        var bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
        var bulletCollider = bullet.GetComponent<Collider2D>();
        bulletRigidbody.AddRelativeForce(Vector2.up * 1000f);
    }

    // Update is called once per frame
    void Update()
    {

    }


}
