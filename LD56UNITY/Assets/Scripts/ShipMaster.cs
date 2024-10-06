using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMaster : MonoBehaviour
{
    public Transform floater1;
    public Transform floater2;
    public Transform floater3;
    public Transform floater4;

    public float heightOffset;
    public float smoothness = 1f;
    public float rockAmount;
    public float rockSpeed;

    public Transform ship; 

    public float speed;
    public float rotationSpeed;

    private Vector3 direction;
    private float averageHeight;
    private float zRotation;

    private Rigidbody rb;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }
    private void Update()
    {
        HandleMovement();
        HandleBuoyancyAndRocking();
    }

    void HandleBuoyancyAndRocking()
    {
        averageHeight = (floater1.position.y + floater2.position.y + floater3.position.y + floater4.position.y) / 4;

        Vector3 average12 = (floater1.position + floater2.position) / 2;
        Vector3 average34 = (floater3.position + floater4.position) / 2;
        direction = (average12 - average34).normalized;

        zRotation = rockAmount * Mathf.Sin(Time.time * rockSpeed);

        if (ship != null)
        {
            // Adjust ship's height based on buoyancy
            ship.transform.position = new Vector3(ship.transform.position.x, averageHeight + heightOffset, ship.transform.position.z);

            // Smoothly adjust the ship's rotation based on the calculated direction and rocking
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            ship.transform.rotation = Quaternion.Euler(
                Quaternion.Lerp(ship.transform.rotation, targetRotation, smoothness).eulerAngles.x,
                ship.transform.rotation.eulerAngles.y,
                zRotation
            );
        }
    }

    void HandleMovement()
    {
        float moveVertical = Input.GetAxis("Vertical");
        float rotateHorizontal = Input.GetAxis("Horizontal");

        if (moveVertical > 0)
        {
            Vector3 movement = transform.forward * -moveVertical * speed * Time.deltaTime;
            rb.velocity = new Vector3(movement.x, 0, movement.z);

            this.transform.Rotate(0, rotateHorizontal * rotationSpeed * Time.deltaTime, 0);
        }
        else if (rotateHorizontal != 0)
        {
            float stationaryRotationSpeed = rotationSpeed * 0.33f;
            this.transform.Rotate(0, rotateHorizontal * stationaryRotationSpeed * Time.deltaTime, 0);
        }
    }

    private void OnDrawGizmos()
    {
        // Visualize the floater positions in the editor
        Gizmos.color = Color.yellow;
        if (floater1 && floater2 && floater3 && floater4)
        {
            Gizmos.DrawSphere(floater1.position, 0.05f);
            Gizmos.DrawSphere(floater2.position, 0.05f);
            Gizmos.DrawSphere(floater3.position, 0.05f);
            Gizmos.DrawSphere(floater4.position, 0.05f);
        }
    }
}
