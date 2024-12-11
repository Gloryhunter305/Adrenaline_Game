using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.Build.Content;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{

    [Header("Enemy Components")]
    public float scanRadius = 0.8f;

    [Header("Player Targetting")]
    public Transform target;
    public float shootingRange = 10f;
    public float detectionDistance = 15f;
    public float rotationSpeed = 5f;
    public ProjectileScript bulletPrefab;

    [Header("Spawn Manager")]
    [SerializeField] private GameManager targetSpawner;

    [Header("Timer")]
    public float timer;
    public LayerMask layerMask;     //Scans for wall layer


    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Wall");

        //Enemy will interact with GameManager
        targetSpawner = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        
    }

    private void AimAtTarget(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Smoothly rotate towards the player
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

    }

    private bool CanSeePlayer(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, shootingRange);
        
        // Check if the raycast hit something
        if (hit.collider != null)
        {
            // If the hit object is the player, return true
            if (hit.collider.transform == target)
            {
                return true;
            }
        }
        
        // If the raycast hit something else, return false
        return false;
    }

    private void ShootAtPlayer()
    {
       timer += Time.deltaTime;

       if (timer > 2)
       {
            timer = 0;
            Instantiate(bulletPrefab, transform.position, Quaternion.identity);
       }
    }

    public void WasShot()
    {  
        targetSpawner.TargetHit(gameObject);
        Destroy(gameObject); 
    }

    void OnDrawGizmosSelected()
    {
        //Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, scanRadius);
    }
}
