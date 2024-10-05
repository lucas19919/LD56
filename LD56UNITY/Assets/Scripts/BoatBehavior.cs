using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatBehavior : MonoBehaviour
{
    public Transform floater1;
    public Transform floater2;
    public Transform floater3;
    public Transform floater4;

    public float heightOffset;

    private Vector3[] floaters;
    private float averageHeight;
    private Vector3 currentNormal;

    public float smoothness = 1f;

    public Transform ship;

    private Vector3 direction;

    private void Start()
    {
        // Initialize floaters array and default normal
        floaters = new Vector3[4];
        currentNormal = Vector3.up;
    }

    private void Update()
    {
        // Update floater positions
        floaters[0] = floater1.position;
        floaters[1] = floater2.position;
        floaters[2] = floater3.position;
        floaters[3] = floater4.position;

        getPosition();
        applyPositon();
    }

    void getPosition()
    {
        // Calculate average height and normal from floater positions
        averageHeight = (floaters[0].y + floaters[1].y + floaters[2].y + floaters[3].y) / 4;

        Vector3 average12 = (floater1.position + floater2.position) / 2;
        Vector3 average34 = (floater3.position + floater4.position) / 2;
        direction = (average12 - average34).normalized;
        
    }

    void applyPositon()
    {
        // Apply position based on average floater height
        this.transform.position = new Vector3(this.transform.position.x, averageHeight + heightOffset, this.transform.position.z);
        ship.transform.rotation = Quaternion.Lerp(ship.transform.rotation, Quaternion.LookRotation(direction), smoothness);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach (Vector3 floater in floaters)
        {
            Gizmos.DrawSphere(floater, 0.05f);
        }
    }
}
