using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Behaviour : MonoBehaviour
{
    [HideInInspector] public static Player_Behaviour _instance;

    [Header("Variables")]
    [Tooltip("The player speed")]
    [SerializeField] private float movementSpeed = 3f;
    [Tooltip("The maximum of the player's life")]
    [SerializeField] private int maxPlayerLife = 5;
    [Tooltip("Current Player's life.")]
    [SerializeField] private int playerLife = 5;
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
    [Tooltip("If the player has the sword")]
    public bool hasSword = true;
    [Tooltip("The Sword GameObject")]
    [SerializeField] private GameObject sword;
    [Tooltip("Time taken for an attack")]
    [SerializeField] private float attackTime = 0.2f;
    [Tooltip("Time before combo reset (if we keep it)")]
    [SerializeField] private float attackResetTime = 0.5f;
    [Tooltip("Move the position of the center of the attack collider")]
    [SerializeField] private Vector2 attackDifference = Vector2.zero;
    [Tooltip("Changes the size of the attack collider")]
    [SerializeField] private Vector2 attackSize = Vector2.one;
    [Tooltip("The strength used to throw the sword")]
    [SerializeField] private Vector2 swordSpeed = Vector2.one;
    [Tooltip("When the player is hit, it's the stun time before the player can move again")]
    [SerializeField] private float stunTime = 0.2f
    [Header("Interaction")]
    [Tooltip("The radius of the interaction range")]
    public bool isInInteraction = false;
    [Tooltip("The radius of the interaction range")]
    [SerializeField] private float interactionRadius = 0.2f;

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
    private bool isAttackFlipped = false;

    [Header("Debug Interaction")]
    private bool hasInteracted = false;

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

    private void Awake()
    {
        _instance = this;
    }

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
        Interaction();
        if (!isInInteraction)
        {
            Movements();
            Attack();
            ThrowSword();
        }
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
        if (currentAttackTime == 0)
        {
            if (movement > 0.2f) { spriteRenderer.flipX = false; }
            else if (movement < -0.2f) { spriteRenderer.flipX = true; }
        }
        else
        {
            spriteRenderer.flipX = isAttackFlipped;
        }
        attackDifference = new Vector3(Mathf.Abs(attackDifference.x) * (isAttackFlipped ? -1 : 1), attackDifference.y);
        rb.position += new Vector2 (movement * movementSpeed, rb.velocity.y) * Time.deltaTime;
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
        if (attackPressed && !hasAttacked && hasSword)
        {
            hasAttacked = true;
            if (currentAttackTime == 0)
            {
                isAttackFlipped = spriteRenderer.flipX;
                currentAttackTime += Time.deltaTime;
                if (isOnGround) { animator.SetInteger("nbrAtt", 1); }
                else { animator.SetInteger("nbrAtt", 0); }
                animator.SetTrigger("Attack");
            }
        }
        else if (!attackPressed)
        {
            hasAttacked = false;
        }

        if (currentAttackTime < attackTime && currentAttackTime != 0)
        {
            currentAttackTime += Time.deltaTime;
        }
        else { currentAttackTime = 0; }
    }

    private void ThrowSword()
    {
        if(hasSword && throwPressed)
        {
            hasSword = false;
            sword.transform.parent = null;
            Rigidbody2D swordRb = sword.GetComponent<Rigidbody2D>();
            swordRb.simulated = true;
            if (!spriteRenderer.flipX)
            {
                swordRb.angularVelocity = -600;
                swordRb.velocity = swordSpeed;
            }
            else
            {
                swordRb.angularVelocity = 600;
                swordRb.velocity = new Vector2(-swordSpeed.x, swordSpeed.y);
            }
            
        }
    }

    private void Interaction()
    {
        if (interactPressed && !hasInteracted)
        {
            hasInteracted = true;
            Collider2D[] allObjects = Physics2D.OverlapCircleAll(transform.position, interactionRadius);
            Collider2D nearestInteractible = null;
            foreach (Collider2D collider in allObjects)
            {
                Debug.Log("In Range: "+ collider.name + " / Tag: " + collider.tag);
                if (collider.CompareTag("Interactible"))
                {
                    if(nearestInteractible == null)
                    {
                        nearestInteractible = collider;
                    }
                    else if(Vector3.Distance(transform.position, collider.transform.position) < Vector3.Distance(transform.position, nearestInteractible.transform.position))
                    {
                        nearestInteractible = collider;
                    }
                }
            }
            if(nearestInteractible != null)
            {
                nearestInteractible.gameObject.GetComponent<Interactible>().Interacted();
                Debug.Log("Interacted with: " + nearestInteractible.name);
            }
        }
        else if (!interactPressed)
        {
            hasInteracted = false;
        }
    }

    private void Animations()
    {
        animator.SetFloat("velX", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("velY", rb.velocity.y);
        animator.SetBool("isOnGround", isOnGround);
    }

    public void GetHurt(Vector2 ejectForce)
    {
        playerLife -= 1;
        rb.velocity = ejectForce;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0, groundDifference), checkSize);
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, swordSpeed);
        if (currentAttackTime > 0) 
        { 
            Gizmos.color = Color.red; 
            Gizmos.DrawWireCube(transform.position + new Vector3(attackDifference.x, attackDifference.y), attackSize); 
        }
    }
}
