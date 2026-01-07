using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public Image[] heartImages;

    private bool isInvincible = false;
    public float invincibilityDuration = 0.5f;

    void Start()
    {

        if (GameManager.instance != null)
            currentHealth = GameManager.instance.currentHealth;
        else
            currentHealth = maxHealth;

        UpdateHeartsUI();
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        if (GameManager.instance != null) GameManager.instance.currentHealth = currentHealth;

        UpdateHeartsUI();
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;
        currentHealth -= damage;

        if (GameManager.instance != null)
        {
            GameManager.instance.currentHealth = currentHealth;
            GameManager.instance.StartPlayerInvincibility(invincibilityDuration);
        }

        isInvincible = true;
        UpdateHeartsUI();
        if (currentHealth <= 0) Die();
    }

    public void EndInvincibility() { isInvincible = false; }

    private void Die()
    {
        if (GameManager.instance != null) GameManager.instance.RespawnPlayer();
    }

    public void UpdateHeartsUI()
    {
        if (heartImages == null) return;
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (heartImages[i] != null)
                heartImages[i].enabled = i < currentHealth;
        }
    }
}