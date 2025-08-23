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
            //Swizzle values to move on xz plane (horizontal)
            moveDirection.z = moveDirection.y;
            moveDirection.y = 0;
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

    //Player controls
    [Header("Player controls")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float mouseSensitivity = 300.0f;

    private Transform cameraTransform;

    private Vector3 cameraRotation;

    private Vector3 moveDirection;
    private Vector2 lookVector;


    private void Awake()
    {
        cameraTransform = GetComponentInChildren<Camera>().transform;
        
    }

    private void FixedUpdate()
    {
        //Update looking direction

        //Manual swizzling to avoid new vector3 allocation
        cameraRotation.y += lookVector.x * mouseSensitivity * Time.deltaTime;
        cameraRotation.x -= lookVector.y * mouseSensitivity * Time.deltaTime;

        //Rotate camera
        cameraTransform.rotation = Quaternion.Euler(cameraRotation);

        //Move player in direction of camera (rotate move direction vector, then scale with speed and delta time)
        transform.position += Quaternion.Euler(0, cameraRotation.y, 0) * moveDirection * moveSpeed * Time.deltaTime;
    }
}
