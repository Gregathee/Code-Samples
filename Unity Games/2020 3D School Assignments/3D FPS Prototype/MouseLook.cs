/* * (Greg Brandt) 
 * * (Assignment 5) 
 * * Moves camera with mouse
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivitiy = 100f;
    public GameObject player;
    float verticalLookRotation = 0f;

	private void OnApplicationFocus(bool focus)
	{
        Cursor.lockState = CursorLockMode.Locked;
	}

	void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivitiy * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivitiy * Time.deltaTime;

        player.transform.Rotate(Vector3.up * mouseX);

        verticalLookRotation -= mouseY;

        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
    }
}
