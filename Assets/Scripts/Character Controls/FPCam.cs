using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FPCam : MonoBehaviour
{
   

    [Header("References")]
    public Transform orientation;
    public Transform camHolder;
    public float sensX;
    public float sensY;
    float xRotation;
    float yRotation;

    public Vector3 screenPosition;

    // Add a boolean flag to control camera movement
    private bool canMoveCamera = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
 
        Cursor.visible = false;

        // Use a Coroutine to enable camera movement after one second
        StartCoroutine(EnableCameraMovement());
    }

    private IEnumerator EnableCameraMovement()
    {
        yield return new WaitForSeconds(.1f); // Wait for one second
        canMoveCamera = true; // Allow camera movement
    }

    private void Update()
    {

        if (!canMoveCamera)
        {
            // If canMoveCamera is false, don't respond to mouse input
            return;
        }

        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
       float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;
        
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        //rotate cam and orientation
        camHolder.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        screenPosition = Input.mousePosition;
    }

    public void DoFov(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    }
}
