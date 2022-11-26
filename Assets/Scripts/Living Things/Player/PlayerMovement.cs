using System.Collections;
using UnityEngine;

namespace Living_Things.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [Range(0, 30)]
        [SerializeField] private float moveSpeed;
        [SerializeField] private Transform orientation;

        [Header("Jump")]
        [Range(0, 20)]
        [SerializeField] private float jumpForce;
        [Range(0, 5)]
        [SerializeField] private float jumpCooldown;
        [Range(0, 10)]
        [SerializeField] private float fallMultiplier;
        [Range(0, 10)]
        [SerializeField] private float airMultiplier;

        bool isReadyToJump;

        float horizontalInput;
        float verticalInput;

        Vector3 moveDirection;

        Rigidbody playerRb;


        [Header("Ground Check")]
        [Tooltip("The game object that the player mesh renderer is in is needed in order to get the height of the player.")]
        [SerializeField] private MeshRenderer playerRenderer;
        [SerializeField] private float groundDrag;
        [SerializeField] private LayerMask groundLayer;

        float playerHeight;

        bool isPlayerGrounded;

        
        void Start()
        {
            isReadyToJump = true;
            
            playerRb = GetComponent<Rigidbody>();
            
            // If rotation is not frozen, the player falls over
            playerRb.freezeRotation = true;
            
            
            // Get the player height by the y scale of the renderer
            playerHeight = playerRenderer.gameObject.transform.localScale.y * 2f;
        }

        void Update()
        {
            GetInput();

            // If this is called in FixedUpdate, the speed limit can still be exceeded.
            ClampSpeed();

            FallFaster();
        }

        private void GetInput()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            if (Input.GetButton("Jump") && isReadyToJump && isPlayerGrounded)
            {
                isReadyToJump = false;

                Jump();

                // This lets you jump again when you hold the jump button.
                StartCoroutine(IResetJumpState());
            }
        }
		
		private void Jump()
        {
            // reset y velocity to jump the exact same amount every time
            playerRb.velocity = new Vector3(playerRb.velocity.x, 0f, playerRb.velocity.z);
            
            playerRb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        }

        private IEnumerator IResetJumpState()
        {
            yield return new WaitForSeconds(jumpCooldown);

            isReadyToJump = true;
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

        private void FallFaster()
        {
            if (playerRb.velocity.y < -0.01f)
            {
                playerRb.velocity += Vector3.up * (Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
            }
        }

        void FixedUpdate()
        {
            UpdatePlayerGroundState();
                
            HandlePlayerDrag();

            MovePlayer();
        }

        private void UpdatePlayerGroundState()
        {
            isPlayerGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.15f, groundLayer);
        }

        private void HandlePlayerDrag()
        {
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
    }
}