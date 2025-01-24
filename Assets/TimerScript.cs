using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerScript : MonoBehaviour
{
    public static TimerScript instance {get; private set;}

    [Header("Time Components")]
    private float timeRemaining;
    private float maxTime = 10f;                //40f
    private bool timerRunning = false;
    private float timeMultipler = 1f;

    [Header("Timers In Scene")]
    public TextMeshPro mainTimerText;
    public TextMeshPro playerTimerText;


    //Other Scripts it needs to interact with
    GameObject player;

    private void Awake()
    {
        // Ensure there is only one instance of TimerManager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Make this object persistent
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMoveScript playerID = player.GetComponent<PlayerMoveScript>();

        //Main camera start, if player cam is enabled start timer
        if (playerID.playerCam.enabled && !timerRunning)
        {
            StartTimer(); 
        }

        //Timer has begun and the player is alternating between cameras
        if (timerRunning)
        {
            if (timeRemaining >= 0)
            {
                timeRemaining -= Time.deltaTime * timeMultipler;
                UpdateTimerText(playerTimerText); // Initial update of text
                UpdateTimerText(mainTimerText); // Initial update of text
            }
            else
            {
                GameOverScreen();
            }

            //Player is on playerCamera
            if (playerID.playerCam.enabled)
            {
                playerTimerText.enabled = true;
                mainTimerText.enabled = false;
            }

            //Player is on mainCamera
            if (playerID.mainCam.enabled)
            {
                playerTimerText.enabled = false;
                mainTimerText.enabled = true;
            }
        }
    }

    public void IncreaseTime(int amount)
    {
        timeRemaining += amount;
    }

    public void GameOverScreen()
    {
        //Test if these text will spawn again
        mainTimerText.enabled = false;
        playerTimerText.enabled = false;
        SceneManager.LoadScene("GameOverScreen");
    }

    //Increase CountdownTimerRate
    public void IncreaseTimeRate (float multipler)
    {
        timeMultipler *= multipler;
    }
    public void ResetTimeRate()
    {
        timeMultipler = 1;
    }

    //Increase flow of time
    public void IncreaseFlowOfTime(float multiplier)
    {
        Time.timeScale *= multiplier; 
    }
    public void ResetFlowOfTime()
    {
        Time.timeScale = 1; 
    }

    void StartTimer()
    {
        mainTimerText.enabled = true;
        playerTimerText.enabled = true;
        timerRunning = true;
        timeRemaining = maxTime;  
    }

    public void ResetTimer()
    {
        ResetFlowOfTime();
        ResetTimeRate();
        StartTimer();
    }

    private void UpdateTimerText(TextMeshPro selectedText)
    {
        selectedText.text = "Time Left: " + Mathf.Ceil(timeRemaining).ToString(); // Update text display
    }
}
//THIS NEEDS TO FIND PLAYER IN MAIN SCENE