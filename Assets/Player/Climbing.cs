using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.XR;

namespace ronan.player
{
    public class Climbing : MonoBehaviour
    {
        /// <summary>
        /// fix player being able to bounce against the wall to reset their climb
        /// </summary>

        [Header("Reference")]
        public Transform camOrientation;
        public Transform playerOrientation;
        public Rigidbody rb;
        public LayerMask whatIsWall;
        public PlayerInputs pi;
        private PlayerMovement pm;

        [Header("Climbing")]
        public float climbSpeed = 10f;
        public float maxClimbTime = 0.75f;
        private float climbTimer;

        private bool climbing;

        [Header("Detection")]
        public float detectionLength = 0.7f;
        public float sphereCastRadius = 0.25f;
        public float maxWallLookAngle = 30;
        private float wallLookAngle;

        private RaycastHit frontWallHit;
        private bool wallFront;
        Vector2 movementInput;


        private void Awake()
        {
            pi = new PlayerInputs();
            pm = GetComponent<PlayerMovement>();
        }
        private void OnEnable()
        {
            pi.Enable();
        }
        private void OnDisable()
        {
            pi.Disable();
        }
        private void Update()
        {
            Vector2 movIn = pi.Player.Movement.ReadValue<Vector2>();
            movementInput = movIn;

            WallCheck();
            StateMachine();

            if (climbing) ClimbingMovement();
            if (wallFront && !climbing && climbTimer < maxClimbTime && climbTimer > 0) rb.velocity = new Vector3(rb.velocity.x, -4, rb.velocity.z);
        }
        private void WallCheck()
        {
            wallFront = Physics.SphereCast(transform.position, sphereCastRadius, playerOrientation.forward, out frontWallHit, detectionLength, whatIsWall);
            wallLookAngle = Vector2.Angle(playerOrientation.forward, -frontWallHit.normal);

            if (pm.grounded)
            {
                climbTimer = maxClimbTime;
            }
        }

        private void StateMachine()
        {
            if(wallFront && movementInput.y > 0 && wallLookAngle < maxWallLookAngle)
            {
                if (!climbing && climbTimer > 0) StartClimbing();

                if (climbTimer > 0) climbTimer -= Time.deltaTime;
                if (climbTimer < 0) StopClimbing();
            }

            else
            {
                if (climbing) StopClimbing();
            }
        }
        private void StartClimbing()
        {
            climbing = true;
            pm.climbing = true;
        }
        private void ClimbingMovement()
        {
            rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
        }
        private void StopClimbing()
        {
            climbing = false;
            pm.climbing = false;
        }

    }
}
