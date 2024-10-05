using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBehavior : MonoBehaviour
{
    public Transform floater1;
    public Transform floater2;
    public Transform floater3;
    public Transform floater4;

    private Vector3[] floaters;

    private void Start()
    {
        floaters = new Vector3[4];
    }

    private void Update()
    {
        floaters[0] = floater1.position;
        floaters[1] = floater2.position;
        floaters[2] = floater3.position;
        floaters[3] = floater4.position;

        getNormal();
        applyPositon();
    }

    void getNormal()
    {

    }




    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (Vector3 floater in floaters)
        {
            Gizmos.DrawSphere(floater, 0.05f);
        }

        Gizmos.DrawLine(floaters[0], floaters[1]);
        Gizmos.DrawLine(floaters[0], floaters[3]);
        Gizmos.DrawLine(floaters[1], floaters[2]);
        Gizmos.DrawLine(floaters[2], floaters[3]);

    }
}
