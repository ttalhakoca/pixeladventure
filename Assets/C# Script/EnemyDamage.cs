using UnityEngine;


public class EnemyDamage : MonoBehaviour
{

    [Header("Damage Settings")]
    public bool isTrap = false;
    public int damageAmount = 1;
    public float knockbackForce = 15f; 
    public float stompBounceForce = 700f; 


    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRB = collision.gameObject.GetComponent<Rigidbody2D>();
            if (playerRB == null) return;


            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            PlayerMovement playerMove = collision.gameObject.GetComponent<PlayerMovement>();
            ContactPoint2D contact = collision.contacts[0];


            if (contact.normal.y < -0.5f)
            {
                if (!isTrap) 
                {
                    
                    playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, 0f);
                    playerRB.AddForce(Vector2.up * stompBounceForce);
                    if (playerMove != null) playerMove.canMove = true;
                    Die(); 
                }
                else 
                {
                    
                    HandleHit(collision, playerRB, playerMove, knockbackDirection);

                    
                    playerRB.linearVelocity = new Vector2(playerRB.linearVelocity.x, 0f);
                    playerRB.AddForce(Vector2.up * stompBounceForce);
                }
            }
            else
            {
                
                HandleHit(collision, playerRB, playerMove, knockbackDirection);
            }
        }
    }

    private void HandleHit(Collision2D collision, Rigidbody2D playerRB, PlayerMovement playerMove, Vector2 knockbackDirection)
    {

        HealthManager playerHealth = collision.gameObject.GetComponent<HealthManager>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damageAmount);
        }

        if (playerMove != null)
        {
            playerMove.DisableMovement(0.2f); 
        }

        float verticalKnockbackMultiplier = isTrap ? 1.0f : 0.7f;
        float verticalKnockback = knockbackForce * verticalKnockbackMultiplier;

        playerRB.linearVelocity = new Vector2(0f, 0f);


        playerRB.AddForce(new Vector2(knockbackDirection.x * knockbackForce, verticalKnockback), ForceMode2D.Impulse);
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}