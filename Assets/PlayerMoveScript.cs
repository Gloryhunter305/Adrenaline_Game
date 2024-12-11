using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class PlayerMoveScript : MonoBehaviour
{
    public static PlayerMoveScript Player;

    [Header("Camera_Identification")]
    public Camera mainCam;
    public Camera playerCam;

    [Header("Player Components")]
    public Rigidbody2D RB;
    public float normalSpeed, fastSpeed;
    public float playerNoNoSquare = 2f;
    public ProjectileScript BulletPrefab;

    // [Header("Stamina Component")]
    // public float maxStamina;
    // public float currentStamina;
    // public float staminaLossRate;
    // public float staminaGainRate;
    // public bool noStaminaLeft;
    //[SerializeField] StaminaBarScript staminaBar;

    //Other Script to interact with (due to physical contact with targets)
    public TimerScript timer;
    public ProjectileScript projectile;

    private void Awake()
    {
        Player = this;
    }

    void Start()
    {
        // staminaBar = GetComponentInChildren<StaminaBarScript>();
        // currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player camera is active
        if (playerCam.enabled)
        {
            //UI Functions
            //UIStaminaBar();
            

            //Player Functions
            MovePlayer();
            PlayerShoot();
        }
        else
        {
            RB.velocity = Vector2.zero;
        }
    }

    private void MovePlayer()
    {
        Vector2 vel = new Vector2(0,0);
        if (Input.GetKey(KeyCode.D))
        {
            vel.x = normalSpeed;
        }
        if (Input.GetKey(KeyCode.A))
        {
            vel.x = -normalSpeed;
        }
        if (Input.GetKey(KeyCode.W))
        {
            vel.y = normalSpeed;
        }
        if (Input.GetKey(KeyCode.S))
        {
            vel.y = -normalSpeed;
        }

        // //Stamina Consumption
        // if (Input.GetKey(KeyCode.LeftShift) && !noStaminaLeft)
        // {
        //     if (currentStamina > 0)
        //     {
        //         ReduceStamina(staminaLossRate);
        //         RB.velocity = vel * fastSpeed;
        //     }
        //     else
        //     {
        //         noStaminaLeft = true;
        //     }
        // }
        // else if (Input.GetKey(KeyCode.LeftShift) && noStaminaLeft)
        // {
        //     RB.velocity = vel * normalSpeed;
        // }
        // else
        // {
           
            
        // }
    
        //Fixes bug when moving diagonal, player's speed increases
        if (vel.magnitude > 1)
        {
            vel.Normalize();
        }
            
        RB.velocity = vel * normalSpeed;
    }

    private void PlayerShoot()
    {
        //If I click, shoot!
        if (Input.GetMouseButtonDown(0))
        {
            //Okay, but where am I aiming? Let's find out where the mouse cursor is
            Vector3 pos = playerCam.ScreenToWorldPoint(Input.mousePosition);
            //This little bit of math calculates what direction the bullet should
            //  aim to be facing at the mouse cursor. Don't sweat the details
            float angle = Mathf.Atan2(pos.y-transform.position.y, pos.x-transform.position.x) * Mathf.Rad2Deg;
            //Spawn the projectile where the player is, and give it a Z-rotation of the above
            Instantiate(BulletPrefab, transform.position, Quaternion.Euler(0, 0, angle));
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        EnemyScript enemy = other.gameObject.GetComponent<EnemyScript>();
        
        if (enemy != null)
        {
            enemy.WasShot();
            timer.IncreaseTime(1);  //Didn't create a variable for this...
        }
    }

    // public void ReduceStamina(float amount)
    // {
    //     currentStamina -= amount * Time.deltaTime;
    //     staminaBar.UpdateStaminaBar(currentStamina, maxStamina);
    // }
    // public void RestoreStamina(float amount)
    // {
    //     currentStamina += amount * Time.deltaTime;
    //     staminaBar.UpdateStaminaBar(currentStamina, maxStamina);
    // }

    // void UIStaminaBar()
    // {
    //     if (currentStamina >= maxStamina)
    //     {
    //         currentStamina = maxStamina;
    //     }
    //     else if (currentStamina <= 0)
    //     {
    //         currentStamina = 0;
    //     }

    //     staminaBar.UpdateStaminaBar(currentStamina, maxStamina);
    // }

    void OnDrawGizmosSelected()
    {
        //Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, playerNoNoSquare);
    }


}
