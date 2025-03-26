using UnityEngine;

public class HomingShooter : MonoBehaviour
{
    public GameObject homingBulletPrefab; // Homing bullet prefab
    public Transform firePoint; // Bullet spawn position
    public float fireRate = 1.5f; // Time between shots
    public float bulletSpeed = 5f; // Speed of the bullet
    public Transform player; // Assign player in Inspector
    public float attackRange = 10f; // How close the player needs to be

    private float nextFireTime;

    private void Update()
    {
        if (player == null)
            return;

        // Check if player is within attack range
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void Shoot()
    {
        // Instantiate the homing bullet
        GameObject bullet = Instantiate(homingBulletPrefab, firePoint.position, Quaternion.identity);

        // Get direction to the player (this part is now handled by HomingEnemyBullet itself)
        // No need to call SetDirection here
    }
}
