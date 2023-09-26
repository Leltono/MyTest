using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Locomotion : MonoBehaviour
{
    float jumpForce; 
    float movementForce;

    Vector3 input;

    Rigidbody rb;
    PlayerInput playerInput;

    // Start is called before the first frame update
    void Start()
    {
        jumpForce = 250f;
        movementForce = 10f;

        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
    }
    // Update is called once per frame
    void Update()
    {
        input = playerInput.actions["Move"].ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {    
        rb.AddForce(new Vector3(input.x, 0f, input.y) * movementForce );
    }


    public void Jump(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed) 
        { 
            rb.AddForce(Vector3.up * jumpForce);
            Debug.Log("Saltando");
            Debug.Log(callbackContext.phase);
        }
    }

}


