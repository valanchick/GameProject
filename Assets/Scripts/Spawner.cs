using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Основные настройки")]
    public GameObject enemyPrefab;
    public float spawnInterval = 15f;
    public float minDistanceFromPlayer = 5f;
    public float spawnAreaPadding = 2f;

    [Header("Отладка")]
    public bool showDebug = true;
    public Color spawnAreaColor = new Color(1, 0, 0, 0.2f);

    private Transform player;
    private Camera gameCamera;
    private float spawnTimer;
    private Vector2 worldSize;

    void Start()
    {
        InitializeReferences();
        CalculateWorldBounds();
        DebugStartup();
    }

    void Update()
    {
        HandleSpawning();
    }

    void InitializeReferences()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        gameCamera = Camera.main;

        if (player == null) Debug.LogError("Не найден объект с тегом 'Player'!");
        if (gameCamera == null) Debug.LogError("Не найдена основная камера!");
        if (enemyPrefab == null) Debug.LogError("Не назначен префаб врага!");
    }

    void CalculateWorldBounds()
    {
        if (gameCamera == null) return;

        float cameraHeight = 2f * gameCamera.orthographicSize;
        float cameraWidth = cameraHeight * gameCamera.aspect;
        worldSize = new Vector2(cameraWidth, cameraHeight) * 0.9f; 
    }

    void HandleSpawning()
    {
        if (player == null || enemyPrefab == null) return;

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

    void SpawnEnemy()
    {
        Vector2 spawnPos = CalculateSpawnPosition();
        if (spawnPos != Vector2.zero)
        {
            Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            if (showDebug) Debug.Log($"Спавн врага на позиции: {spawnPos}");
        }
    }

    Vector2 CalculateSpawnPosition()
    {
        for (int i = 0; i < 30; i++) 
        {
            Vector2 randomPos = GetRandomPosition();

            if (IsPositionValid(randomPos))
            {
                if (showDebug)
                    Debug.DrawLine(player.position, randomPos, Color.green, 2f);

                return randomPos;
            }
        }

        if (showDebug) Debug.LogWarning("Не удалось найти валидную позицию для спавна");
        return Vector2.zero;
    }

    Vector2 GetRandomPosition()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        return (Vector2)player.position + randomDirection * minDistanceFromPlayer;
    }

    bool IsPositionValid(Vector2 position)
    {
        bool inBounds = Mathf.Abs(position.x) < worldSize.x / 2 - spawnAreaPadding &&
                      Mathf.Abs(position.y) < worldSize.y / 2 - spawnAreaPadding;

        bool farFromPlayer = Vector2.Distance(position, player.position) >= minDistanceFromPlayer;

        return inBounds && farFromPlayer;
    }

    void DebugStartup()
    {
        if (!showDebug) return;

        Debug.Log($"Инициализация спавнера. Размер мира: {worldSize}");
        Debug.Log($"Игрок {(player != null ? "найден" : "не найден")}");
    }

    void OnDrawGizmos()
    {
        if (!showDebug || player == null) return;

        Gizmos.color = spawnAreaColor;
        Gizmos.DrawWireCube(Vector3.zero, worldSize);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(player.position, minDistanceFromPlayer);
    }
}