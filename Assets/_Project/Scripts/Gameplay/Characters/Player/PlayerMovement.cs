using System.Collections;
using _Project.Scripts.Gameplay.Managers.SFX;
using UnityEngine;
using UnityEngine.Events;

namespace _Project.Scripts.Gameplay.Characters.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Ground Movement")]
        [Range(0, 30)] [SerializeField] private float maxMoveSpeed = 12;
        [Range(0, 1)] [SerializeField] private float moveBackwardsMultiplier = 0.75f;
        [SerializeField] private Transform orientationTransform;

        [Space]
        [Tooltip("This is for ground movement, not jumping or etc.")]
        [SerializeField] private UnityEvent onMovementStart;
        
        [Space]
        [SerializeField] private UnityEvent onMovementStop;

        float horizontalInput;
        float verticalInput;
        Vector3 moveDirection;
        bool isMovingBackwards;
        Rigidbody playerRb;

        
        [Header("Jump")]
        [Range(0, 20)] [SerializeField] private float jumpForce;
        [Range(0, 5)] [SerializeField] private float jumpCooldown;
        [Range(0, 10)] [SerializeField] private float fallMultiplier;
        [Range(0, 10)] [SerializeField] private float airMultiplier;

        bool isReadyToJump;


        [Header("Ground Check")]
        [Range(0,10)] [SerializeField] private float jumpRangeFromGround;
        [Range(0,20)] [SerializeField] private float groundDrag;
        [SerializeField] private LayerMask groundLayer;

        bool isPlayerGrounded;

        
        [Header("Animation")] 
        [SerializeField] private Animator playerAnimator;
        [SerializeField] private string animXVelocityFloatParam = "MoveX";
        [SerializeField] private string animZVelocityFloatParam = "MoveZ";
        [SerializeField] private string animIsGroundedBoolParam = "isGrounded";

        const float ANIM_LERP_MULTIPLIER = 8.9f;

        
        void Start()
        {
            isMovingBackwards = false;

            playerRb = GetComponent<Rigidbody>();
            
            // If rotation is not frozen, the player falls over
            playerRb.freezeRotation = true;
            
            
            isReadyToJump = true;
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

                // This lets you jump repeatedly when you hold the jump button.
                StartCoroutine(IResetJumpState());
            }
        }
		
		private void Jump()
        {
            // Reset the y velocity to jump the exact same amount every time
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
            // That means you move slower than usual.
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
                
            UpdatePlayerDrag();

            MovePlayer();
            
            UpdateMovingBackwardsState();
            
            FallFaster();
            
            
            InvokeMovementEvents();

            UpdateMovementAnimation();
        }

        private void UpdatePlayerGroundState()
        {
            isPlayerGrounded = Physics.Raycast(transform.position, Vector3.down, 
                jumpRangeFromGround + 0.15f, groundLayer) && playerRb.velocity.y is < 0.01f and > -0.01f;
        }

        private void UpdatePlayerDrag()
        {
            playerRb.drag = isPlayerGrounded ? groundDrag : 0f;
        }

        private void MovePlayer()
        {
            moveDirection = orientationTransform.forward * verticalInput + orientationTransform.right * horizontalInput;
            moveDirection.Normalize();

            if (isPlayerGrounded)
            {
                playerRb.AddForce(moveDirection * (maxMoveSpeed * 10f));
            }
            else
            {
                playerRb.AddForce(moveDirection * (maxMoveSpeed * 10f * airMultiplier));
            }
        }

        private void UpdateMovingBackwardsState()
        {
            isMovingBackwards = Vector3.Dot(orientationTransform.forward, moveDirection) < -.5f;
        }

        private void FallFaster()
        {
            if (playerRb.velocity.y < -0.01f)
            {
                playerRb.velocity += Vector3.up * (Physics.gravity.y * fallMultiplier * Time.fixedDeltaTime);
            }
        }

        private void InvokeMovementEvents()
        {
            if (playerRb.velocity.magnitude > 0.75f && isPlayerGrounded)
            {
                onMovementStart.Invoke();
            }
            else
            {
                onMovementStop.Invoke();
            }
        }

        private void UpdateMovementAnimation()
        {
            playerAnimator.SetBool(animIsGroundedBoolParam, isPlayerGrounded);
            
            // The velocity lerp lets us have a smoother transition between the animation states.
            // The additions in the second lerp argument lets us have the velocity by direction,
            // therefore we can set the animation parameters accurately.
            playerAnimator.SetFloat(animXVelocityFloatParam, 
                Mathf.Lerp(playerAnimator.GetFloat(animXVelocityFloatParam) ,
                    playerRb.velocity.x * orientationTransform.right.x + playerRb.velocity.z * orientationTransform.right.z, 
                    ANIM_LERP_MULTIPLIER * Time.fixedDeltaTime));
            
            playerAnimator.SetFloat(animZVelocityFloatParam, 
                Mathf.Lerp(playerAnimator.GetFloat(animZVelocityFloatParam) ,
                    playerRb.velocity.z * orientationTransform.forward.z + playerRb.velocity.x * orientationTransform.forward.x,
                    ANIM_LERP_MULTIPLIER * Time.fixedDeltaTime));
        }

        #region Methods Used By Other Scripts

        public void RigidbodyDie()
        {
            playerRb.constraints = RigidbodyConstraints.FreezePosition;
        }

        #endregion
    }
}