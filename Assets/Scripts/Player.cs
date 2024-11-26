using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Components")]
    InputActions inputActions;
    private Rigidbody2D rb;



    [Header("Movement")]
    private Vector2 moveInput;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;



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



    [Header("Tools")]
    [SerializeField] private float destroySpeed;
    public float DestroySpeed { get { return destroySpeed; } }

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
        rb = GetComponent<Rigidbody2D>();
        currBatteryWh = maxBatteryWh;
    }

    void Update()
    {
        isGrounded = GroundCheck();
        HandleBattery();
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        float s = isRunning ? speed * 1.5f : speed;
        s *= currBatteryWh <= 0 ? 0 : 1;

        Vector2 move = new Vector2(moveInput.x, 0) * s;
        rb.linearVelocity = new Vector2(move.x, rb.linearVelocity.y);

        //flip sprite by changing scale
        Flip(moveInput.x);
    }

    void Flip(float xInput)
    {
        if (xInput > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (xInput < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    #region Energy

    public void ConsumeEnergyWS(int amountWs)
    {
        currBatteryWh -= amountWs / 3600;
    }

    public void ConsumeEnergyW(int amountW)
    {
        currBatteryWh -= amountW;
    }

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
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    bool GroundCheck()
    {
        return Physics2D.Raycast(transform.position + Vector3.up * rayOffset, Vector2.down, rayDistance, groundLayer);
    }

    #endregion

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position + Vector3.up * rayOffset, transform.position + Vector3.up * rayOffset + Vector3.down * rayDistance);
    }
} 