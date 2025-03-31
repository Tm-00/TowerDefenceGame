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
    }

    public void ShowPauseCamera()
    {
        playerCamera.enabled = false;
        pauseCamera.enabled = true;
    }    
}
