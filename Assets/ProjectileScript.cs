using System.Collections;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public Rigidbody2D RB;
    public float bulletSpeed = 10;
    public float lifetime = 6f;
    public string targetTag;


    void Start()
    {
        StartCoroutine(DestroyAfterTime(lifetime));
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        RB.velocity = transform.right * bulletSpeed;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyScript hazard = other.gameObject.GetComponent<EnemyScript>();

        if (hazard != null)
        {
            hazard.WasShot();
        }

        if (!other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
