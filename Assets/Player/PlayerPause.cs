using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPause : MonoBehaviour
{
    [SerializeField]GameEvent pauseEvent;
    [SerializeField]GameEvent unpauseEvent;

    PlayerMovement playerMovement;

    InputAction PauseKey;

    private bool paused;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        PauseKey = playerMovement.PlayerInput.FindAction("Pause");
    }

    private void OnEnable()
    {
        PauseKey.started += PauseKeyPress;
    }


    private void OnDisable()
    {
        PauseKey.started -= PauseKeyPress;
    }

    private void Start()
    {

    }

    private void PauseKeyPress(InputAction.CallbackContext context)
    {
        //Change if game is paused or unpaused
        paused = !paused;

        //Signal game is paused or unpaused
        if(paused)
        {
            pauseEvent.InvokeListeners();
        }
        else
        {
            unpauseEvent.InvokeListeners();
        }
    }
}
