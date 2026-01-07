using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private int scoreValue = 10;
    private string uniqueID;

    private void Start()
    {

        uniqueID = gameObject.name + transform.position.ToString();


        if (GameManager.instance != null && GameManager.instance.collectedItems.Contains(uniqueID))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManager.instance != null)
            {

                GameManager.instance.collectedItems.Add(uniqueID);
                GameManager.instance.AddScore(scoreValue);
            }
            Destroy(gameObject);
        }
    }
}