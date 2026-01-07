using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 1f;            
    public float distance = 3f;         
    private Vector2 startPoint;         
    private bool movingRight = true;    

    void Start()
    {

        startPoint = transform.position;
    }

    void Update()
    {
        if (movingRight)
        {

            if (transform.position.x < startPoint.x + distance)
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);

                GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {

                movingRight = false;
            }
        }
        else
        {

            if (transform.position.x > startPoint.x - distance)
            {
                transform.Translate(Vector2.left * speed * Time.deltaTime);

                GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {

                movingRight = true;
            }
        }
    }
}