using UnityEngine;

public class TeleportingEnemy : MonoBehaviour
{
    public GameObject bulletPrefab; 
    public Transform player;
    public float teleportInterval = 10f;
    public float shootInterval = 2f;
    public int maxHealth = 100;
    public float bulletSpeed = 5f;

    private float teleportTimer;
    private float shootTimer;
    private int currentHealth;
    private Camera mainCamera;
    private Vector2 screenBounds;

    void Start()
    {
        currentHealth = maxHealth;
        mainCamera = Camera.main;
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        TeleportToRandomPosition();
    }

    void Update()
    {
        teleportTimer += Time.deltaTime;
        if (teleportTimer >= teleportInterval)
        {
            TeleportToRandomPosition();
            teleportTimer = 0f;
        }

        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            ShootAtPlayer();
            shootTimer = 0f;
        }
    }

    void TeleportToRandomPosition()
    {
        float x = Random.Range(-screenBounds.x + 1, screenBounds.x - 1);
        float y = Random.Range(-screenBounds.y + 1, screenBounds.y - 1);
        transform.position = new Vector2(x, y);
    }

    void ShootAtPlayer()
    {
        if (player == null || !player.gameObject.activeInHierarchy)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            if (player == null) return;
        }

        Vector2 direction = (player.position - transform.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().linearVelocity = direction * bulletSpeed;
        bullet.transform.right = direction;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}