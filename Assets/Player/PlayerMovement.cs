using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Modified externally by other scripts (such as controller)
    public Vector2 moveDirection;
    public Vector2 lookVector;

    private void FixedUpdate()
    {
        transform.position += (Vector3)moveDirection;
    }
}
