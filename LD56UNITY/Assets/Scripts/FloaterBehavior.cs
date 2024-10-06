using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloaterBehavior : MonoBehaviour
{
    private Vector3 position;

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + Vector3.up * 10, Vector3.down, out hit, 20f, 3))
        {
            position = new Vector3(this.transform.position.x, hit.point.y, this.transform.position.z);
        }

        this.transform.position = position;
    }
}
