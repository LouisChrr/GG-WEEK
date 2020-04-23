using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class FPSController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    public LayerMask wallMask = -1;
    public float frontRayDistance, backRayDistance;
    public bool canMove = true;
    public bool frontHit, backHit;
    float xRotation;
    float lerpValue;
    public bool rotating;
    public bool isOnWall;
    void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    public void JumpOnWall()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Camera.main.transform.TransformDirection(Vector3.forward), out hit, frontRayDistance, wallMask))
        {
            Debug.DrawRay(transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * hit.distance, Color.red);
            frontHit = true;
        }
        else
        {
            Debug.DrawRay(transform.position, Camera.main.transform.TransformDirection(Vector3.forward) * frontRayDistance, Color.blue);
            frontHit = false;
        }


        if (!isOnWall && frontHit && !characterController.isGrounded || rotating)
        {
            gravity = 0.0f;
            // TOURNER EN X A -90
            // Quaternion targetRot = transform.rotation;
            // xRotation = targetRot.eulerAngles.x;
            // xRotation -= 90;
            // Quaternion newRot = Quaternion.Lerp(transform.rotation, targetRot, lerpValue);
            lerpValue += Time.deltaTime * 2.0f;
            xRotation = Mathf.Lerp(0, -90, lerpValue);
            this.transform.eulerAngles = new Vector3(xRotation, 0, 0);
            if (xRotation > -89.5f)
            {
                rotating = true;
            }
            else
            {
                xRotation = -90f;
                isOnWall = true;
                lerpValue = 0;
                this.transform.eulerAngles = new Vector3(xRotation, 0, 0);
                rotating = false;
            }
        }


    }


}