using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

    private bool isRunning;
    public Vector2 moveDirection;


    [Header("Ground Raycast")]
    private bool isGrounded;
    [SerializeField] private float rayOffset;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask groundLayer;

    void OnEnable()
    {
        if (inputActions == null)
        {
            inputActions = new InputActions();
            inputActions.Player.Movement.performed += ctx => moveDirection = ctx.ReadValue<Vector2>();
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
    }

    void Update()
    {
        isGrounded = GroundCheck();

        //rotate towards camera look direction when start moving
        if (moveDirection.y > 0)
        {
            transform.rotation = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0);
        }
    }

    void FixedUpdate()
    {
        float s = isRunning ? speed * 2 : speed;
        //rb.velocity = new Vector3(moveDirection.x * s, rb.velocity.y, moveDirection.y * s);
        //apply velocity relative to character rotation
        rb.velocity = transform.TransformDirection(new Vector3(moveDirection.x * s, rb.velocity.y, moveDirection.y * s));
    }

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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position + Vector3.up * rayOffset, Vector3.down * rayDistance);
    }
} 