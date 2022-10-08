using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")] 
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody playerRb;


    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private float groundDrag;
    [SerializeField] private LayerMask groundLayer;
    
    
    bool isGrounded;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();

        // If rotation is not frozen, the player falls over
        playerRb.freezeRotation = true;
    }

    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        HandleDrag();
        
        MovePlayer();
        
        ClampSpeed();
    }

    private void HandleDrag()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);

        if (isGrounded)
        {
            playerRb.drag = groundDrag;
        }
        else
        {
            playerRb.drag = 0f;
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        playerRb.AddForce(moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);
    }

    private void ClampSpeed()
    {
        Vector3 flatVelocity = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);

        // If the limit is exceeded
        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;

            playerRb.velocity = new Vector3(limitedVelocity.x, playerRb.velocity.y, limitedVelocity.z);
        }
    }
}