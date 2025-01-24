using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraSwitch : MonoBehaviour
{
    public static CameraSwitch instance {get; private set;}
    public Camera mainCamera;       // Reference to the main camera
    public Camera playerCamera;     // Reference to the player camera

    private void Awake()
    {
        // Ensure there is only one instance of TimerManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Make this object persistent
            DontDestroyOnLoad(mainCamera);
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate
        }
    }
    
    private void Start()
    {
        // Ensure only the main camera is active at the start
        mainCamera.enabled = true;
        playerCamera.enabled = false;
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
}
