using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class playermovement : MonoBehaviour
{

    [SerializeField] float walkspeed = 8f;

    Vector2 moveInput;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Run();
    }

    void Run()
    {
        Vector3 playerVelocity = new Vector3(moveInput.x * walkspeed, rb.velocity.y, moveInput.y * walkspeed);
        rb.velocity = transform.TransformDirection(playerVelocity);

    }

    private void OnMovement(InputValue value)
    {
        moveInput = value.Get<Vector2>();
        Debug.Log("test");
    }
}
