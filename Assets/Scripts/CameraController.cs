using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // mouse sensitivity
    [SerializeField]
    private float sensitivity = 100f;

    // gets player gameObject
    public Transform playerBody;

    // rotation of camera pivioting on the x-axis (up and down)
    private float xRotation = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // locks cursor on screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // gets the mouse x position
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        
        // gets the mouse y position
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // rotates on x-axis based on mouse y position
        xRotation -= mouseY;

        // prevents camera from rotating fully 360 up and down
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // applies up and down rotation
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // rotates player left and right
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
