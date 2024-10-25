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



    [Header("Movement")]
    private Vector3 up;
    private Vector2 moveInput;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private GameObject planet;



    [Header("Ground Raycast")]
    private bool isGrounded;
    [SerializeField] private float rayOffset;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask groundLayer;



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
    public float CurrBatteryWh { get { return currBatteryWh; }}

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
        HandleBattery();
        IndividualRelativeGravity();
        //HandleRotation();
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

    void IndividualRelativeGravity()
    {
        // Vector3 planetCenter = planet.transform.position;
        // Vector3 playerToPlanet = transform.position - planetCenter;
        // Vector3 up = playerToPlanet.normalized;

        // transform.position = planetCenter + playerToPlanet.normalized * planet.GetComponent<SphereCollider>().radius * 100;
        // transform.rotation = Quaternion.FromToRotation(transform.up, up) * transform.rotation;

        Vector3 planetCenter = planet.transform.position;
        Vector3 playerToPlanet = transform.position - planetCenter;
        up = playerToPlanet.normalized;

        transform.rotation = Quaternion.FromToRotation(transform.up, up) * transform.rotation;

        rb.AddForce(-up * 9.8f, ForceMode.Acceleration);

        //legal, funciono.
    }

    void Movement()
    {
        float s = isRunning ? speed * 2 : speed;
        s *= currBatteryWh <= 0 ? 0 : 1;

        //get the camera forward and move the player in that direction
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        if (moveDirection.magnitude > 0)
        {
            isWalking = true;

            moveDirection = mainCamera.transform.rotation * moveDirection;

            if (moveDirection.magnitude > 0.001f)
            {
                //rb.MovePosition(transform.position + moveDirection * (s * Time.deltaTime));

                transform.position += moveDirection * (speed * Time.deltaTime);
            }
        }

        else
        {
            isWalking = false;
        }

        //rb.velocity = new Vector3(moveDirection.x * s, rb.velocity.y, moveDirection.z * s);
    }

    #region Energy

    public void Charge(float amountWs)
    {
        float charge = amountWs / 3600;
        currBatteryWh = Mathf.Min(currBatteryWh + charge, maxBatteryWh);
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
        if (isGrounded && currBatteryWh > jumpConsumptionWs)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    bool GroundCheck()
    {
        return Physics.Raycast(transform.position + up * rayOffset, -up, rayDistance, groundLayer);
    }

    #endregion

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position + up * rayOffset, -up * rayDistance);
    }
} 