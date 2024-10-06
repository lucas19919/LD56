using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallBehavior : MonoBehaviour
{
    public float speed;
    public float range;

    private Vector3 launchDirection;
    private Vector3 startPosition;
    private bool isLaunched = false;

    void Start()
    {
        startPosition = transform.position; 
    }

    public void Launch(Vector3 direction)
    {
        launchDirection = direction.normalized;
        isLaunched = true;
    }

    void Update()
    {
        if (isLaunched)
        {
            transform.position += launchDirection * speed * Time.deltaTime;

            if (Vector3.Distance(startPosition, transform.position) >= range)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<HealthManager>().cannonHit(33f);
            Debug.Log("hit!");
        }
        
        Destroy(this.gameObject);
    }
}
