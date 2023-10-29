using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;

public class Climbing : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;
    public PlayerMovement pm;
    public LayerMask whatIsWall;

    [Header("Climbing")]
    public float climbSpeed;
    public float maxClimbTime;
    private float climbTimer;

    private bool climbing;

    [Header("Detection")]
    public float detectionLength;
    public float sphereCastRadius;
    public float maxWallLookAngle;
    private float wallLookAngle;

    private RaycastHit frontWallHit;
    private bool wallFront;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        WallCheck();
        StateMachine();

        if (climbing)
        {
            ClimbingMovement();
        }
    }

    private void StateMachine()
    {
        //State 1 - Climbing
        if (wallFront && Input.GetKey(KeyCode.W) && wallLookAngle < maxWallLookAngle)
        {
            if (!climbing && climbTimer > 0) 
            {
                StartClimbing();
            }

            if ( climbTimer > 0)
            {
                climbTimer -= Time.deltaTime;
            }

            if (climbTimer < 0)
            {
                StopClimbing();
            }
        }

        //State 3 - None
        else
        {
            if (climbing)
            {
                StopClimbing();
            }
        }
    }

    private void WallCheck()
    {
        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, orientation.forward, out frontWallHit, detectionLength, whatIsWall);
        wallLookAngle = Vector3.Angle(orientation.forward, -frontWallHit.normal);

        if(pm.grounded)
        {
            climbTimer = maxClimbTime;
        }
    }

    private void StartClimbing()
    {
        climbing = true;

        //Cha
    }

    private void ClimbingMovement()
    {
        rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);

        //play sound while climbing
    }

    private void StopClimbing()
    {
        climbing = false;
    }
}
