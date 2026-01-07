using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour
{

    private EnemyDamage enemyDamage;

    [Header("Rotation Settings")]
    public bool isRotating = false;
    public float rotationSpeed = 360f;

    [Header("Timed Trap Settings")]
    public bool isTimedTrap = false;
    public float activeTime = 2f;
    public float inactiveTime = 2f;
    private Collider2D trapCollider;
    private SpriteRenderer spriteRenderer;

    void Awake()
    {

        enemyDamage = GetComponent<EnemyDamage>();
        if (enemyDamage == null)
        {
            Debug.LogError("Trap scripti, EnemyDamage scripti olmadan çalýþamaz! Lütfen EnemyDamage ekleyin.");
            enabled = false;
            return;
        }

        trapCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();


        if (isTimedTrap)
        {
            StartCoroutine(TimedTrapCycle());
        }
    }
    void Update()
    {
        if (isRotating)
        {

            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }

    IEnumerator TimedTrapCycle()
    {

        bool isActive = true;

        while (true)
        {

            if (isActive)
            {

                if (trapCollider != null) trapCollider.enabled = true;
                if (spriteRenderer != null) spriteRenderer.enabled = true;
                if (enemyDamage != null) enemyDamage.enabled = true; 

                yield return new WaitForSeconds(activeTime);

                isActive = false; 
            }
            else
            {

                if (trapCollider != null) trapCollider.enabled = false;
                if (spriteRenderer != null) spriteRenderer.enabled = false;
                if (enemyDamage != null) enemyDamage.enabled = false; 

                yield return new WaitForSeconds(inactiveTime);

                isActive = true;
            }
        }
    }
}
