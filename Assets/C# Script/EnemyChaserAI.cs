using UnityEngine;

public class EnemyChaserAI : MonoBehaviour
{
    [Header("Movement Settings")]
    public float chaseSpeed = 3.5f;
    public float detectionRange = 10f;

    [Header("Detection Settings")]
    public Transform player;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private bool hasStartedChasing = false; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {

        if (!hasStartedChasing)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer < detectionRange)
            {
                hasStartedChasing = true;
                if (anim != null) anim.SetTrigger("isChasing");
            }
        }


        if (hasStartedChasing)
        {
            ChasePlayer();
            if (anim != null) anim.speed = 2f; 
        }
    }

    void ChasePlayer()
    {
        float direction = player.position.x > transform.position.x ? 1 : -1;


        rb.linearVelocity = new Vector2(direction * chaseSpeed, rb.linearVelocity.y);


        spriteRenderer.flipX = (direction > 0);
    }
}