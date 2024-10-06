using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public enum AIState { PATROL, ENGAGE }
    public AIState currentState = AIState.PATROL;

    public Transform player;
    public Transform origin;

    public float rotSpeed;
    public float speed;

    public float patrolRadius = 25f;

    private Rigidbody rb;
    private CannonFire cannonFire;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cannonFire = GetComponent<CannonFire>();
    }

    void Update()
    {
        switch (currentState)
        {
            case AIState.PATROL:
                Patrol();
                break;
            case AIState.ENGAGE:
                Engage();
                break;
        }
    }

    private void Patrol()
    {
        Vector3 directionToBase = origin.position - transform.position;
        float distanceFromBase = directionToBase.magnitude;

        if (distanceFromBase < patrolRadius - 5 || distanceFromBase > patrolRadius + 5)
        {
            Vector3 desiredDirection = distanceFromBase < patrolRadius - 5 ? -directionToBase : directionToBase;
            Quaternion desiredRotation = Quaternion.LookRotation(desiredDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, rotSpeed * Time.deltaTime);
        }
        else
        {
            Vector3 tangentDirection = Vector3.Cross(directionToBase, Vector3.up).normalized;
            Quaternion desiredRotation = Quaternion.LookRotation(tangentDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, rotSpeed * Time.deltaTime);
        }

        rb.velocity = transform.forward * speed;
    }

    private void Engage()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        float dotProduct = Vector3.Dot(transform.right, directionToPlayer);

        bool obstacleInFront = Physics.Raycast(transform.position, transform.forward, 15);
        bool obstacleBehind = Physics.Raycast(transform.position, -transform.forward, 10);

        // Distance adjustments
        float optimalDistance = 25; // Adjust this based on your needs
        if (distanceToPlayer > optimalDistance + 5 && !obstacleInFront)
        {
            rb.velocity = transform.forward * speed;
        }
        else if (distanceToPlayer < optimalDistance - 5 && !obstacleBehind)
        {
            rb.velocity = transform.forward * -speed;
        }
        else
        {
            rb.velocity = Vector3.zero; // Stop if we are at an optimal distance
        }

        // Orientation adjustments
        Vector3 offsetPosition = player.position - transform.right * optimalDistance; // Adjust AI's target position slightly to the left of the player
        Quaternion targetRotation = Quaternion.LookRotation(offsetPosition - transform.position);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);

        if (Mathf.Abs(dotProduct) > 0.95) // If we are nearly perpendicular to the target
        {
            cannonFire.FireCannon();
        }
    }

    public void SetState(AIState newState)
    {
        currentState = newState;

        if (currentState == AIState.PATROL)
        {
            rb.velocity = Vector3.zero;
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (origin != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(origin.position, patrolRadius);
        }
    }
}
