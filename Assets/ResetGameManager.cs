using UnityEngine;

public class ResetGameManager : MonoBehaviour
{
    public static ResetGameManager instance;

    public int levelCount;      //Store Gamemanager's count in here

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }   
        else
        {
            Destroy(gameObject);    //Destroy existing managers like this
        }
    }

    public void getLevel()
    {
        GameManager GM = FindFirstObjectByType<GameManager>();

        levelCount = GM.level;
    }

    public void ResetGame()
    {
        //Find all managers used in the scene and create a resetFunction for all of them
        var camera = FindFirstObjectByType<CameraSwitch>();
        if (camera != null)
        {
            camera.ResetCameras();
        }
        var manager = FindFirstObjectByType<GameManager>();
        if (manager != null)
        {
            manager.ResetGame();
        }

        var timer = FindFirstObjectByType<TimerScript>();
        if (timer != null)
        {
            timer.RevertTimer();
        }
    }
}
