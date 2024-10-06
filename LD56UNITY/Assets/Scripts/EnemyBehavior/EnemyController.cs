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

    private float playerDistance;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cannonFire = GetComponent<CannonFire>();
    }

    void Update()
    {
        playerDistance = (player.position - origin.position).magnitude;

        if (playerDistance <= (2 * patrolRadius))
            SetState(AIState.ENGAGE);
        else
            SetState(AIState.PATROL);


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

        Vector3 movement = transform.forward * speed * Time.deltaTime;
        rb.velocity = new Vector3 (movement.x, 0, movement.z);
    }

    private void Engage()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        Vector3 directionToPlayerNormalized = directionToPlayer.normalized;

        // Step 1: Move to Optimal Distance
        float optimalDistance = 25.0f;
        float distanceTolerance = 5.0f;

        if (distanceToPlayer > optimalDistance + distanceTolerance)
        {
            // Move towards the player if too far
            rb.velocity = transform.forward * speed;
        }
        else if (distanceToPlayer < optimalDistance - distanceTolerance)
        {
            // Move away from the player if too close
            rb.velocity = -transform.forward * speed;
        }
        else
        {
            // Stop movement if within optimal distance
            rb.velocity = Vector3.zero;
        }

        // Step 2: Align Perpendicular to Player
        // Check if we are nearly perpendicular (using dot product with transform.right)
        float dotProduct = Vector3.Dot(transform.right, directionToPlayerNormalized);

        // Rotate until the boat is perpendicular
        if (Mathf.Abs(dotProduct) < 0.95f)
        {
            // Calculate target rotation so that transform.right is aligned with direction to player
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayerNormalized, Vector3.up) * Quaternion.Euler(0, 90, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
        }
        else
        {
            // Step 3: Fire Cannons when aligned perpendicularly
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
