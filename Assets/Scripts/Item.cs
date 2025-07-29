using UnityEngine;
using System.Collections; 

public class StaminaParticle : MonoBehaviour
{
    [SerializeField] private int staminaRestore = 20;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CharacterMovement player = other.GetComponent<CharacterMovement>();
            if (player != null)
            {
                player.RestoreStamina(staminaRestore);
                Destroy(gameObject);
            }
        }
    }

}