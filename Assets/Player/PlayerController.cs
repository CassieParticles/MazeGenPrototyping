using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]private InputActionAsset playerInput;

    private InputAction move;
    private InputAction look;

    private PlayerMovement playerMovement;


    private void Awake()
    {
        //Get input actions
        move = playerInput.FindAction("Move");
        look = playerInput.FindAction("Look");

        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        //Set up callbacks on enable
        move.performed += MovePlayer;
        look.performed += LookPlayer;
        move.canceled += MovePlayer;
        look.canceled += LookPlayer;
    }

    private void OnDisable()
    {
        //Clean up callbacks on disable
        move.performed -= MovePlayer;
        look.performed -= LookPlayer;
        move.canceled -= MovePlayer;
        look.canceled -= LookPlayer;
    }

    private void MovePlayer(InputAction.CallbackContext context)
    {
        //Setsz variable in playerMovement, ensures player movement doesn't depend on controller
        playerMovement.MoveDirection = context.ReadValue<Vector2>();
    }

    private void LookPlayer(InputAction.CallbackContext context)
    {
        //Setsz variable in playerMovement, ensures player movement doesn't depend on controller
        playerMovement.LookVector = context.ReadValue<Vector2>();
    }
}
