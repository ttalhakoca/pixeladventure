using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    [Header("Ayarlar")]
    public float range = 8f;           
    public float fireRate = 2f;        
    private float nextFireTime;

    [Header("Referanslar")]
    public Transform player;           
    public GameObject bulletPrefab;    
    public Transform firePoint;        
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();


        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);


        if (distanceToPlayer <= range)
        {
            LookAtPlayer();

            if (Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void LookAtPlayer()
    {

        if (player.position.x > transform.position.x)
        {

            transform.localScale = new Vector3(-3f, 3f, 1f);
        }
        else
        {

            transform.localScale = new Vector3(3f, 3f, 1f);
        }
    }

    void Shoot()
    {
        if (anim != null) anim.SetTrigger("AttackTrigger");


        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        float lookDir = (transform.localScale.x == -3f) ? 3f : -3f;
        bullet.transform.localScale = new Vector3(lookDir, 3f, 1f);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}