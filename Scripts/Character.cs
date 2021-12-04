using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float MoveSpeed, JumpFore;

    public bool Jumping;

    public Rigidbody2D RG2D;

    
   
    float MovX ;
    float MovY ;

    //List of a item which picked up (if applicable)
    
    //public List<item>();

    // Start is called before the first frame update
    void Start()
    {
        RG2D = GetComponent<Rigidbody2D>();
        MoveSpeed = 1f ;
        JumpFore = 6f;
        Jumping = true;
    }

    // Update is called once per frame
    void Update()
    {
        MovX = Input.GetAxisRaw("Horizontal");
        MovY = Input.GetAxisRaw("Vertical");
    }


    void FixedUpdate()
    {
        // Horizontal Movement
        if (MovX != 0)
        {
            Run(MovX);
        }
        // Jumping
        if (MovY == 1 && !Jumping)
        {
            Jump(MovY);
        }
        //Crouching
        if (MovY == -1)
        {
            transform.localScale = new Vector2(1f, 0.5f);
        }
        else
        {
            transform.localScale = new Vector2(1f, 1f);
        }
    }

    void Run(float dir)
    {
        float X_Axis = MoveSpeed * dir * Time.fixedDeltaTime * 80;
        RG2D.velocity = new Vector2(X_Axis, RG2D.velocity.y);
    }
    void Jump(float dir)
    {
        float Y_Axis = JumpFore * Time.fixedDeltaTime * 45;
        RG2D.velocity = new Vector2(RG2D.velocity.x, Y_Axis);
        Jumping = true;
    }
}
