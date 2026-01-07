using UnityEngine;
using System.Collections;
public class PlayerMovement : MonoBehaviour 
{

    Rigidbody2D rb;
    SpriteRenderer sr;

    Animator anim;  

    [Header("Movement")] 
    public float moveSpeed = 5f;   
    public float jumpForce = 12f;  

    [Header("Ground Check")] 
    public Transform groundCheck;      
    public float groundCheckRadius = 0.2f; 
    public LayerMask groundLayer;       
    bool isGrounded;
    public bool canMove = true;
    public void DisableMovement(float duration)
    {
        StartCoroutine(LockMovement(duration));
    }
    IEnumerator LockMovement(float duration)
    {
        canMove = false;
        yield return new WaitForSeconds(duration);
        canMove = true;
    }

    void Awake()
    {

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        anim = GetComponent<Animator>();
    }

    void Update()
    {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager gm = FindAnyObjectByType<GameManager>(); 
                if (gm != null)
                {
                    gm.TogglePause();
                }
            }
            if (!canMove) return;
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (moveInput != 0) sr.flipX = moveInput < 0;

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        if (anim)
        {
            anim.SetFloat("speed", Mathf.Abs(rb.linearVelocity.x)); 
            anim.SetBool("isGrounded", isGrounded);                
            anim.SetFloat("yVel", rb.linearVelocity.y);            
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!groundCheck) return; 
        Gizmos.color = Color.red; 
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}