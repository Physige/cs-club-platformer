using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    // gets character controller
    public CharacterController controller;
    
    // test object for raycast position
    public Transform grappleObject;

    // gets player camera
    public Camera playerCamera;

    // player movement speed
    public float speed = 12f;

    // player grapple speed
    public float grappleSpeed = 24f;

    // gravity constant
    public float gravity = -9.81f;

    // drag to slow down player momentum after grapple
    public float drag = -8;
    
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

    // stores the point at which the grapple hook landed
    private Vector3 grapplePoint;

    // stores what state the player is in i.e. normal movement or doing a grapple shot
    private string playerState = "move";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerState == "grapple") {
            grappleMovement();
        } else {
            playerMovement();
        }

        // if E is pressed, a grapple is shot
        if (Input.GetKeyDown(KeyCode.E)) {
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit raycastHit)) {
                // raycastHit is whatever the raycast hits
                
                // moves test object to where we are looking
                grappleObject.position = raycastHit.point;

                // stores position of grapple
                grapplePoint = raycastHit.point;
                
                // prevents player controlled movement
                playerState = "grapple";
            }
        }
    }

    private void playerMovement() {
        // checks if player is touching the ground
        isOnGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // check if the player is on the ground and is still falling
        if (isOnGround && velocity.y < 0) {
            // resets the player falling velocity (it's -2 because the condition may be called before fully touching ground)
            velocity.y = -2f;

            // resets the player's x, z velocity from the grapple if the player touches the ground
            velocity.x = 0f;
            velocity.z = 0f;
        }

        // dampens player momentum from grapple
        if (velocity.x != 0 || velocity.z != 0) {
            // slows player's x velocity depending on initial direction
            if (velocity.x > 0.1) {
                velocity.x += drag * Time.deltaTime;
            } else if (velocity.x < -0.1) {
                velocity.x -= drag * Time.deltaTime;
            } else {
                velocity.x = 0;
            }

            // slows player's z velocity depending on initial direction
            if (velocity.z > 0.1) {
                velocity.z += drag * Time.deltaTime;
            } else if (velocity.z < -0.1) {
                velocity.z -= drag * Time.deltaTime;
            } else {
                velocity.z = 0;
            }
        }

        // gets A D input (if D, x is 1; if A, x is -1)
        float x = Input.GetAxis("Horizontal");
        // gets W S input (if W, z is 1; if A, z is -1)
        float z = Input.GetAxis("Vertical");

        // gets shift key and handles sprint
        if (Input.GetKeyDown(KeyCode.LeftShift)) {
            speed = 24f;
        } else if (Input.GetKeyUp(KeyCode.LeftShift)) {
            speed = 12f;
        }

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

    private void grappleMovement() {
        // calculates direction of player grapple
        Vector3 grappleDirection = (grapplePoint - transform.position).normalized;
        
        // moves player in direction of grapple
        controller.Move(grappleDirection * grappleSpeed * Time.deltaTime);

        // checks if player reached end of grapple
        if (Vector3.Distance(transform.position, grapplePoint) < 3) {
            playerState = "move";
        }

        // cancels grapple if space is pressed
        if (Input.GetKeyDown(KeyCode.Space)) {
            // carries player grapple momentum over
            velocity = grappleDirection * grappleSpeed;
            playerState = "move";
        }
    }
}
