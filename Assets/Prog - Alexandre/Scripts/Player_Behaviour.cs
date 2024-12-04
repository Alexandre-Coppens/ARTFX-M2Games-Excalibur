using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Behaviour : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float jumpAirTime = 2f;

    [Header("GroundCheck")]
    [SerializeField] private float groundDifference = 1f;
    [SerializeField] private float checkSize = 1f;
    [SerializeField] private int groundLayer = 6;

    [Header("Debug Jump")]
    [SerializeField] private bool isOnGround = true;
    [SerializeField] private bool isJumping;
    [SerializeField] private bool canJump;
    [SerializeField] private float currentAirJumpTime;

    [Header("Debug Components")]
    [SerializeField] private Player_Inputs inputs;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;

    [Header("Debug Controller")]
    [SerializeField] private float movement;
    [SerializeField] private bool jumpPressed;
    [SerializeField] private bool attackPressed;
    [SerializeField] private bool interactPressed;
    [SerializeField] private bool throwPressed; 

    

    void Start()
    {
        inputs = Player_Inputs.instance;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        CheckGround();
        Movements();
        Animations();
    }

    private void GetInputs()
    {
        movement = inputs.movement;
        jumpPressed = inputs.jumpPressed;
        attackPressed = inputs.attackPressed;
        interactPressed = inputs.interactionPressed;
        throwPressed = inputs.throwPressed;
    }

    private void CheckGround()
    {
        Collider2D[] hit = Physics2D.OverlapCircleAll(transform.position + new Vector3(0, groundDifference), checkSize);
        bool _test = false;
        foreach (Collider2D col in hit)
        {
            if (col.gameObject.layer == groundLayer) { _test = true; break; }
        }
        if (_test && rb.velocity.y < 0.1f)
        {
            isOnGround = true;
        }
        else { isOnGround = false; }
    }

    private void Movements()
    {
        spriteRenderer.flipX = movement > 0 ? false : true;
        rb.velocity = new Vector2 (movement * movementSpeed, rb.velocity.y);
        Jump();
    }

    private void Jump()
    {
        if (jumpPressed)
        {
            if (!isJumping && isOnGround)
            {
                animator.SetTrigger("Jump");
                isJumping = true;
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                currentAirJumpTime = 0;
            }
            else if (isJumping && currentAirJumpTime <= jumpAirTime)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                currentAirJumpTime += Time.deltaTime;
            }
            else if(isJumping && currentAirJumpTime > jumpAirTime) 
            { 
                isJumping = false;
            }
        }
        else
        {
            isJumping = false;
        }
    }

    private void Animations()
    {
        animator.SetFloat("velX", rb.velocity.x);
        animator.SetFloat("velY", rb.velocity.y);
        animator.SetBool("isOnGround", isOnGround);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, groundDifference), checkSize);
    }
}
