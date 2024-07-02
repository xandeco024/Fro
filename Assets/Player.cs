using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Scripting.APIUpdating;

public class Player : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private Camera cam;

    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;

    private Vector3 moveDirection;
    private bool jumpTrigger;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        moveDirection = transform.right * x + transform.forward * z;

        if (Input.GetButtonDown("Jump"))
        {
            jumpTrigger = true;
        }

        ControlCamera();
    }

    void FixedUpdate()
    {
        if (jumpTrigger)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpTrigger = false;
        }

        rb.MovePosition(rb.position + moveDirection * speed * Time.fixedDeltaTime);
    }

    void ControlCamera()
    {
        //rotate camera when mouse drag
        if (Input.GetMouseButton(1))
        {
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");

            cam.transform.Rotate(Vector3.right * -y);
        }
    }
} 
