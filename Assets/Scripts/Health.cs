using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public HealthBar healthBar;

    public float MaxHealth = 100f;
    public float CurrentHealth;

    private void Start()
    {
        CurrentHealth = MaxHealth;
        healthBar.SetMaxHealth(MaxHealth);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetHealth(20f);
        }
    }


    void SetHealth(float amount)
    {
        CurrentHealth -= amount;
        healthBar.SetHealth(CurrentHealth);
    }
}
