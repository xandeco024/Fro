using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using Unity.VisualScripting;
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
    [SerializeField] private float maxBatteryWh;
    public float MaxBatteryWh { get { return maxBatteryWh; } }
    [SerializeField] private float currentBatteryWh;
    public float CurrBatteryWh { get { return currentBatteryWh; }}

    //consumption
    [SerializeField] private int baseConsumptionWs;
    [SerializeField] private int walkingConsumptionWs;
    [SerializeField] private int runningConsumptionWs;
    [SerializeField] private int jumpConsumptionW;
    private Dictionary<string, int> consumptionWsDict = new Dictionary<string, int>{
        {"Base", 0}
    };
    public Dictionary<string, int> ConsumptionWsDict { get { return consumptionWsDict; } }
    private float currentConsumptionWs;
    public float CurrentConsumptionWs { get { return currentConsumptionWs; } }


    private bool recharging;
    public bool Recharging { get { return recharging; } }
    private float rechargeRateWs;
    public float RechargeRateWs { get { return rechargeRateWs; } }

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
        currentBatteryWh = maxBatteryWh;

        consumptionWsDict["Base"] = baseConsumptionWs;
        InvokeRepeating("HandleBattery", 1, 1);
    }

    void Update()
    {
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

    #region Energy

    public void ConsumeEnergyW(float amountW)
    {
        currentBatteryWh -= amountW;
    }

    public void RechargeW(float amountW)
    {
        currentBatteryWh = Mathf.Min(currentBatteryWh + amountW, maxBatteryWh);
    }

    public void AddConsumption(string key, int value)
    {
        consumptionWsDict[key] = value;
    }

    public void RemoveConsumption(string key)
    {
        consumptionWsDict.Remove(key);
    }

    void HandleBattery()
    {
        currentConsumptionWs = 0;

        foreach (var item in consumptionWsDict)
        {
            currentConsumptionWs += item.Value;
        }

        if (currentBatteryWh > 0)
        {
            ConsumeEnergyW(currentConsumptionWs / 3600 * 60);
        }

        else
        {
            Debug.Log("dead");
            CancelInvoke("HandleBattery");
        }

        if (recharging)
        {
            RechargeW(rechargeRateWs);
        }
    }

    #endregion

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