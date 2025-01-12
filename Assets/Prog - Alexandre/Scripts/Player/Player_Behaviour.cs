using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class Player_Behaviour : MonoBehaviour
{
    [HideInInspector] public static Player_Behaviour _instance;

    [Header("Variables")]
    [Tooltip("The player speed")]
    [SerializeField] private float movementSpeed = 3f;
    //[Tooltip("The maximum of the player's life")]
    //[SerializeField] private int maxPlayerLife = 5;
    [Tooltip("Current Player's life.")]
    public int playerLife = 5;
    [Tooltip("Put the placeholder here (animations)")]
    public GameObject placeholder;
    [Tooltip("Put the Game Over here (animations)")]
    public GameObject gameOver;
    [Tooltip("Put the Game Over Buttons Script here")]
    public Buttons gameOverButtons;
    [Tooltip("Put the Pause Gameobject here")]
    public GameObject pauseMenu;
    [Tooltip("Put the Pause Buttons Script here")]
    public Buttons pauseButtons;
    [Tooltip("Put the Life UI GameObject here")]
    public GameObject healthUI;

    [Header("Jump")]
    [Tooltip("Player Y speed when he jumps")]
    [SerializeField] private float jumpForce = 1f;
    [Tooltip("Max time the player can press the jump button to get higher")]
    [SerializeField] private float jumpAirTime = 2f;
    [Tooltip("The gravity left:normal - right: falling")]
    [SerializeField] private Vector2 gravityScale = new Vector2(9.8f, 15);

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
    [Tooltip("Time taken to make the hitbox for the attack")]
    [SerializeField] private float attackHitTime = 0.2f;
    [Tooltip("Time before the player can throw the sword")]
    [SerializeField] private float attackThrowTime = 1.5f;
    [Tooltip("Move the position of the center of the attack collider")]
    [SerializeField] private Vector2 attackDifference = Vector2.zero;
    [Tooltip("Changes the size of the attack collider")]
    [SerializeField] private Vector2 attackSize = Vector2.one;
    [Tooltip("The strength used to throw the sword")]
    [SerializeField] private Vector2 swordSpeed = Vector2.one;

    [Header("Hit")]
    [Tooltip("When the player is hit, it's the stun time before the player can move again")]
    [SerializeField] private float stunTime = 0.2f;
    [Tooltip("Transition when the player fall into the spikes")]
    [SerializeField] private Animator transiAnim;
    [Tooltip("VFX when player is hit")]
    [SerializeField] private VisualEffect vfx;

    [Header("Interaction")]
    [Tooltip("The player cannot move in interaction")]
    public bool isInInteraction = false;
    [Tooltip("The radius of the interaction range")]
    [SerializeField] private float interactionRadius = 0.2f;
    [Tooltip("Time it takes for the player to take or remove his sword")]
    [SerializeField] private float interactionTime = 0.2f;

    [Header("Sounds")]
    [SerializeField] private AudioClip moveSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip hitSound;


    [Header("Power Ups")]
    [Tooltip("First Power Up")]
    public bool canBreakPOW = false;
    [Tooltip("Second Power Up")]
    public bool canThrowPOW = false;
    [Tooltip("Third Power Up")]
    public bool lastPOW = false;

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
    private float attackPressTime;
    private bool hasHit;
    private bool signThrow;
    private bool isDead;

    [Header("Debug Interaction")]
    //private bool hasInteracted = false;
    private Vector3 lastCheckpoint;
    private bool hasPaused = true;

    [Header("Debug Components")]
    private Player_Inputs inputs;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private AudioSource audioSource;

    [Header("Debug Controller")]
    [HideInInspector]public float movement;
    private bool jumpPressed;
    private bool attackPressed;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        Time.timeScale = 1;
        inputs = Player_Inputs.instance;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        vfx = GetComponentInChildren<VisualEffect>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead) return;
        GetInputs();
        CheckGround();
        InteractionAttack();
        if (!isInInteraction)
        {
            Movements();
        }
        Animations();
        Pause();
    }

    private void GetInputs()
    {
        if (!isInInteraction)
        {
            movement = inputs.movement;
        }

            jumpPressed = inputs.jumpPressed;
        attackPressed = inputs.attackPressed;
    }

    private void Pause()
    {
        if (inputs.pausePressed && hasPaused) return;
        if (!inputs.pausePressed) {hasPaused = false; return; }
        if(Time.timeScale == 0)
        {
            pauseButtons.canInteractController = false;
            pauseMenu.SetActive(false);
            healthUI.SetActive(true);
            Time.timeScale = 1;
            hasPaused = true;
        }
        else
        {
            pauseMenu.SetActive(true);
            healthUI.SetActive(false);
            pauseButtons.canInteractController = true;
            Time.timeScale = 0;
            hasPaused = true;
        }
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
        if (rb.velocity.y < -0.1f) { rb.gravityScale = gravityScale.y; }
        else { rb.gravityScale = gravityScale.x; }
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
                audioSource.clip = jumpSound;
                audioSource.Play();
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

    private void ThrowSword()
    {
        if(hasSword)
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

    private void InteractionAttack()
    {
        if (attackPressed)
        {
            attackPressTime += Time.deltaTime;

            if(attackPressTime > attackThrowTime && !signThrow && hasSword && canThrowPOW)
            {
                inputs.AddRumble(new Vector2(2, 5), 0.1f);
                Debug.Log("Rumble");
                signThrow = true;
            }

            if(hasAttacked) { return; }
            
            Collider2D[] allObjects = Physics2D.OverlapCircleAll(transform.position, interactionRadius);
            Collider2D nearestInteractible = null;
            bool enemiesNear = false;
            foreach (Collider2D collider in allObjects)
            {
                Debug.Log("In Range: " + collider.name + " / Tag: " + collider.tag);
                if (collider.CompareTag("Interactible"))
                {
                    if (nearestInteractible == null)
                    {
                        nearestInteractible = collider;
                    }
                    else if (Vector3.Distance(transform.position, collider.transform.position) < Vector3.Distance(transform.position, nearestInteractible.transform.position))
                    {
                        nearestInteractible = collider;
                    }
                    
                }
                else if (collider.CompareTag("Enemies"))
                {
                    enemiesNear = true;
                }
            }

            if (nearestInteractible != null && !enemiesNear)
            {
                hasAttacked = true;
                movement = 0;
                nearestInteractible.gameObject.GetComponent<Interactible>().Interacted();
                Debug.Log("Interacted with: " + nearestInteractible.name);
                StartCoroutine(Interact());
            }
            else if (hasSword)
            {
                if (currentAttackTime == 0)
                {
                    audioSource.clip = attackSound;
                    audioSource.Play();
                    StartCoroutine(PlayerAttack());
                    hasAttacked = true;
                    isAttackFlipped = spriteRenderer.flipX;
                    currentAttackTime += Time.deltaTime;
                    if (isOnGround) { animator.SetInteger("nbrAtt", 1); }
                    else { animator.SetInteger("nbrAtt", 0); }
                    animator.SetTrigger("Attack");
                }
            }
            else if (!hasSword)
            {
                if (sword.transform.parent == null || !canThrowPOW) return;
                if (sword.transform.parent.CompareTag("Interactible"))
                {
                    sword.GetComponent<Sword>().ComeBack();
                    animator.SetTrigger("GetSword");
                }
            }
        }
        else
        {
            if(attackPressTime > attackThrowTime && canThrowPOW)
            {
                ThrowSword();
                signThrow = false;
            }
            attackPressTime = 0;
            hasAttacked = false;
        }

        if(currentAttackTime < attackTime && currentAttackTime != 0 && currentAttackTime > attackHitTime && !hasHit)
        {
            if (!hasHit)
            {
                Collider2D[] hit = Physics2D.OverlapBoxAll(transform.position + new Vector3(attackDifference.x, attackDifference.y), attackSize, 0);
                CanBeHit nearestHit = null;
                foreach (Collider2D hit2 in hit)
                {
                    if (hit2.GetComponent<CanBeHit>() != null)
                    {
                        if (nearestHit == null) { nearestHit = hit2.GetComponent<CanBeHit>(); }
                        else if (Vector3.Distance(transform.position, hit2.transform.position) < Vector3.Distance(transform.position, nearestHit.transform.position))
                        {
                            nearestHit = hit2.GetComponent<CanBeHit>();
                        }
                    }
                }
                if (nearestHit != null)
                {
                    if (canBreakPOW || nearestHit.CompareTag("Enemies")) nearestHit.Attacked();
                    inputs.AddRumble(new Vector2(200, 200), 0.1f);
                }
                hasHit = true;
            }
        }
        else if (currentAttackTime < attackTime && currentAttackTime != 0)
        {
            currentAttackTime += Time.deltaTime;
        }
        else { currentAttackTime = 0; hasHit = false; }
    }

    private void Animations()
    {
        animator.SetFloat("velX", Mathf.Abs(movement) + Mathf.Abs(rb.velocity.x));
        animator.SetFloat("velY", rb.velocity.y);
        animator.SetBool("isOnGround", isOnGround);
        animator.SetBool("hasSword", hasSword);
        placeholder.transform.eulerAngles = new Vector3(0f, spriteRenderer.flipX?180:0, 0f);
        if (playerLife == 0)
        {
            animator.SetBool("Die", true);
            gameOver.GetComponent<Animation>().Play();
            isDead = true;
            gameOverButtons.canInteractController = true;
            StartCoroutine(Die());
        }
    }

    public void GetHurt(Vector2 ejectForce)
    {
        playerLife -= 1;
        healthUI.GetComponent<UIHealth>().UpdateHealth(playerLife);
        rb.velocity = ejectForce;
        animator.SetTrigger("Hit");
        vfx.Play();
        audioSource.clip = hitSound;
        audioSource.Play();
        StartCoroutine("PlayerStun");
    }
    private IEnumerator PlayerAttack()
    {
        isInInteraction = true;
        yield return new WaitForSeconds(attackTime);
        isInInteraction = false;
    }
    private IEnumerator PlayerStun()
    {
        isInInteraction = true;
        yield return new WaitForSeconds(stunTime);
        isInInteraction = false;
    }
    private IEnumerator Interact()
    {
        isInInteraction = true;
        animator.SetTrigger("Interact");
        yield return new WaitForSeconds(interactionTime);
        isInInteraction = false;
    }

    public IEnumerator Spiked()
    {
        playerLife--;
        isInInteraction = true;
        healthUI.GetComponent<UIHealth>().UpdateHealth(playerLife);
        animator.SetBool("Die", true);
        yield return new WaitForSeconds(0.2f);
        transiAnim.SetTrigger("Ended");
        yield return new WaitForSeconds(0.6f);
        transform.position = lastCheckpoint;
        transiAnim.SetTrigger("Started");
        animator.SetBool("Die", false);
        yield return new WaitForSeconds(0.2f);
        isInInteraction = false ;
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(1.9f);
        healthUI.SetActive(false);
        Time.timeScale = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Checkpoint"))
        {
            lastCheckpoint = collision.transform.position;
        }
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
