using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;

    void Update()
    {
        // Get forward movement input and calculate the forward direction based on current rotation
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 forwardDirection = new Vector3(Mathf.Sin(transform.eulerAngles.y * Mathf.Deg2Rad), 0, Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad));

        // Apply movement in the forward direction based on input
        Vector3 movement = forwardDirection * -moveVertical * speed * Time.deltaTime;
        if (moveVertical >= 0)
            transform.position += movement;

        // Rotate ship based on horizontal input only if it's moving
        float rotateHorizontal = Input.GetAxis("Horizontal");
        if (movement.magnitude != 0)
            transform.Rotate(0, rotateHorizontal * rotationSpeed * Time.deltaTime, 0);
    }
}
