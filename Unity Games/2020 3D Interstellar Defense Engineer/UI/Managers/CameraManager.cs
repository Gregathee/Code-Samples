using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager cameraManager;
    [SerializeField]  List<CinemachineVirtualCamera> cameras = null;
    [SerializeField] Camera mainCamera = null;
    [SerializeField] float orthoSize = 1;
    float adjustedOrthoSize;
    int state = 0;
    CinemachineVirtualCamera activeCamera = null;
    float maxScrollDelta = 14;
    float minScrollDelta = 5;

	private void Awake()
	{
        cameraManager = this;
	}

	private void Start()
    {
        activeCamera = cameras[0];
        activeCamera.gameObject.SetActive(true);
        adjustedOrthoSize = orthoSize;
    }

    private void Update()
    {
        adjustedOrthoSize += Input.mouseScrollDelta.y * -1;
        if (adjustedOrthoSize > maxScrollDelta) adjustedOrthoSize = maxScrollDelta;
        if (adjustedOrthoSize < minScrollDelta) adjustedOrthoSize = minScrollDelta;
        if (cameras[4].gameObject.activeInHierarchy)
        {
            mainCamera.orthographic = true;
            cameras[4].m_Lens.OrthographicSize = adjustedOrthoSize ;

        }
        else mainCamera.orthographic = false;
    }

    public void ChangeState(int newState)
    {
        if(newState >= 0 && newState < cameras.Count && newState != state) 
        {
            cameras[newState].gameObject.SetActive(true);
            activeCamera.gameObject.SetActive(false);
            activeCamera = cameras[newState];
            state = newState;
        }
    }
}
