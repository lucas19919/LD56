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

    // Constants for boat behavior
    float optimalDistance = 7.5f;
    float obstacleAvoidanceStrength = 1.0f;

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
        Vector3 directionToPlayer = player.position - transform.position; // Direction towards the player
        float distanceToPlayer = directionToPlayer.magnitude; // Calculate distance to the player
        directionToPlayer.Normalize(); // Normalize the direction to the player

        // Step 1: Check for obstacles and avoid if necessary
        if (CheckForObstacles(out Vector3 avoidanceDirection))
        {
            // Steer away from the obstacle if detected
            SteerTowards(avoidanceDirection);
        }
        else
        {
            // Step 2: Engage with the player normally
            if (distanceToPlayer > optimalDistance + 2.0f) // Too far, steer inward
            {
                SteerTowards(directionToPlayer); // Steer toward the player to reduce distance
            }
            else if (distanceToPlayer < optimalDistance - 2.0f) // Too close, steer outward
            {
                SteerAway(directionToPlayer); // Steer away from the player to increase distance
            }
            else
            {
                // Within optimal distance, just circle the player
                CirclePlayer(directionToPlayer);
            }

            // Step 3: Fire when perpendicular to the player
            if (IsPerpendicularToPlayer(directionToPlayer))
            {
                cannonFire.FireCannon(); // Fire when aligned for the shot
            }
        }

        // Step 4: Move forward
        rb.velocity = transform.forward * speed * Time.deltaTime; // Always move forward
    }

    // This method checks for obstacles in front of the boat and returns a steering direction if found
    private bool CheckForObstacles(out Vector3 avoidanceDirection)
    {
        float rayDistance = 15.0f; // How far the ray will check for obstacles
        avoidanceDirection = Vector3.zero;

        // Cast a ray forward to detect obstacles
        if (Physics.Raycast(transform.position, transform.forward, rayDistance))
        {
            // Obstacle detected in front, we need to steer away
            // Try to steer to the right or left depending on open space
            Vector3 rightCheck = transform.right * 2; // Check to the right
            Vector3 leftCheck = -transform.right * 2; // Check to the left

            // Cast rays to the right and left to find open space
            bool rightClear = !Physics.Raycast(transform.position, transform.right, rayDistance / 2);
            bool leftClear = !Physics.Raycast(transform.position, -transform.right, rayDistance / 2);

            if (rightClear)
            {
                avoidanceDirection = transform.right * obstacleAvoidanceStrength;
            }
            else if (leftClear)
            {
                avoidanceDirection = -transform.right * obstacleAvoidanceStrength;
            }
            else
            {
                // If both directions are blocked, steer hard left as a last resort
                avoidanceDirection = -transform.right * obstacleAvoidanceStrength;
            }

            return true; // Return true since we detected an obstacle
        }

        return false; // No obstacle detected
    }

    // This method steers the boat towards the player or an avoidance direction
    private void SteerTowards(Vector3 targetDirection)
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
    }

    // This method steers the boat away from the player to increase distance
    private void SteerAway(Vector3 directionToPlayer)
    {
        Vector3 awayDirection = -directionToPlayer; // Invert direction to move away
        SteerTowards(awayDirection);
    }

    // This method keeps the boat circling the player at optimal distance
    private void CirclePlayer(Vector3 directionToPlayer)
    {
        Vector3 tangentDirection = Vector3.Cross(directionToPlayer, Vector3.up).normalized; // Tangent direction for circling
        SteerTowards(tangentDirection); // Steer towards the tangent to circle the player
    }

    // This method checks if the boat is perpendicular to the player for firing
    private bool IsPerpendicularToPlayer(Vector3 directionToPlayer)
    {
        float dotProduct = Vector3.Dot(transform.right, directionToPlayer); // Dot product between the right side and the player
        return Mathf.Abs(dotProduct) > 0.98f; // Consider it perpendicular if close to 1 (or -1)
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
