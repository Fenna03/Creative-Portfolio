using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f; // Speed of the player movement
    public float jumpForce = 5.0f; // Force of the jump
    public Transform cameraTransform; // Reference to the camera transform
    private Vector2 moveInput;
    private Rigidbody rb;
    private Animator anim;
    private bool isGrounded;

    void Start()
    {
        // Get the Rigidbody and Animator components attached to the player
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();

        anim.SetBool("isRunning", false);
        anim.SetBool("isJumping", false);
        anim.SetBool("isFalling", false);
    }

    void Update()
    {
        // Update the character's rotation and animations
        HandlePlayerDirection();
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        // Apply the movement in FixedUpdate for consistent physics updates
        MovePlayer();
    }

    // Called by Unity Input System when "Move" action is triggered
    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    // Called by Unity Input System when "Jump" action is triggered
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded)
        {
            JumpPlayer();
        }
    }

    private void MovePlayer()
    {
        if (rb != null)
        {
            // Get the camera's forward and right directions
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            // Flatten the camera's forward and right directions to avoid vertical influence
            cameraForward.y = 0;
            cameraRight.y = 0;

            cameraForward.Normalize();
            cameraRight.Normalize();

            // Calculate movement direction relative to the camera
            Vector3 movement = (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
        }
    }

    private void JumpPlayer()
    {
        if (rb != null)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    private void HandlePlayerDirection()
    {
        if (moveInput.magnitude > 0)
        {
            // Calculate movement direction relative to the camera
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            cameraForward.y = 0;
            cameraRight.y = 0;

            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 direction = (cameraForward * moveInput.y + cameraRight * moveInput.x).normalized;

            if (direction != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 700 * Time.deltaTime);
            }
        }
    }

    private void UpdateAnimations()
    {
        // Update running animation
        if (moveInput.magnitude > 0 && isGrounded)
        {
            anim.SetBool("isRunning", true);
        }
        else
        {
            anim.SetBool("isRunning", false);
        }

        // Update jumping and falling animations
        if (!isGrounded)
        {
            if (rb.velocity.y > 0)
            {
                anim.SetBool("isJumping", true);
                anim.SetBool("isFalling", false);
            }
            else if (rb.velocity.y < 0)
            {
                anim.SetBool("isJumping", false);
                anim.SetBool("isFalling", true);
            }
        }
        else
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the player is grounded
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        if (collision.collider.CompareTag("Death"))
        {
            //Destroy(this.gameObject); // Destroy the player
        }
    }
}