using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Electrical
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
    //consumption
    [SerializeField] private int baseConsumptionWs;
    [SerializeField] private int walkingConsumptionWs;
    [SerializeField] private int runningConsumptionWs;
    [SerializeField] private int jumpConsumptionW;

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

    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("HandleBattery", 1, 1);
    }

    public override void Update()
    {
        base.Update();
        isGrounded = GroundCheck();
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        float s = isRunning ? speed * 1.5f : speed;
        s *= currentBatteryWh <= 0 ? 0 : 1;

        Vector2 move = new Vector2(moveInput.x, 0) * s;
        rb.linearVelocity = new Vector2(move.x, rb.linearVelocity.y);

        if (moveInput.x != 0)
        {
            if (isRunning)
            {
                consumptionWsDict["Running"] = runningConsumptionWs;
                consumptionWsDict.Remove("Walking");
            }
            else
            {
                consumptionWsDict["Walking"] = walkingConsumptionWs;
                consumptionWsDict.Remove("Running");
            }
        }
        else
        {
            consumptionWsDict.Remove("Walking");
            consumptionWsDict.Remove("Running");
        }

        //flip sprite by changing scale
        Flip(moveInput.x);
    }

    void Flip(float xInput)
    {
        if (currentBatteryWh <= 0) return;

        if (xInput > 0 && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (xInput < 0 && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    #region Movement

    private void Jump()
    {
        if (isGrounded && currentBatteryWh > jumpConsumptionW)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            ConsumeEnergyW(jumpConsumptionW);
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