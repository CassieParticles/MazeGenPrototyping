using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    private InputAction move;
    private InputAction look;

    private PlayerMovement playerMovement;


    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        //Get input actions
        move = playerMovement.PlayerInput.FindAction("Move");
        look = playerMovement.PlayerInput.FindAction("Look");
    }

    private void OnEnable()
    {
        //Set up callbacks on enable
        move.performed += MovePlayer;
        look.performed += LookPlayer;
        move.canceled += MovePlayer;
        look.canceled += LookPlayer;

        //Lock cursor when player controls are enabled
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnDisable()
    {
        //Clean up callbacks on disable
        move.performed -= MovePlayer;
        look.performed -= LookPlayer;
        move.canceled -= MovePlayer;
        look.canceled -= LookPlayer;

        //Unlock controls when player controls are disabled
        Cursor.lockState = CursorLockMode.None;
    }

    private void MovePlayer(InputAction.CallbackContext context)
    {
        //Sets variable in playerMovement, ensures player movement doesn't depend on controller
        playerMovement.MoveDirection = context.ReadValue<Vector2>();
    }

    private void LookPlayer(InputAction.CallbackContext context)
    {
        //Setsz variable in playerMovement, ensures player movement doesn't depend on controller
        playerMovement.LookVector = context.ReadValue<Vector2>();
    }
}
