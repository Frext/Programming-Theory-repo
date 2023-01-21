using System.Collections;
using _Project.Scripts.Camera;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Characters.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [Range(0, 30)]
        [SerializeField] private float maxMoveSpeed;
        [Tooltip("This script is needed to get the orientation object automatically.")]
        [SerializeField] private CameraController cameraControllerScript;

        Transform orientation;

        
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
        [Range(0,10)]
        [SerializeField] private float jumpRangeFromGround;
        [SerializeField] private float groundDrag;
        [SerializeField] private LayerMask groundLayer;

        bool isPlayerGrounded;

        
        [Header("Animation")] 
        [SerializeField] private Animator playerAnimator;
        
        private static readonly int MoveX = Animator.StringToHash("MoveX");
        private static readonly int MoveZ = Animator.StringToHash("MoveZ");
        
        const float ANIM_LERP_MULTIPLIER = 8.9f;

        void Start()
        {
            orientation = cameraControllerScript.GetOrientationObject();
            
            isReadyToJump = true;
            
            
            playerRb = GetComponent<Rigidbody>();
            
            // If rotation is not frozen, the player falls over
            playerRb.freezeRotation = true;
        }

        void Update()
        {
            HandleInput();

            // If this is called in FixedUpdate, the speed limit can still be exceeded a little bit.
            ClampSpeed();

            FallFaster();
        }

        private void HandleInput()
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

            // If the speed limit is exceeded
            if (flatVelocity.magnitude > maxMoveSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * maxMoveSpeed;
                
                playerRb.velocity = new Vector3(limitedVelocity.x, playerRb.velocity.y, limitedVelocity.z);
            }
            // If the player is going backwards, it should move a bit slower.
            else if(verticalInput < -0.01f)
            {
                
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
            
            RefreshAnimator();
        }

        private void UpdatePlayerGroundState()
        {
            isPlayerGrounded = Physics.Raycast(transform.position, Vector3.down, 
                jumpRangeFromGround + 0.15f, groundLayer) && playerRb.velocity.y is < 0.01f and > -0.01f;
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
                playerRb.AddForce(moveDirection.normalized * (maxMoveSpeed * 10f), ForceMode.Force);
            }
            else
            {
                playerRb.AddForce(moveDirection.normalized * (maxMoveSpeed * 10f * airMultiplier), ForceMode.Force);
            }
        }

        private void RefreshAnimator()
        {
            // The velocity lerp lets us have a smoother transition between the animation states.
            // The additions in the second lerp argument lets us have the velocity by direction,
            // therefore we can set the animation parameters accurately.
            
            playerAnimator.SetFloat(MoveX, Mathf.Lerp(playerAnimator.GetFloat(MoveX) ,playerRb.velocity.x * orientation.right.x +
                playerRb.velocity.z * orientation.right.z, ANIM_LERP_MULTIPLIER * Time.fixedDeltaTime));
            
            playerAnimator.SetFloat(MoveZ, Mathf.Lerp(playerAnimator.GetFloat(MoveZ) ,playerRb.velocity.z * orientation.forward.z +
                playerRb.velocity.x * orientation.forward.x, ANIM_LERP_MULTIPLIER * Time.fixedDeltaTime));
        }

        public void RigidbodyDie()
        {
            playerRb.constraints = RigidbodyConstraints.FreezePosition;
        }
    }
}