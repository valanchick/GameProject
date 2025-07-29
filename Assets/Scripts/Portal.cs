using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal linkedPortal;
    public float cooldownTime = 0.5f;

    private bool isOnCooldown = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOnCooldown && linkedPortal != null)
        {
            
            linkedPortal.StartCooldown();
            StartCooldown();
            TeleportPlayer(other.gameObject);
        }
    }

    void TeleportPlayer(GameObject player)
    {
        Vector2 offset = player.transform.position - transform.position;

        player.transform.position = (Vector2)linkedPortal.transform.position + offset;

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = rb.linearVelocity;
        }
    }

    public void StartCooldown()
    {
        isOnCooldown = true;
        Invoke(nameof(ResetCooldown), cooldownTime);
    }

    void ResetCooldown()
    {
        isOnCooldown = false;
    }
}