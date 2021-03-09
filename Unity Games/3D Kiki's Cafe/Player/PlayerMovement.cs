using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 0.15f;
    [SerializeField] float sprintMultiplier = 1.5f;
    [SerializeField] float cameraYRotateSpeed = 2;
    [SerializeField] float cameraXRotateSpeed = 1;
    [SerializeField] float cameraZoomSpeed = 1;
    [SerializeField] float maxRotateCameraAngle = 10;
    [SerializeField] float minRotateCameraAngle = 80;
    [SerializeField] float maxCameraZoom = -5;
    [SerializeField] float minCameraZoom = -20;
    [SerializeField] GameObject playerBody = null;
    [SerializeField] GameObject cameraHandle = null;
    [SerializeField] new GameObject camera = null;
    bool strafe = false;
    bool sprint = false;
    
    public void Move()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button10) || Input.GetKeyDown(KeyCode.LeftShift)) { sprint = !sprint;}
        if (Input.GetKeyDown(KeyCode.Joystick1Button11) || Input.GetKeyDown(KeyCode.LeftControl)) { strafe = !strafe;}
        PositionCamera();
        MovePosition();
    }

    void PositionCamera()
    {
        //rotate camera based on input
        Vector3 localRotation = cameraHandle.transform.localEulerAngles;
        localRotation.y -= cameraYRotateSpeed * Input.GetAxis("RotateCameraY") * Time.deltaTime;
        localRotation.x += cameraXRotateSpeed * Input.GetAxis("RotateCameraX") * Time.deltaTime;
        localRotation.x = Mathf.Clamp(localRotation.x, minRotateCameraAngle, maxRotateCameraAngle);
        cameraHandle.transform.localEulerAngles = localRotation;
        
        //move camera based on input
        Vector3  localPosition = camera.transform.localPosition;
        localPosition.z += cameraZoomSpeed * Input.GetAxis("CameraZoom") * Time.deltaTime;
        localPosition.z = Mathf.Clamp(localPosition.z, minCameraZoom, maxCameraZoom);
        camera.transform.localPosition = localPosition;
    }

    void MovePosition()
    {
        //Get direction player wants to move relative to camera angle on x and z axis
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 localMoveDir = new Vector3(horizontalInput, 0, verticalInput);
        
        //Get direction camera is facing excluding its x rotation to maximise move direction parallel to the floor
        Vector3 localRotation = cameraHandle.transform.localEulerAngles;
        localRotation.x = 0;
        Quaternion quat = new Quaternion();
        quat.eulerAngles = localRotation;
        Vector3 moveDir = (quat * localMoveDir);
        moveDir.y = 0;

        if (strafe)
        {
            Vector3 rotation = cameraHandle.transform.localEulerAngles;
            rotation.x = 0;
            playerBody.transform.localEulerAngles = rotation;
        }
        else
        {
            //Move player and face player in direction of movement
            if (moveDir != Vector3.zero) playerBody.transform.forward = moveDir;
        }

        float speed = moveSpeed;
        if (sprint) speed *= sprintMultiplier;
        transform.Translate(moveDir.normalized * speed * Time.deltaTime);
    }
    
}
