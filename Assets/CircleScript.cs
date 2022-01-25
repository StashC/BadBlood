using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleScript : MonoBehaviour
{
    // Start is called before the first frame update

    private Rigidbody2D RB;
    private float THRUST = 10f;

    public HealthBar healthbar;

    private int MAXHEALTH = 50;
    private int currentHealth;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        currentHealth = MAXHEALTH;
        healthbar.SetMaxHealth(MAXHEALTH);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            ChangeHealth(1);
        }
        if (Input.GetKeyDown("left shift"))
        {
            ChangeHealth(-1);
        }
    }

    void FixedUpdate()
    {
        if (Input.GetKey("w"))
        {
            RB.AddForce(new Vector2(0, 1) * THRUST * 4.9f);
        }
        if (Input.GetKey("a"))
        {
            RB.AddForce(new Vector2(-1, 0) * THRUST);
        }
        if (Input.GetKey("s"))
        {
            RB.AddForce(new Vector2(0, -1) * THRUST);
        }
        if (Input.GetKey("d"))
        {
            RB.AddForce(new Vector2(1, 0) * THRUST);
        }
    }

    void ChangeHealth(int hp)
    {
        currentHealth -= hp;
        healthbar.SetHealth(currentHealth);
    }
}
