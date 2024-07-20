using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody rb;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;

    private bool isRunning;
    private Vector3 moveDirection;
    private bool jumpTrigger;

    [Header("Ground Raycast")]
    private bool isGrounded;
    [SerializeField] private float rayOffset;
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        moveDirection = transform.right * x + transform.forward * z;

        if (Input.GetButtonDown("Jump"))
        {
            jumpTrigger = true;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        isGrounded = GroundCheck();
    }

    void FixedUpdate()
    {
        if (jumpTrigger && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpTrigger = false;
        }

        float s = isRunning ? speed * 2 : speed;

        rb.MovePosition(rb.position + moveDirection * s * Time.fixedDeltaTime);
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