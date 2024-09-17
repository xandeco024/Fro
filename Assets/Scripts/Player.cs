using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Cinemachine.CinemachineFreeLook freeLookCamera;
    [SerializeField] private Camera mainCamera;

    InputActions inputActions;
    private Rigidbody rb;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    public Vector2 moveInput;

    [Header("Ground Raycast")]
    private bool isGrounded;
    [SerializeField] private float rayOffset;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask groundLayer;

    private bool rotatingForward = false, rotatingBackward = false;




    [Header("Wachu doin?")]
    private bool isWalking;
    private bool isRunning;
    private bool isJumping;




    [Header("Energy")]
    private bool charging;
    public bool Charging { get { return charging; } }
    [SerializeField] private float maxBatteryWh;
    public float MaxBatteryWh { get { return maxBatteryWh; } }
    [SerializeField] private float currBatteryWh;
    public float CurrBatteryWh { get { return currBatteryWh; } }

    //consumption
    [SerializeField] private int baseConsumptionWs;
    [SerializeField] private int walkingConsumptionWs;
    [SerializeField] private int runningConsumptionWs;
    [SerializeField] private int jumpConsumptionWs;

    private int currentConsumptionWs() {
        int consumption = baseConsumptionWs;

        if (isRunning)
        {
            consumption += runningConsumptionWs;
        }

        else if (isWalking)
        {
            consumption += walkingConsumptionWs;
        }

        if (isJumping)
        {
            consumption += jumpConsumptionWs;
        }

        return consumption;
    }

    public int CurrentConsumptionWs { get { return currentConsumptionWs(); } }

    private bool uncharged;

    void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new InputActions();

            inputActions.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
            inputActions.Player.Jump.performed += ctx => Jump();

            inputActions.Player.Run.started += ctx => isRunning = true;
            inputActions.Player.Run.canceled += ctx => isRunning = false;
        }

        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        currBatteryWh = maxBatteryWh;
    }

    void Update()
    {
        isGrounded = GroundCheck();
        HandleRotation();
        HandleBattery();
    }

    void FixedUpdate()
    {
        Movement();
    }

    void HandleRotation()
    {
        Vector3 targetDirection = Vector3.zero;

        targetDirection = mainCamera.transform.forward * moveInput.y;
        targetDirection += mainCamera.transform.right * moveInput.x;
        targetDirection.Normalize();
        targetDirection.y = 0;

        if (targetDirection == Vector3.zero)
        {
            targetDirection = Vector3.forward;
        }

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);  

        transform.rotation = playerRotation;
    }

    void Movement()
    {
        float s = isRunning ? speed * 2 : speed;

        //get the camera forward and move the player in that direction
        Vector3 moveDirection = mainCamera.transform.forward * moveInput.y + mainCamera.transform.right * moveInput.x;
        moveDirection.Normalize();
        moveDirection.y = 0;

        rb.velocity = new Vector3(moveDirection.x * s, rb.velocity.y, moveDirection.z * s);
    }

    #region Energy

    public void ReceiveEnergy(float energy)
    {

    }

    void HandleBattery()
    {
        float consumptionWh = (currentConsumptionWs() * Time.deltaTime / 3600) * 60;
        
        currBatteryWh -= consumptionWh;

        if (currBatteryWh < 0 && !uncharged)
        {
            uncharged = true;
            currBatteryWh = 0;
        }


        if (uncharged)
        {
            Debug.Log("You are dead");
        }
    }

    #endregion

    #region Movement

    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    bool GroundCheck()
    {
        return Physics.Raycast(transform.position + Vector3.up * rayOffset, Vector3.down, rayDistance, groundLayer);
    }

    #endregion

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position + Vector3.up * rayOffset, Vector3.down * rayDistance);
    }
} 