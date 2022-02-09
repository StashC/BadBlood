using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class healthScript : MonoBehaviour
{
    public HealthBar healthbar;
    public int _team;

    [SerializeField] private int _maxHealth = 50;
    private int currentHealth;
    [SerializeField] private UnityEvent onDeathEvent;

    void Start()
    {
        currentHealth = _maxHealth;
        if (healthbar != null) healthbar.SetMaxHealth(_maxHealth);
    }

    void ChangeHealth(int hp)
    {
        currentHealth -= hp;
        healthbar.SetHealth(currentHealth);
    }

    public void DealDamage(int damage)
    {
        currentHealth -= damage;
        if (healthbar != null)
        {
            healthbar.SetHealth(currentHealth);
        }
        if (currentHealth <= 0) onDeathEvent.Invoke();
        Debug.LogError("My health is: " + currentHealth);

    }
}
