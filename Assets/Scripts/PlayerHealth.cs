using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public float maxHealth;
    private float currentHealth;
    public Bar healthbar;

    private void Start()
    {
        currentHealth = maxHealth;
        healthbar.setMaxAmount(maxHealth);
    }

    public int TakeDamage(int damage)
    {
        float damageTaken = Mathf.Min(damage, currentHealth); 
        currentHealth -= damageTaken;
        healthbar.setAmount(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }

        return (int)damageTaken; 
    }

    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}