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
    public Vector2 moveDirection;

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

            inputActions.Player.Movement.performed += ctx => moveDirection = ctx.ReadValue<Vector2>();
            inputActions.Player.Jump.performed += ctx => Jump();

            inputActions.Player.Run.started += ctx => isRunning = true;
            inputActions.Player.Run.canceled += ctx => isRunning = false;

            inputActions.Player.Forward.performed += ctx => ToggleForward(true);
            inputActions.Player.Backward.performed += ctx => ToggleForward(false);
        }

        inputActions.Enable();
    }

    void ToggleForward(bool forward)
    {
        if (forward)
        {
            rotatingForward = true;
            rotatingBackward = false;
        }
        else
        {
            rotatingForward = false;
            rotatingBackward = true;
        }
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

    void HandleRotation()
    {
        //smoothly rotate forward if rotatingForward is true
        if (rotatingForward)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0), 0.05f);
            //when the rotation is almost done, set rotatingForward to false
            if (transform.rotation == Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0))
            {
                rotatingForward = false;
            }
        }
        else if (rotatingBackward)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, mainCamera.transform.eulerAngles.y + 180, 0), 0.05f);
            //when the rotation is almost done, set rotatingBackward to false
            if (transform.rotation == Quaternion.Euler(0, mainCamera.transform.eulerAngles.y + 180, 0))
            {
                rotatingBackward = false;
            }
        }
    }

    void Update()
    {
        isGrounded = GroundCheck();
        HandleRotation();
        HandleBattery();
    }

    void FixedUpdate()
    {
        float s = isRunning ? speed * 2 : speed;
        //apply velocity relative to character rotation
        //Mathf.Abs(moveDirection.y) pra quando ele andar pra tras ele nao ir de ré, mas sim virar e andar pra tras

        rb.velocity = transform.TransformDirection(new Vector3(moveDirection.x * s, rb.velocity.y, Mathf.Abs(moveDirection.y) * s));
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