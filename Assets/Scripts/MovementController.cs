using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    // gets character controller
    public CharacterController controller;

    // player movement speed
    public float speed = 12f;

    // gravity constant
    public float gravity = -9.81f;
    
    // stores current speed of player
    private Vector3 velocity;

    // how high player jumps
    public float jumpHeight = 3f;

    // gets gameObject that checks if player is touching ground
    public Transform groundCheck;
    // radius groundCheck checks
    public float groundDistance = 0.4f;
    // sets what groundCheck gameObject should check
    public LayerMask groundMask;
    // stores whether or not player is touching the ground
    private bool isOnGround;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // checks if player is touching the ground
        isOnGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // check if the player is on the ground and is still falling
        if (isOnGround && velocity.y < 0) {
            // resets the player falling velocity (it's -2 because the condition may be called before fully touching ground)
            velocity.y = -2f;
        }

        // gets A D input (if D, x is 1; if A, x is -1)
        float x = Input.GetAxis("Horizontal");
        // gets W S input (if W, z is 1; if A, z is -1)
        float z = Input.GetAxis("Vertical");

        // moves player in respect to global direction
        // Vector3 move = new Vector3(x, 0f, z);

        // moves player in respect to relative camera rotation
        Vector3 move = (transform.right * x) + (transform.forward * z);

        // moves the player controller
        controller.Move(move * speed * Time.deltaTime);

        // checks if the space button is pressed and is on the ground
        if (Input.GetButton("Jump") && isOnGround) {
            // gives upwards velocity for jump enough to jump up to specified height (sqrt(height to jump to * gravity * -2))
            velocity.y = Mathf.Sqrt(jumpHeight * gravity * -2);
        }

        // applies gravity onto player's vertical velocity
        velocity.y += gravity * Time.deltaTime;

        // moves player's position based on velocity and makes player fall
        controller.Move(velocity * Time.deltaTime);
    }
}
