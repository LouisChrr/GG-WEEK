using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Assign here")]
    public GameObject CameraObject;
    public GameObject InitialCamera;
    public LayerMask wallMask = -1;

    [Space(10)]


    [Header("Modify here")]

    public float InitialFOV;
    public float RunningFOV;
    public float Xsensitivity;
    public float Ysensitivity;

    public float InitialSpeed;
    public float CrouchingSpeed;
    public float RunningSpeed;
    float CrouchingOffset = 0f;
    public float SpeedReduction;
    public float JumpForce;
    public float frontRayDistance, backRayDistance, downRayDistance;
    // [Space(10)]

    [Header("Don't modify here !! ")]
    public float t,t1;
    public float timer;
    public int LForce = 0;
    private float baseSpeedReduction;
    public float FOV;
    Rigidbody PlayerPhysic;
    int FWDForce = 0;
    public float Speed;
    public bool isCrouching;
    public bool isRoof;
    public bool isInAir;
    public bool isRunning;
    public Vector3 ForwardLookingDirection;
    Vector3 LeftLookingDirection;
    public Vector3 MovingDirection;
    Vector2 InputDirection;
    Vector2 PreviousInputDirection;
    public float Xrotation;
    public float Yrotation;
    public bool IsGrounded;
    RaycastHit hitUp;
    //Vector3 JumpingDirection;
    public bool canJump;
    public bool IsLanded;

    Vector3 DistanceWalked;
    Vector3 PreviousPos;
    public KeyCode Forward;
    public KeyCode Backward;
    public KeyCode Left;
    public KeyCode Right;
    public KeyCode Jump;
    public KeyCode Run;
    public KeyCode Crouch;
    private CapsuleCollider playerCapsuleCollider;
    public Vector3 baseGravity, wallGravity;

    private float baseSpeed;
    public bool frontHit, backHit, downHit;

    Quaternion targetRot, oldRot;
    public float lerpValue;
    public bool rotating, rotatingback;
    public bool isOnWall;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        InitializeTweakVariable();
        baseGravity = Physics.gravity;
        PlayerPhysic = GetComponent<Rigidbody>();
        playerCapsuleCollider = GetComponent<CapsuleCollider>();
        baseSpeed = Speed;
    }

    void Update()
    {
        UpdateColliderSize();
        //if (!rotating && !rotatingback)
       // {
            UpdateCameraRotation();

      //  }

        CameraObject.GetComponent<Camera>().fieldOfView = FOV;
        JumpOnWall();
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

        if((!isOnWall && frontHit && !IsLanded) || rotating)
        {
            // Quaternion newRot = Quaternion.Lerp(transform.rotation, targetRot, lerpValue);
            if (!rotating)
            {
                // ON COMMENCE LA PHASE DE ROTATE
                //targetRot = Quaternion.LookRotation(Camera.main.transform.up, -Camera.main.transform.forward);
                targetRot = Quaternion.LookRotation(Camera.main.transform.TransformDirection(transform.up), -Camera.main.transform.TransformDirection(transform.forward));
                oldRot = Camera.main.transform.rotation;
                rotating = true;
                lerpValue = 0;
            }

            lerpValue += Time.deltaTime*2.0f;
            //xRotation = Mathf.Lerp(0, Camera.main.transform.up, lerpValue);
            
            this.transform.rotation = Quaternion.Lerp(oldRot, targetRot, lerpValue);
            //InitialCamera.transform.eulerAngles = new Vector3(xRotation, 0, 0);
            if (lerpValue < 0.98f)
            {
                rotating = true;
            }
            else
            {
                this.transform.rotation = targetRot;
                isOnWall = true;
                lerpValue = 0;


                rotating = false;
                PlayerPhysic.useGravity = false;
            }
        }


        if (isOnWall)
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, downRayDistance))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.red);
                downHit = true;
            }
            else
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * downRayDistance, Color.yellow);
                downHit = false;
            }


            if (Input.GetKeyDown(Jump) && !rotatingback)
            {
                targetRot = Quaternion.identity;
                oldRot = Camera.main.transform.rotation;
                rotatingback = true;
                lerpValue = 0;
            }

        }


        if ((isOnWall && !downHit) || rotatingback)
        {
           // print("Player saute en haut du mur");
            if (!rotatingback)
            {
                // ON COMMENCE LA PHASE DE ROTATE
                //targetRot = Quaternion.LookRotation(Camera.main.transform.up, -Camera.main.transform.forward);
                //targetRot = Quaternion.LookRotation(Camera.main.transform.TransformDirection(transform.forward), -Camera.main.transform.TransformDirection(transform.up));
                targetRot = Quaternion.identity;
                oldRot = Camera.main.transform.rotation;
                rotatingback = true;
                lerpValue = 0;
            }

            lerpValue += Time.deltaTime * 1.0f;
            //xRotation = Mathf.Lerp(0, Camera.main.transform.up, lerpValue);

            this.transform.rotation = Quaternion.Lerp(oldRot, targetRot, lerpValue);
            //InitialCamera.transform.eulerAngles = new Vector3(xRotation, 0, 0);
            if (lerpValue < 0.98f)
            {
                rotatingback = true;
            }
            else
            {
                this.transform.rotation = Quaternion.identity;
                isOnWall = false;
                lerpValue = 0;
                rotatingback = false;
                PlayerPhysic.useGravity = true;
            }
        }

           
        

    }

    public void ResetPlayer()
    {
        PlayerPhysic.velocity = Vector3.zero;
        PlayerPhysic.angularVelocity = Vector3.zero;
        PlayerPhysic.Sleep();
        IsLanded = true;
        isInAir = true;

        SpeedReduction = 1;
        baseSpeedReduction = SpeedReduction;
        PreviousPos = transform.position;

        Speed = baseSpeed;

        FWDForce = 0;
        LForce = 0;

        ForwardLookingDirection = new Vector3(0, 0, 0);
        MovingDirection = new Vector3(0, 0, 0);


        // float Xrotation; // ROTATION
        // float Yrotation; // ROTATION


        DistanceWalked = new Vector3(0, 0, 0);

        isCrouching = false;
        isRoof = false;
        isInAir = true;
        isRunning = false;
        IsGrounded = false;
        canJump = false;
        IsLanded = true;
    }

    void FixedUpdate()
    {

        UpdateRunning();
        CheckForGround();
        CheckForRoof();
        if (!rotating)
        {
            UpdatePlayerMovement();
            UpdateStance();
        }
        

    }


   
    void UpdateCameraRotation()
    {
        //print(Input.GetAxis("Mouse X"));
        Xrotation = CameraObject.transform.eulerAngles.y + Input.GetAxis("Mouse X") * Xsensitivity;
        Yrotation = CameraObject.transform.eulerAngles.x - Input.GetAxis("Mouse Y") * Ysensitivity;

        //if (CameraObject.transform.localEulerAngles.x >= 270 && CameraObject.transform.localEulerAngles.x <= 360) Yrotation = Mathf.Clamp(Yrotation, 270, 361);
        //else Yrotation = Mathf.Clamp(Yrotation, -1, 90);
        
            CameraObject.transform.eulerAngles = new Vector3(Yrotation, Xrotation, 0);
    }

    void UpdatePlayerMovement()
    {
        if (isOnWall)
        {
            // ForwardLookingDirection = new Vector3(Mathf.Sin(this.transform.eulerAngles.y * Mathf.Deg2Rad), 0, Mathf.Cos(this.transform.eulerAngles.y * Mathf.Deg2Rad));           // Calculate axis depending on player's rotation value
            // LeftLookingDirection = new Vector3(-Mathf.Cos(this.transform.eulerAngles.y * Mathf.Deg2Rad), 0, Mathf.Sin(this.transform.eulerAngles.y * Mathf.Deg2Rad));

            if(transform.rotation.x > -80)
            {
                //     ForwardLookingDirection = new Vector3(0, Mathf.Cos(Xrotation * Mathf.Deg2Rad), Mathf.Sin(Xrotation * Mathf.Deg2Rad));           // Calculate axis depending on player's rotation value
                //  LeftLookingDirection = new Vector3(0, -Mathf.Cos(Xrotation * Mathf.Deg2Rad), Mathf.Sin(Xrotation * Mathf.Deg2Rad));
                //  MovingDirection = LeftLookingDirection * LForce + ForwardLookingDirection * FWDForce;


                //PlayerPhysic.MoveRotation(PlayerPhysic.rotation * Quaternion.Euler(new Vector3(0, Input.GetAxis("Mouse X") * MouseSensitivity, 0)));
                PlayerPhysic.MovePosition(transform.position + (transform.forward * FWDForce * 0.1f ) + (transform.right * -LForce * 0.1f));
            }
        }
        else
        {
            ForwardLookingDirection = new Vector3(Mathf.Sin(Xrotation * Mathf.Deg2Rad), 0, Mathf.Cos(Xrotation * Mathf.Deg2Rad));           // Calculate axis depending on player's rotation value
            LeftLookingDirection = new Vector3(-Mathf.Cos(Xrotation * Mathf.Deg2Rad), 0, Mathf.Sin(Xrotation * Mathf.Deg2Rad));

            MovingDirection = LeftLookingDirection * LForce + ForwardLookingDirection * FWDForce;
        }


                // Calculate moving direction

       // if (IsGrounded)
       if(!isOnWall && IsGrounded)
            PlayerPhysic.velocity += Speed * (Vector3.Normalize(MovingDirection)) * SpeedReduction;         // Apply physic force to the player's rigidbody in the moving direction

        if (!isCrouching && !isInAir && !isRoof && !isRunning) Speed = InitialSpeed;          // If standing still, reset to default speed



        FWDForce = 0;           // Reset movement values
        LForce = 0;

        if (Input.GetKey(Forward)) FWDForce += 1;             // If control key pressed, apply associated movement value
        if (Input.GetKey(Backward)) FWDForce += -1;
        if (Input.GetKey(Left)) LForce += 1;
        if (Input.GetKey(Right)) LForce += -1;



        DistanceWalked = transform.position - PreviousPos;

        if (DistanceWalked.magnitude > 1.8f && !isInAir)
        {
            PreviousPos = transform.position;

        }

    }

    void UpdateStance()
    {
        if (Input.GetKey(Crouch) && hitUp.point.y - transform.TransformPoint(playerCapsuleCollider.center).y >= 1f || Input.GetKey(Crouch) && !isRoof)
        {
            isCrouching = true;
        }

        else if (!isRoof && isCrouching && !isInAir || Input.GetKeyUp(Crouch) && !isRoof && !isInAir)
        {
            isCrouching = false;
        }
    }

    void UpdateColliderSize()
    {
        if (isCrouching)
        {
            Speed = CrouchingSpeed;

            CrouchingOffset = -0.1f;

            playerCapsuleCollider.height = 1.5f;
            playerCapsuleCollider.center = new Vector3(0, -0.25f, 0);
        }

        else
        {
            if (!isRunning) Speed = InitialSpeed;

            CrouchingOffset = 0.6f;

            playerCapsuleCollider.height = 2;
            playerCapsuleCollider.center = new Vector3(0, 0, 0);
        }

        Vector3 DesiredPosition = new Vector3(0, CrouchingOffset, 0);

        if (InitialCamera.transform.localPosition != DesiredPosition) InitialCamera.transform.localPosition = Vector3.Lerp(InitialCamera.transform.localPosition, DesiredPosition, Time.deltaTime * 10f);

    }

    void UpdateRunning()
    {

        if (Input.GetKey(Run) && !isCrouching && MovingDirection != new Vector3(0, 0, 0))
        {
            isRunning = true;
            Speed = RunningSpeed;
        }

        else isRunning = false;

        //if (Input.GetKeyDown(Run) && !isRoof)
        //{
        //    if (isCrouching) isCrouching = false;
        //}

        if (isRunning) FOV = Mathf.Lerp(FOV, RunningFOV, Time.deltaTime * 5f);
        else FOV = Mathf.Lerp(FOV, InitialFOV, Time.deltaTime * 5);
    }

    void InitializeTweakVariable()
    {

        Physics.gravity = new Vector3(0, -9.81f, 0);
        baseSpeedReduction = SpeedReduction;
        // Xsensitivity = 1 + 3 * PlayerPrefs.GetFloat("Sensitivity");
        // Ysensitivity = 1 + 3 * PlayerPrefs.GetFloat("Sensitivity");

        //InitialFOV = 90.0f;
        //FOV = InitialFOV;
        //RunningFOV = FOV * 1.3f;

        //SpeedReduction = 1;
        //InitialSpeed = 0.75f;
        //Speed = InitialSpeed;
        //RunningSpeed = Speed * 1.9f;
        //CrouchingSpeed = Speed / 2;

        PreviousPos = transform.position;

    }

    void CheckForGround()
    {

        if (IsGrounded)
        {
            if (!IsLanded)
            {
                IsLanded = true;
            }
            SpeedReduction = baseSpeedReduction;
            canJump = true;
            isInAir = false;
            PlayerPhysic.useGravity = false;
            PlayerPhysic.drag = 10f;

        }

        else
        {

            IsLanded = false;
            canJump = false;

            PlayerPhysic.drag = 0f;
            isInAir = true;
            if (!rotating && !isOnWall)
            {
                PlayerPhysic.useGravity = true;
            }

            RegulateVelocity();

            PlayerPhysic.velocity += new Vector3(MovingDirection.x * 0.3f, 0, MovingDirection.z * 0.3f) * SpeedReduction;

        }

        if (Input.GetKey(Jump) && canJump)
        {
            SpeedReduction = SpeedReduction / 4.0f;
            canJump = false;
 
            if (!isRoof) isCrouching = false;

            PlayerPhysic.velocity += new Vector3(0, 3f * JumpForce, 0);            //This line is called every spacebar pressed

        }

    }

    void RegulateVelocity()       // Clamp the vertical velocity of the player
    {
       
        float x = Mathf.Clamp(PlayerPhysic.velocity.x, -5f, 5f);
        float y = Mathf.Clamp(PlayerPhysic.velocity.y, -5f, 5f);
        float z = Mathf.Clamp(PlayerPhysic.velocity.z, -5f, 5f);

        PlayerPhysic.velocity = new Vector3(x, y, z);

    }

    void CheckForRoof()
    {
        if (Physics.SphereCast(transform.TransformPoint(playerCapsuleCollider.center), 0.49f, Vector3.up, out hitUp, 0.75f, 1 << 9)) isRoof = true;           // If ray detects object, there is a roof
        else isRoof = false;
    }

    public void UpdateCrouch()
    {
        isCrouching = isRoof;
    }

    void OnCollisionStay(Collision CollisionInfo)
    {

        foreach (ContactPoint con in CollisionInfo.contacts)
        {

            Vector3 SphereCenter = transform.TransformPoint(playerCapsuleCollider.center) + (playerCapsuleCollider.height / 2 - 0.26f) * Vector3.down;

            //  if (con.point.y < SphereCenter.y && CollisionInfo.gameObject.layer == 9) IsGrounded = true;
            if (con.point.y < SphereCenter.y) IsGrounded = true;


        }

    }

    private void OnCollisionExit(Collision collision)
    {
        //foreach (ContactPoint con in collision.contacts)
        //{
        //    Vector3 SphereCenter = transform.TransformPoint(playerCapsuleCollider.center) + (playerCapsuleCollider.height / 2 - 0.26f) * Vector3.down;
        //    if (con.point.y < SphereCenter.y) IsGrounded = false;
        //}
        // if (collision.gameObject.layer == 9) IsGrounded = false;
        IsGrounded = false;


        // IsGrounded = false;
    }


}