using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOrientation : MonoBehaviour
{
    [SerializeField] private Transform cameraObject;
    private PlayerInputs pi;
    private Vector2 desiredDir;
    Vector3 forward;
    Vector3 backward;
    Vector3 left;
    Vector3 right;

    private void Awake()
    {
        pi = new PlayerInputs();
        forward = new Vector3(0f, 0f, 0f);
        backward = new Vector3(0f, -180f, 0f);
        left = new Vector3(0f, -90f, 0f);
        right = new Vector3(0f, 90f, 0f);
    }
    private void OnEnable()
    {
        pi.Enable();
    }
    private void OnDisable()
    {
        pi.Disable();
    }

    private void FixedUpdate()
    {
        Vector2 rawMovementInput = pi.Player.Movement.ReadValue<Vector2>();
        Vector3 newMovementInput = new Vector3(rawMovementInput.x, rawMovementInput.y, 0);

        Vector3 tarForward = cameraObject.forward;

        // Vector2 tarRight = cameraObject.right;

        //tarForward.x = 0f;
        //tarRight.y = 0f;

        //desiredDir = tarForward * rawMovementInput.y + tarRight * rawMovementInput.x;

        if (rawMovementInput.magnitude >= 0.1f)
        {

            Vector3 dir = cameraObject.forward;
            dir = new Vector3(0, dir.y, 0);
            Vector3 offSetDir = new Vector3();
            if (newMovementInput.x < 0.1f)
            {
                offSetDir = left;
            }
            else if (newMovementInput.x > 0.1f)
            {
                offSetDir = right;
            }
            if (newMovementInput.y > 0.1f)
            {
                offSetDir = forward;
            }
            else if (newMovementInput.y < 0.1f)
            {
                offSetDir = backward;
            }

            Quaternion trueDir = Quaternion.Euler(cameraObject.forward + offSetDir);

            gameObject.transform.rotation = trueDir;
            //gameObject.transform.rotation = Quaternion.LookRotation(rawMovementInput);
      
        }
    }
}
