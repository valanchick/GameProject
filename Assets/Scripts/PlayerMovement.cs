using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float sprintEnergyDrain = 10f;
    [SerializeField] private float sprintEnergyRegen = 5f;
    [SerializeField] private float maxStamina = 100f;

    private bool isRunning;
    private float currentStamina;
    private Vector2 movement;
    private Animator animator;
    private Rigidbody2D rb;

    public Bar staminaBar;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentStamina = maxStamina;
        staminaBar.setMaxAmount(maxStamina);
    }

    private void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        movement = movement.normalized;


        if (Input.GetKey(KeyCode.LeftShift) && currentStamina > 0)
        {
            isRunning = true;
            currentStamina -= sprintEnergyDrain * Time.deltaTime;
        }
        else
        {
            isRunning = false;
            if (currentStamina < maxStamina)
            {
                currentStamina += sprintEnergyRegen * Time.deltaTime;
            }
        }
        staminaBar.setAmount(currentStamina);

        animator.SetBool("IsRunning", isRunning);
        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude); 
    }

    private void FixedUpdate()
    {

        float currentSpeed = isRunning ? sprintSpeed : speed;
        rb.linearVelocity = movement * currentSpeed;
    }

    public void RestoreStamina(int amount)
    {
        currentStamina = Mathf.Clamp(currentStamina + amount, 0, maxStamina);
        staminaBar.setAmount(currentStamina);
    }
}