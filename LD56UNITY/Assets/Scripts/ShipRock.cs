using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipRock : MonoBehaviour
{
    public float rockAmount = 5.0f;
    public float rockSpeed = 1.0f;

    void Update()
    {
        float zRotation = rockAmount * Mathf.Sin(Time.time * rockSpeed);
        this.transform.localEulerAngles = new Vector3(this.transform.localEulerAngles.x, this.transform.localEulerAngles.y, zRotation);
    }
}
