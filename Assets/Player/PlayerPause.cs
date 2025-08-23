using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPause : MonoBehaviour
{
    GameEvent pauseEvent;
    GameEvent unpauseEvent;

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
        pauseEvent = GameEvent.GetEvent("Pause");
        unpauseEvent = GameEvent.GetEvent("Unpause");
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
