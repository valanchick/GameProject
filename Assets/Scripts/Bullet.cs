using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] public int damage;
    [SerializeField] public float speed;
    [SerializeField] public float lifetime;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerHealth>().TakeDamage(damage);
            Destroy(gameObject); 
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}