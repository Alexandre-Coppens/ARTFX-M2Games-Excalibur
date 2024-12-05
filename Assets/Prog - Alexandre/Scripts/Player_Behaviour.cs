using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Behaviour : MonoBehaviour
{
    [Header("Variables")]
    [Tooltip("The player speed")]
    [SerializeField] private float movementSpeed = 3f;
    [Tooltip("Player Y speed when he jumps")]
    [Header("Jump")]
    [SerializeField] private float jumpForce = 1f;
    [Tooltip("Max time the player can press the jump button to get higher")]
    [SerializeField] private float jumpAirTime = 2f;
    [Header("GroundCheck")]
    [Tooltip("Controll the Y center of the circle")]
    [SerializeField] private float groundDifference = 1f;
    [Tooltip("Controll the radius of the circle")]
    [SerializeField] private float checkSize = 1f;
    [Tooltip("To detect which layer is the Ground")]
    [SerializeField] private int groundLayer = 6;
    [Header("Attack")]
    [Tooltip("Time taken for an attack")]
    [SerializeField] private float attackTime = 0.2f;
    [Tooltip("Time before combo reset (if we keep it)")]
    [SerializeField] private float attackResetTime = 0.5f;
    [Tooltip("Move the position of the center of the attack collider")]
    [SerializeField] private Vector2 attackDifference = Vector2.zero;
    [Tooltip("Changes the size of the attack collider")]
    [SerializeField] private Vector2 attackSize = Vector2.one;

    [Header("Debug Jump")]
    private bool isOnGround = true;
    private bool isJumping;
    private bool canJump;
    private float currentAirJumpTime;

    [Header("Debug Attack")]
    private bool hasAttacked = true;
    private float currentAttackTime;
    private int currentAttack;
    private float currentResetAttackTime;

    [Header("Debug Components")]
    private Player_Inputs inputs;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    [Header("Debug Controller")]
    private float movement;
    private bool jumpPressed;
    private bool attackPressed;
    private bool interactPressed;
    private bool throwPressed; 

    

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
        Attack();
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
        }
        else
        {
            isJumping = false;
        }
    }

    private void Attack() 
    {
        if (attackPressed && !hasAttacked)
        {
            if(currentAttackTime == 0)
            {
                currentAttackTime += Time.deltaTime;
            }
            else if(currentAttackTime < attackTime)
            {
                currentAttackTime += Time.deltaTime;
            }
            else { currentAttackTime = 0; }
        }
        else if (!attackPressed)
        {
            hasAttacked = false;
        }
    }

    private void Animations()
    {
        animator.SetFloat("velX", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("velY", rb.velocity.y);
        animator.SetBool("isOnGround", isOnGround);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, groundDifference), checkSize);
        if (attackTime > 0) 
        { 
            Gizmos.color = Color.red; 
            Gizmos.DrawWireCube(transform.position + new Vector3(attackDifference.x, attackDifference.y), attackSize); 
        }
    }
}
