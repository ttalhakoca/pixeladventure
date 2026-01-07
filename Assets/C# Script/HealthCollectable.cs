using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
    [SerializeField] 
    public int healthAmount = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
        {
            HealthManager playerHealth = collision.GetComponent<HealthManager>();

            if (playerHealth != null)
            {
                playerHealth.Heal(healthAmount);
                Destroy(gameObject);
            }
        }
    }
}