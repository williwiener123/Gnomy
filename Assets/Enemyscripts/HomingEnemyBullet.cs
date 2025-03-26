using UnityEngine;
using PlayerHealth;  // Ensure this is added if you're using a namespace

public class HomingEnemyBullet : MonoBehaviour
{
    public float speed = 5f; // Bullet speed
    public float lifetime = 3f; // Bullet disappears after this time
    public int damageAmount = 1; // Damage dealt to the player
    public GameObject destroyEffect; // Assign a destroy effect prefab
    public LayerMask groundLayer; // Set to "Ground" in Inspector
    public float homingStrength = 5f; // Homing strength

    private Transform player; // Reference to player
    private Vector2 moveDirection; // The direction the bullet is moving

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player
        Destroy(gameObject, lifetime); // Destroy bullet after 'lifetime' seconds
    }

    private void Update()
    {
        if (player != null)
        {
            // Move the bullet towards the player
            Vector2 directionToPlayer = (player.position - transform.position).normalized;
            moveDirection = Vector2.Lerp(moveDirection, directionToPlayer, homingStrength * Time.deltaTime);

            transform.position += (Vector3)moveDirection * speed * Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the bullet hits the player
        if (collision.CompareTag("Player"))
        {
            PlayerHealth.Health playerHealth = collision.GetComponent<PlayerHealth.Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);  // Player takes damage
                Debug.Log("Player took " + damageAmount + " damage!");
            }
            else
            {
                Debug.LogWarning("PlayerHealth component not found on the player!");
            }

            DestroyBullet();
        }

        // Check if the bullet hit the ground or other objects
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            DestroyBullet();
        }

        // Check if the bullet hit a player's projectile (bullet)
        if (collision.CompareTag("PlayerBullet")) // Ensure the player's bullets have the tag "PlayerBullet"
        {
            DestroyBullet();
        }
    }

    void DestroyBullet()
    {
        // Play destroy effect if there is one
        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject); // Destroy the homing bullet
    }
}
