using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShipFloat : MonoBehaviour
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

    private Vector3 direction;
    private float averageHeight;
    private float zRotation;

    void Update()
    {
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
}
