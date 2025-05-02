using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraSwitch : MonoBehaviour
{
    public Camera mainCamera;       // Reference to the main camera
    public Camera playerCamera;     // Reference to the player camera
    
    private void Start()
    {
        ResetCameras();
    }

    private void Update()
    {       
        // Check for input to switch cameras (e.g., pressing the "C" key)
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchCameras();
        }  
    }

    private void SwitchCameras()
    {
        // Toggle the active state of the cameras
        mainCamera.enabled = !mainCamera.enabled;
        playerCamera.enabled = !playerCamera.enabled;
    }

    public void ResetCameras()
    {
        mainCamera.enabled = true;
        playerCamera.enabled = false;
    }
}
