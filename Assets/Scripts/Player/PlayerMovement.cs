using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform orientation;

    [Space] 
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;

    bool isReadyToJump = true;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody playerRb;


    [Header("Ground Check")] 
    [SerializeField] private float playerHeight;

    [SerializeField] private float groundDrag;
    [SerializeField] private LayerMask groundLayer;

    bool isPlayerGrounded;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();

        // If rotation is not frozen, the player falls over
        playerRb.freezeRotation = true;
    }

    void Update()
    {
        GetInput();

        // If this is called in FixedUpdate, the speed limit can still be exceeded.
        ClampSpeed();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetButton("Jump") && isReadyToJump && isPlayerGrounded)
        {
            isReadyToJump = false;

            Jump();

            // This lets you jump if you hold the jump button. 
            StartCoroutine(IResetJumpState());
        }
    }

    void FixedUpdate()
    {
        HandleDrag();

        MovePlayer();
    }

    private void HandleDrag()
    {
        isPlayerGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, groundLayer);

        playerRb.drag = isPlayerGrounded ? groundDrag : 0f;
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (isPlayerGrounded)
        {
            playerRb.AddForce(moveDirection.normalized * (moveSpeed * 10f), ForceMode.Force);
        }
        else
        {
            playerRb.AddForce(moveDirection.normalized * (moveSpeed * 10f * airMultiplier), ForceMode.Force);
        }
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

    private void Jump()
    {
        // Reset y velocity to jump the exact amount every time.
        playerRb.velocity = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);

        playerRb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        print("Jump");
    }

    private IEnumerator IResetJumpState()
    {
        yield return new WaitForSeconds(jumpCooldown);

        isReadyToJump = true;
    }
}