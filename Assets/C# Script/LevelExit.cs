using UnityEngine;

public class LevelExit : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {

            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector2.zero;
            }

            if (GameManager.instance != null)
            {
                GameManager.instance.OpenLevelCompleteScreen();
            }
        }
    }
}
