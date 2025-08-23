using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Modified externally by other scripts (such as controller)
    public Vector2 MoveDirection
    {
        get => moveDirection;
        set
        {
            moveDirection = value.normalized;
        }
    }

    public Vector2 LookVector
    {
        get => lookVector;
        set
        {
            lookVector = value.normalized;
        }
    }

    private Transform cameraTransform;

    private Vector2 moveDirection;
    private Vector2 lookVector;


    private void Awake()
    {
        cameraTransform = GetComponentInChildren<Camera>().transform;
    }

    private void FixedUpdate()
    {
        transform.position += (Vector3)moveDirection;
    }
}
