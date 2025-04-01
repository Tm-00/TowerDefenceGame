using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{

    [Header("Cameras")] 
    public Camera playerCamera;
    public Camera pauseCamera;
    
    public void ShowPlayerCamera()
    {
        playerCamera.enabled = true;
        pauseCamera.enabled = false;
        Debug.Log("Switching to player camera");
    }

    public void ShowPauseCamera()
    {
        Debug.Log("Switching to pause camera");
        playerCamera.enabled = false;
        pauseCamera.enabled = true;
    }    
}
