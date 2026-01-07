using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 12f;
    public float maxDistance = 15f;
    private Vector2 startPosition;

    void Start()
    {
        startPosition = transform.position;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        float direction = transform.localScale.x > 0 ? 1f : -1f;
        rb.linearVelocity = new Vector2(direction * speed, 0);
    }

    void Update()
    {

        if (Vector2.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player") || collision.CompareTag("Ground"))
        {
            if (collision.CompareTag("Player"))
            {
                HealthManager hm = collision.GetComponent<HealthManager>();
                if (hm != null) hm.TakeDamage(1);
            }
            Destroy(gameObject);
        }
    }
}