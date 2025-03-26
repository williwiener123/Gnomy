using UnityEngine;

public class HomingBullet : MonoBehaviour
{
    public float initialSpeed = 10f;   // Speed when first shot
    public float homingSpeed = 7f;     // Speed when homing
    public float homingDelay = 0.2f;   // Time before homing starts
    public float lifetime = 5f;        // Bullet lifespan
    public int damage = 10;            // Damage to enemies
    public float detectionRadius = 5f; // Radius to detect enemies
    public float homingStrength = 10f; // Homing smoothness
    public LayerMask enemyLayer;       // Set this to the enemy layer in Inspector
    public LayerMask groundLayer;      // Set this to the ground layer in Inspector
    public GameObject destroyEffect;   // Effect when bullet hits

    private Transform target;          // Closest enemy within range
    private Vector2 moveDirection;     // Current movement direction
    private bool isHoming = false;     // If homing is active
    private Vector2 lastMoveDirection; // To track if it stops moving

    private void Start()
    {
        // Get the initial direction towards the cursor
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        moveDirection = (mousePos - transform.position).normalized;

        // Start homing after a delay
        Invoke("StartHoming", homingDelay);

        // Destroy bullet after some time
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        if (!isHoming)
        {
            // Move in the initial direction first
            transform.position += (Vector3)moveDirection * initialSpeed * Time.deltaTime;
        }
        else
        {
            FindClosestEnemy(); // Continuously check for the nearest enemy

            if (target != null)
            {
                // Move towards the enemy smoothly
                Vector2 directionToEnemy = (target.position - transform.position).normalized;
                moveDirection = Vector2.Lerp(moveDirection, directionToEnemy, homingStrength * Time.deltaTime);

                transform.position += (Vector3)moveDirection * homingSpeed * Time.deltaTime;

                // Rotate towards enemy
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, angle), homingStrength * Time.deltaTime);
            }
            else
            {
                // If no enemy is found, keep moving forward
                transform.position += (Vector3)moveDirection * homingSpeed * Time.deltaTime;
            }

            // Check if the bullet is stuck in place (i.e., no movement)
            if (moveDirection == lastMoveDirection)
            {
                // If movement direction hasn't changed, force the bullet to move to the right
                moveDirection = Vector2.right;
            }

            lastMoveDirection = moveDirection; // Update the last move direction
        }
    }

    void StartHoming()
    {
        isHoming = true;
    }

    void FindClosestEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (Collider2D enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        target = closestEnemy;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemyScript = other.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(damage);
            }

            DestroyBullet();
        }

        // Check if bullet hits the ground
        if (((1 << other.gameObject.layer) & groundLayer) != 0)
        {
            DestroyBullet();
        }
    }

    void DestroyBullet()
    {
        if (destroyEffect != null)
        {
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    // Draw Gizmo for detection radius
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

