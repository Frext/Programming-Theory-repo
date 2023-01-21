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
        [SerializeField] private float maxMoveSpeed = 12;
        [Range(0, 1)]
        [SerializeField] private float moveBackwardsMultiplier = 0.75f;
        [Tooltip("This script is needed to get the orientation object automatically.")]
        [SerializeField] private CameraController cameraControllerScript;

        Transform orientation;
        
        float horizontalInput;
        float verticalInput;

        Vector3 moveDirection;

        bool isMovingBackwards;

        Rigidbody playerRb;

        
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

            isMovingBackwards = false;
            
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
            
            // If you are moving backwards, the speed limit is lower than usual.
            if (isMovingBackwards && flatVelocity.magnitude >= maxMoveSpeed * moveBackwardsMultiplier)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * (maxMoveSpeed * moveBackwardsMultiplier);
                
                playerRb.velocity = new Vector3(limitedVelocity.x, playerRb.velocity.y, limitedVelocity.z);
            }
            else if (flatVelocity.magnitude >= maxMoveSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * maxMoveSpeed;
                
                playerRb.velocity = new Vector3(limitedVelocity.x, playerRb.velocity.y, limitedVelocity.z);
            }
        }


        void FixedUpdate()
        {
            UpdatePlayerGroundState();
                
            HandlePlayerDrag();

            MovePlayer();
            
            FallFaster();
            

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
            moveDirection.Normalize();
            
            isMovingBackwards = Vector3.Dot(orientation.forward, moveDirection) < -.5f;

            if (isPlayerGrounded)
            {
                playerRb.AddForce(moveDirection * (maxMoveSpeed * 10f));
            }
            else
            {
                playerRb.AddForce(moveDirection * (maxMoveSpeed * 10f * airMultiplier));
            }
        }
        
        private void FallFaster()
        {
            if (playerRb.velocity.y < -0.01f)
            {
                playerRb.velocity += Vector3.up * (Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime);
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