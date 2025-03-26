using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    public GameObject bulletPrefab; // Bullet prefab
    public Transform firePoint; // Bullet spawn position
    public float fireRate = 1.5f; // Normal fire rate
    public float increasedFireRate = 0.75f; // Faster fire rate when boss is under 50% HP
    public float bulletSpeed = 5f; // Speed of the bullet
    public Transform player; // Assign player in Inspector
    public float attackRange = 10f; // How close player needs to be

    private float nextFireTime;
    private Enemy bossHealth; // Referens till bossens health script

    private void Start()
    {
        bossHealth = GetComponent<Enemy>(); // Hämta bossens hälsosystem
    }

    private void Update()
    {
        if (player == null || bossHealth == null)
            return;

        // Justera fireRate baserat på bossens hälsa
        float currentFireRate = bossHealth.health <= bossHealth.health / 2 ? increasedFireRate : fireRate;

        // Kolla om spelaren är inom räckhåll
        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + currentFireRate; // Uppdatera skott-timer
            }
        }
    }

    void Shoot()
    {
        // Instantiate bullet
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        // Get direction to the player
        Vector2 direction = (player.position - firePoint.position).normalized;

        // Set the bullet's direction and speed
        bullet.GetComponent<EnemyBullet>().SetDirection(direction);
    }
}
