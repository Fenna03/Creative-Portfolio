using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float walkSpeed = 7f;
    [SerializeField] float jumpPower = 5f;

    Vector2 moveInput;

    public bool isGrounded;
    public bool isJumping;

    public int jumpAmount;

    Rigidbody rb;
    Animator anim;

    void Start()
    {
        //gets the needed components
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();

        anim.SetBool("isRunning", false);
        anim.SetBool("isJumping", false);
        anim.SetBool("isFalling", false);
    }

    void Update()
    {
        //so you can run and turn
        Run();
        RotateTowardsMovementDirection();

        if (moveInput.sqrMagnitude < 0.01f && isGrounded) // Check if there's no input and the player is on the ground
        {
            anim.SetBool("isRunning", false);
        }
    }

    //how to run
    void Run()
    {
        // Create velocity in world space using movement input
        Vector3 playerVelocity = new Vector3(moveInput.x * walkSpeed, rb.velocity.y, moveInput.y * walkSpeed);

        // Apply the velocity to the Rigidbody
        rb.velocity = transform.TransformDirection(playerVelocity);
    }

    void RotateTowardsMovementDirection()
    {
        // Calculate the direction vector based on the movement input
        Vector3 movementDirection = new Vector3(moveInput.x, 0f, moveInput.y);

        if (movementDirection.sqrMagnitude > 0.01f) // Check if there's significant movement input
        {
            // Convert the movement direction to world space
            Vector3 worldDirection = transform.TransformDirection(movementDirection);

            if (moveInput.y >= 0)
            {
                // Compute the target rotation based on the movement direction
                Quaternion targetRotation = Quaternion.LookRotation(worldDirection, Vector3.up);

                // Smoothly rotate the character to face the target direction
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 7f);
            }

        }
    }


    //how to actually move when holding button
    public void OnMovement(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (isGrounded == true)
        {
            anim.SetBool("isRunning", true);
        }
    }

    //jumping when holding button
    public void OnJump(InputAction.CallbackContext context)
    {
        if (jumpAmount <= 1)
        {
            if (isGrounded)
            {
                jumpAmount++;
                anim.SetBool("isJumping", true);
                anim.SetBool("isRunning", false);
                if (jumpAmount >= 1)
                {
                    anim.SetBool("isRunning", false);
                    anim.SetBool("isFalling", true);
                }
                rb.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
            }
        }
    }

    //when grounded
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            jumpAmount = 0;
            isGrounded = true;
            anim.SetBool("isFalling", false);
            anim.SetBool("isJumping", false);
        }
    }

    //when not grounded
    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
