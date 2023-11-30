using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private PlayerRespawn playerRespawn;
    private float respawnDelaySeconds = 2f;

    [Header("Movement")]
    public float speed = 5;
    public float waitTime = .3f;
  
    [Header("Path")]
    public Transform pathHolder;
    Transform player;


    [Header("Light")]
    public Light spotLight;
    public float viewDistace;
    public LayerMask viewMask;
    private float viewAngle;
    private float playerVisableTimer;
    Color originalSLColour;

    public float timeToSpotPlayer = .5f;

    private bool isChasingPlayer = false; // Add a flag to determine if the enemy is chasing the player



    void Start()
    {
        playerRespawn = GameObject.Find("Player").GetComponent<PlayerRespawn>();

        player = GameObject.FindGameObjectWithTag ("Player").transform ;
        viewAngle = spotLight.spotAngle;
        originalSLColour = spotLight.color;

        Vector3[] waypoints = new Vector3[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild (i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }

        StartCoroutine(FollowPath(waypoints));
    }

    private void Update()
    {
        if (CanSeePlayer())
        {
            playerVisableTimer += Time.deltaTime;
            

            if (playerVisableTimer >= timeToSpotPlayer)
            {
                isChasingPlayer = true; // Player is spotted, start chasing
                StartCoroutine(RespawnAfterDelay());
                //playerRespawn.RespawnNow();

            }
        }
        else
        {
            playerVisableTimer -= Time.deltaTime;
            
            isChasingPlayer = false; // Player is not visible, stop chasing
        }
        
        playerVisableTimer = Mathf.Clamp(playerVisableTimer, 0, timeToSpotPlayer);
        spotLight.color = Color.Lerp(originalSLColour, Color.red, playerVisableTimer / timeToSpotPlayer);

        if (isChasingPlayer)
        {
            // Add code to follow the player here
            // For example, you can use a function like MoveTowardsPlayer()
            MoveTowardsPlayer();
        }


    }

    private IEnumerator RespawnAfterDelay()
    {
        // Wait for a specific duration before respawning
        yield return new WaitForSeconds(respawnDelaySeconds);

        // Now respawn the player
        playerRespawn.RespawnNow();
    }

    bool CanSeePlayer()
    {
        if(Vector3.Distance(transform.position, player.position) < viewDistace)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle (transform.forward, dirToPlayer);
           
            if (angleBetweenGuardAndPlayer < viewAngle / 2f)
            {
                if (!Physics.Linecast(transform.position, player.position, viewMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    void MoveTowardsPlayer()
    {
        // Calculate the direction towards the player
        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);

        if (angleBetweenGuardAndPlayer < viewAngle / 2f)
        {
            // You can use the following code to move towards the player
            // This code assumes that 'speed' is the desired chasing speed
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            //make the enemy look in the player's direction
            transform.LookAt(player);
        }
    }




    IEnumerator FollowPath(Vector3[] waypoints)
    {
        transform.position = waypoints[0];

        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];
        transform.LookAt (targetWaypoint);
        

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
            if (transform.position == targetWaypoint)
            {
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];
                yield return new WaitForSeconds(waitTime);
                
                yield return StartCoroutine(TurnToFace(targetWaypoint));
            }

            yield return null;
        }
    }

    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90-Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);

        float elapsedTime = 0;
        float rotationDuration = 1.0f; // Adjust this value to control rotation speed

        while (elapsedTime < rotationDuration)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure that the final rotation is exactly the target rotation
        transform.rotation = targetRotation;
    }

    private void OnDrawGizmos()
    {
        
        //Determines which point is the starting poisiton but seeing the first child in the path object
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPositon = startPosition;

        foreach (Transform waypoint in pathHolder)
        {
            //Draws a sphere for each waypoint in the game editior.
            Gizmos.DrawSphere(waypoint.position, .3f);

            //Draws a line connecting each waypoint together.
            Gizmos.DrawLine (previousPositon, waypoint.position);
            previousPositon = waypoint.position;
        }

        //Draws a line from the last waypoint position to the starting point.
        Gizmos.DrawLine(previousPositon, startPosition);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistace);

    }

   
}
