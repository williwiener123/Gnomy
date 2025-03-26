using UnityEngine;
using PlayerHealth;  // Ensure this is added if you're using a namespace

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5f; // Bullet speed
    public float lifetime = 3f; // Bullet disappears after this time
    public int damageAmount = 1; // Damage dealt to the player
    public GameObject destroyEffect; // Assign a destroy effect prefab
    public LayerMask groundLayer; // Set to "Ground" in Inspector

    private Vector2 moveDirection; // The direction the bullet will move

    private void Start()
    {
        Destroy(gameObject, lifetime); // Destroy bullet after 'lifetime' seconds
    }

    private void FixedUpdate()
    {
        // Move the bullet in the set direction
        transform.position += (Vector3)moveDirection * speed * Time.fixedDeltaTime;
    }

    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized; // Set direction when instantiated
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the bullet hit the ground
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            PlayDestroyEffect();
            Destroy(gameObject);
        }
        // Check if the bullet hit the player
        else if (collision.CompareTag("Player"))
        {
            // Ensure the collision object has a Health component attached
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

            PlayDestroyEffect();
            Destroy(gameObject);
        }
    }

    void PlayDestroyEffect()
    {
        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }
    }
}
