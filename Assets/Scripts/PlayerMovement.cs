﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxcollider;
    private float wallJumpCooldown;
    private float horizontalInput;
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;


  
    private void Awake()
    {   
        //get references from game object
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxcollider = GetComponent<BoxCollider2D>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.J))
        {
            TakeDamage(20);
        }

        horizontalInput = Input.GetAxis("Horizontal");
        
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        //flip player when moving left-right
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1,1,1);
               

        //set animatir parameters
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        // wall jumping logic
        if(wallJumpCooldown > 0.2f)
        {
            
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
            
            if(onWall() && isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;

            }
            else            
                body.gravityScale = 3;

            if (Input.GetKey(KeyCode.Space))
                Jump();

        }
        else
        {
            wallJumpCooldown += Time.deltaTime;
        }

    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        else if(onWall() && !isGrounded())              
        {
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);// can modify 3 value according what I want
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);// can modify 3 value according what I want

            wallJumpCooldown = 0;
           
        }
                
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {           
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxcollider.bounds.center, boxcollider.bounds.size, 0, Vector2.down, 0.1f,groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxcollider.bounds.center, boxcollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }


    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }
}
