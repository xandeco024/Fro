using System.Collections;
using System.Collections.Generic;
using System.Runtime.Versioning;
using UnityEngine;

public class Player : Electrical
{
    [Header("Components")]
    private Rigidbody2D rb;
    private Animator animator;
    private Storage inventory;
    public Storage Inventory { get { return inventory; } }


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
    [SerializeField] private int walkingConsumptionWs;
    [SerializeField] private int runningConsumptionWs;
    [SerializeField] private int jumpConsumptionW;



    void Awake()
    {
        inventory = new Storage();
    }

    public override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        ItemWorld.SpawnItemWorld(new Vector3(transform.position.x + 2, transform.position.y, 0), new Item { itemType = Item.ItemType.smallMetalScrap, amount = 3 });
        ItemWorld.SpawnItemWorld(new Vector3(transform.position.x - 2, transform.position.y, 0), new Item { itemType = Item.ItemType.smallPlasticScrap, amount = 5 });
    }

    public override void Update()
    {
        base.Update();
        isGrounded = GroundCheck();
        Animation();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Animation()
    {
        if (currentBatteryWh > 0)
        {
            if (isWalking || isRunning && moveInput.magnitude > 0) animator.SetBool("walking", true);
            else animator.SetBool("walking", false);
            //if running, set the current animation speed to 1.5
            if (isRunning) animator.SetFloat("speed", 1.5f);
            else animator.SetFloat("speed", 1);
        }
        else 
        {
            animator.SetBool("walking", false);
        }
    }

    #region Movement

    void Movement()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0);

        if (moveInput.magnitude > 0)
        {
            isRunning = Input.GetKey(KeyCode.LeftShift);

            if (isRunning)
            {
                
                AddConsumptionWs("Running", runningConsumptionWs);
                RemoveConsumptionWs("Walking");
                isWalking = false;
            }
            else
            {
                AddConsumptionWs("Walking", walkingConsumptionWs);
                RemoveConsumptionWs("Running");
                isWalking = true;
            }
        }
        else
        {
            RemoveConsumptionWs("Walking");
            RemoveConsumptionWs("Running");
            isWalking = false;
        }

        float s = isRunning ? speed * 1.5f : speed;
        s *= currentBatteryWh <= 0 ? 0 : 1;

        Vector2 move = new Vector2(moveInput.x, 0) * s;
        rb.linearVelocity = new Vector2(move.x, rb.linearVelocity.y);

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



    private void Jump()
    {
        if (isGrounded && currentBatteryWh > 0)
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