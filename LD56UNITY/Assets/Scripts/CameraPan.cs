using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPan : MonoBehaviour
{
    public float panSpeed = 50f;  // Speed at which the camera rotates

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleCameraPan();
    }

    void HandleCameraPan()
    {
        float rotateHorizontal = Input.GetAxis("Mouse X");

        if (rotateHorizontal != 0)
        {
            float rotationAmount = rotateHorizontal * panSpeed * Time.deltaTime;

            transform.Rotate(0, rotationAmount, 0);
        }
    }
}
