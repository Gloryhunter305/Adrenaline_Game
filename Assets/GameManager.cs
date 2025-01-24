using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; private set;}

    [Header("Moving to New Spot Variables")]
    public LayerMask layerMask;     //Scans for wall layer
    public GameObject player;       //Going to grab player's radius so spawner won't spawn within player's radius
    public GameObject enemy;        //Grab enemy's radius so it doesn't spawn close to wall OR towards others

    [Header("SpawnManager Variables")]
    public GameObject enemyPrefab;
    public int numberOfTargets = 6;     //Could use this for something...
    public int targetsRemaining;

    [Header("Private variables")]
    [SerializeField] private List<GameObject> activeTargets = new List<GameObject>();
    [SerializeField] private float respawnDelay = 3f;

    [Header("Scoring & Reward Components")]
    public int level = 1;
    public int targetAddTime = 1, levelClearAddTime = 5;
    public float timeMultipler = 1.1f;

    // public GameObject laserBeamPrefab;
    // private int chance = 8;


    //Other Scripts it is interacting with
    public TimerScript timer;

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

        targetsRemaining = numberOfTargets;
        SpawnTargets();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnTargets()
    {
        for (int i = 0; i < numberOfTargets; i++)
        {
            PlayerMoveScript playerScript = player.GetComponent<PlayerMoveScript>();
            EnemyScript enemyScript = enemy.GetComponent<EnemyScript>();

            float playerScanRadius = playerScript.playerNoNoSquare; // Get the player's scan radius
            float enemyRadius = enemyScript.scanRadius;             //Get the enemy's scan Radius
            Vector2 playerPos = player.transform.position;          //Get player's position on the screen
            

            Vector2 spawnPosition = GetRandomPosition(playerPos, playerScanRadius, enemyRadius);
            GameObject target = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            activeTargets.Add(target);
        }
    }


    Vector2 GetRandomPosition(Vector2 playerPosition, float playerScanRadius, float enemyScanRadius) 
    {
        //Dimensions of the game arena
        float randomX = Random.Range(-6, 18.01f);
        float randomY = Random.Range(-5, 4.501f);
        Vector2 spawnPosition = new Vector2(randomX,randomY);

        //If spawnPosition is within the player's scanRadius re-randomize enemy's spawn point
        if (Vector2.Distance(spawnPosition, playerPosition) < playerScanRadius)
        {
            return GetRandomPosition(playerPosition, playerScanRadius, enemyScanRadius);
        }

        //If spawnPosition doesn't overlap with the Walls in the Floor Plan
        if (!IsSafeSpace(spawnPosition, enemyScanRadius))
        {
            return GetRandomPosition(playerPosition, playerScanRadius, enemyScanRadius);
        }

        //If spawnPosition doesn't suit with the enemy's scanRadius **Something Else**

        
        return spawnPosition;
    }
    

    bool IsSafeSpace(Vector2 position, float withinItsRadar)
    {
        // Cast a circle cast around the potential spawn position
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, withinItsRadar, layerMask);

        // Check if there are any hits (i.e., overlap)
        return hits.Length == 0;
    }

    public void TargetHit(GameObject enemy)
    {
        activeTargets.Remove(enemy);
        timer.IncreaseTime(targetAddTime);

        if (activeTargets.Count == 0)
        {
            //If all targets have been destroyed, increase countdown time rate by .1
            timer.IncreaseTimeRate(timeMultipler);

            //Increase level Difficulty
            level++;
            Debug.Log("Level: " + level);
            //Every 5 levels increase speed of player, projectiles etc.
            if (level % 3 == 0)
            {
                timer.IncreaseFlowOfTime(timeMultipler);
                //chance++;
            }
             
            // //Player recieves reward (Laser Beam)
            // if (chance > Random.Range(0, 9))
            // {
            //     Instantiate(laserBeamPrefab, player.transform.position + new Vector3(1.5f, 0, 0), Quaternion.identity);
            // }
            
        
            timer.IncreaseTime(levelClearAddTime);
            StartCoroutine(RespawnTargets());
        }
    }

    public void ResetScore()
    {
        level = 1;
    }
    private IEnumerator RespawnTargets()
    {
        yield return new WaitForSeconds(respawnDelay);
        targetsRemaining = numberOfTargets;
        SpawnTargets();
    }
}

//THIS NEEDS TO FIND PLAYER IN MAIN SCENE