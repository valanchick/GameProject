using UnityEngine;

public class VampireEnemy : EnemyHealth
{
    [Header("Настройки движения")]
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float stoppingDistance = 1f;
    [SerializeField] private float chaseRange = 8f;

    [Header("Вампирские способности")]
    [SerializeField] private int attackDamage = 15;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField][Range(0f, 1f)] private float healthStealPercent = 0.3f;

    [Header("Ссылки")]
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask playerLayer;

    private float lastAttackTime;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement; 

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            player = GameObject.FindWithTag("Player").transform;
        }
    }

    private void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            if (distanceToPlayer > stoppingDistance)
            {
                movement = (player.position - transform.position).normalized;
                rb.linearVelocity = movement * moveSpeed;
            }
            else
            {
                movement = Vector2.zero;
                rb.linearVelocity = Vector2.zero;

                if (Time.time >= lastAttackTime + attackCooldown)
                {
                    AttackPlayer();
                    lastAttackTime = Time.time;
                }
            }
        }
        else
        {
            movement = Vector2.zero;
            rb.linearVelocity = Vector2.zero;
        }

        if (animator != null)
        {
            animator.SetFloat("DirectionX", movement.x);
            animator.SetFloat("DirectionY", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }
    }

    private void AttackPlayer()
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            int damageDealt = playerHealth.TakeDamage(attackDamage);
            StealHealth(damageDealt);
        }
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    private void StealHealth(int damageDealt)
    {
        int healthToRestore = Mathf.RoundToInt(damageDealt * healthStealPercent);
        Heal(healthToRestore);
        Debug.Log($"Вампир восстановил {healthToRestore} здоровья");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
    }
}