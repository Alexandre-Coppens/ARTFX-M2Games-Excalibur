using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Tilemaps;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //ENEMY FLIP IS RIGHT
    [Header("Variables")]
    [Tooltip("Define what the enemy is currently doing")]
    [SerializeField] private EnemyAction currentAction;
    [Tooltip("The level of difficulty of the enemy")]
    [SerializeField, Range(1,3)] private int ennemyLevel = 1;
    [Tooltip("The number of health of the ennemy")]
    [SerializeField, Range(1, 10)] private int health = 3;

    [Header("CheckPlayer")]
    [Tooltip("X is left range of enemy & Y is the right range of the enemy")]
    [SerializeField] private Vector2 ennCheckSize = Vector2.one;
    [Tooltip("Enemy chasing range")]
    [SerializeField] private float enChaseRange = 10f;
    [Tooltip("Left Check to not fall from platforms")]
    [SerializeField] private Vector3 leftPlatCheck = Vector2.zero;
    [Tooltip("Right Check to not fall from platforms")]
    [SerializeField] private Vector3 rightPlatCheck = Vector2.zero;

    [Header("Roaming")]
    [Tooltip("Enemy normal speed")]
    [SerializeField] private float ennWalkSpeed = 2f;
    [Tooltip("X is left range of enemy & Y is the right range of the enemy")]
    [SerializeField] private Vector2 ennWalkRange;

    [Header("Chasing")]
    [Tooltip("When the enemy see the player he will stop for before chasing")]
    [SerializeField] private float enStopTime = 2f;
    [Tooltip("Enemy chasing speed")]
    [SerializeField] private float ennChaseSpeed = 2f;

    [Header("Attack")]
    [Tooltip("Time taken for an attack")]
    [SerializeField] private float attackTime = 0.2f;
    [Tooltip("Exact Time when the ennemy hit")]
    [SerializeField] private float hurtTime = 0.1f;
    [Tooltip("The force the ennemy hits you (knockback)")]
    [SerializeField] private Vector2 hitForce = Vector2.one;
    [Tooltip("Move the position of the center of the attack collider")]
    [SerializeField] private Vector2 attackDifference = Vector2.zero;
    [Tooltip("Changes the size of the attack collider")]
    [SerializeField] private Vector2 attackSize = Vector2.one;

    [Header("Hit")]
    [Tooltip("Velocity with wich the ennemy will move when hit")]
    [SerializeField] private float hitVelocity = 3f;
    [Tooltip("Nbr in seconds of stun when hit")]
    [SerializeField] private float hitStunTime = 0.7f;

    [Header("Debug Idle")]
    private float idleLeft;
    private EnemyAction nextAction;

    [Header("Debug Chase")]
    private Vector3 chasePoint;

    [Header("Debug Roaming")]
    private Vector2 ennWalkBoundaries;

    [Header("Debug Attack")]
    private float currentAttackTime;
    private bool hasAttacked = false;

    [Header("Elements")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator animator;
    [SerializeField] private Player_Behaviour player;

    public enum EnemyAction
    {
        Idle,
        Roaming,
        Attacking,
        Hit,
        Chase,
        Dead,
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        ennWalkBoundaries = new Vector2(transform.position.x + ennWalkRange.x, transform.position.x + ennWalkRange.y);
        player = Player_Behaviour._instance;
        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        switch (currentAction)
        {
            case EnemyAction.Idle:
                IdleWait();
                break;
            case EnemyAction.Roaming:
                if(animator.GetInteger("State") != 1) { animator.SetInteger("State", 1); animator.SetTrigger("Change"); }
                CheckForPlayer();
                Roaming();
                break;
            case EnemyAction.Attacking:
                if (animator.GetInteger("State") != 2) { animator.SetInteger("State", 2); animator.SetTrigger("Change"); }
                Attack();
                break;
            case EnemyAction.Chase:
                if (animator.GetInteger("State") != 4) { animator.SetInteger("State", 4); animator.SetTrigger("Change"); }
                CheckForPlayer();
                Chase();
                break;
        }
    }

    private void IdleWait()
    {
        if (idleLeft <= 0)
        {
            currentAction = nextAction;
        }
        else
        {
            idleLeft -= Time.deltaTime;
        }
    }

    private void Roaming()
    {
        if (spriteRenderer.flipX) { rb.velocity = new Vector2(ennWalkSpeed, rb.velocity.y); }
        else { rb.velocity = new Vector2(-ennWalkSpeed, rb.velocity.y); }

        if(rb.velocity == Vector2.zero) rb.velocity = new Vector2(0,0.1f);

        RaycastHit2D hitL = Physics2D.Raycast(new Vector2(transform.position.x - transform.localScale.x * 0.5f - 0.05f, transform.position.y), Vector2.left, 0.3f);
        RaycastHit2D hitR = Physics2D.Raycast(new Vector2(transform.position.x + transform.localScale.x * 0.5f + 0.05f, transform.position.y), Vector2.right, 0.3f);

        if (transform.position.x < ennWalkBoundaries.x) { spriteRenderer.flipX = true; }
        else if (transform.position.x > ennWalkBoundaries.y) { spriteRenderer.flipX = false; }
        else if (hitL) { spriteRenderer.flipX = true; }
        else if (hitR) { spriteRenderer.flipX = false; }
    }

    private void Attack()
    {
        rb.velocity = Vector2.zero;
        if(currentAttackTime == 0)
        {
            currentAttackTime += Time.deltaTime;
            attackDifference = new Vector3(Mathf.Abs(attackDifference.x) * (spriteRenderer.flipX ? 1 : -1), attackDifference.y);
        }
        else if (currentAttackTime < attackTime && currentAttackTime > hurtTime && !hasAttacked)
        {
            hasAttacked = true;
            Collider2D[] hit = Physics2D.OverlapBoxAll(transform.position + new Vector3(attackDifference.x, attackDifference.y), attackSize, 0);
            foreach (Collider2D collider2D in hit)
            {
                if(collider2D.CompareTag("Player"))
                {
                    Player_Behaviour player_Behaviour = collider2D.GetComponent<Player_Behaviour>();
                    player_Behaviour.GetHurt(new Vector2(spriteRenderer.flipX?hitForce.x:-hitForce.x, hitForce.y));
                }
            }
        }
        else if (currentAttackTime < attackTime)
        {
            currentAttackTime += Time.deltaTime;
        }
        else
        {
            currentAttackTime = 0;
            hasAttacked = false;
            CheckForPlayer();
        }
    }

    private void Chase()
    {
        if (chasePoint.x > transform.position.x) spriteRenderer.flipX = true;
        else spriteRenderer.flipX = false;

        Debug.Log(Vector3.Distance(chasePoint, transform.position));
        if (Vector3.Distance(chasePoint, transform.position) > 1f)
        {
            if (spriteRenderer.flipX) { rb.velocity = new Vector2(ennChaseSpeed, rb.velocity.y); }
            else { rb.velocity = new Vector2(-ennChaseSpeed, rb.velocity.y); }
        }
        else
        {
            idleLeft = 1f;
            if (animator.GetInteger("State") != 0) { animator.SetInteger("State", 0); animator.SetTrigger("Change"); }
            nextAction = EnemyAction.Roaming;
            currentAction = EnemyAction.Idle;
            CheckForPlayer();
        }
    }

    private void CheckForPlayer()
    {
        Collider2D[] hit = Physics2D.OverlapBoxAll(transform.position, ennCheckSize, 0);
        RaycastHit2D[] ray = Physics2D.RaycastAll(transform.position, player.transform.position.x<transform.position.x?Vector2.left:Vector2.right, enChaseRange);
        foreach (Collider2D col in hit)
        {
            if (col.CompareTag("Player"))
            {
                rb.velocity = Vector2.zero;
                currentAction = EnemyAction.Attacking;
                if (currentAttackTime == 0) { spriteRenderer.flipX = transform.position.x < player.transform.position.x; }
                return;
            }
        }

        bool grounded = false;
        hit = !spriteRenderer.flipX?Physics2D.OverlapCircleAll(transform.position + leftPlatCheck, 0.05f): Physics2D.OverlapCircleAll(transform.position + rightPlatCheck, 0.05f);
        foreach (Collider2D col in hit)
        {
            if (col.CompareTag("Ground"))
            {
                grounded = true;
                break;
            }
        }
        if (!grounded) 
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            if (currentAction == EnemyAction.Roaming)
            {
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }
            if (currentAction == EnemyAction.Chase)
            {
                chasePoint = transform.position;
            }
            return; 
        }
        if (ennemyLevel == 1) { return; }
        foreach (RaycastHit2D raycast in ray)
        {
            if (raycast.collider.CompareTag("Player"))
            {
                chasePoint = player.transform.position;
                if (currentAction != EnemyAction.Chase)
                {
                    nextAction = EnemyAction.Chase;
                    if (animator.GetInteger("State") != 0) { animator.SetInteger("State", 0); animator.SetTrigger("Change"); }
                    idleLeft = enStopTime;
                    currentAction = EnemyAction.Idle;
                }
                return;
            }
        }

        if(currentAction != EnemyAction.Chase)
        {
            currentAction = EnemyAction.Roaming;
        }
    }

    public void Attacked()
    {
        health -= 1;
        if (health > 0)
        {
            idleLeft = hitStunTime;
            if (animator.GetInteger("State") != 3) { animator.SetInteger("State", 3); animator.SetTrigger("Change"); }
            nextAction = EnemyAction.Attacking;
            currentAction = EnemyAction.Idle;
            rb.velocity = new Vector2(hitVelocity * (player.transform.position.x > transform.position.x ? -1 : 1), 0);
        }
        else
        {
            animator.SetTrigger("Dead");
            currentAction = EnemyAction.Dead;
            gameObject.tag = "Untagged";
            Destroy(GetComponent<CanBeHit>());
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (!EditorApplication.isPlaying)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(transform.position.x + ennWalkRange.x, transform.position.y - 1), new Vector3(transform.position.x + ennWalkRange.x, transform.position.y + 1));
            Gizmos.DrawLine(new Vector3(transform.position.x + ennWalkRange.y, transform.position.y - 1), new Vector3(transform.position.x + ennWalkRange.y, transform.position.y + 1));
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(transform.position.x + ennWalkRange.x - transform.localScale.x * 0.5f, transform.position.y - 1), new Vector3(transform.position.x + ennWalkRange.x - transform.localScale.x * 0.5f, transform.position.y));
            Gizmos.DrawLine(new Vector3(transform.position.x + ennWalkRange.y + transform.localScale.x * 0.5f, transform.position.y - 1), new Vector3(transform.position.x + ennWalkRange.y + transform.localScale.x * 0.5f, transform.position.y));

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + new Vector3(attackDifference.x, attackDifference.y), attackSize);
        }
        else
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(ennWalkBoundaries.x, transform.position.y - 1), new Vector3(ennWalkBoundaries.x, transform.position.y + 1));
            Gizmos.DrawLine(new Vector3(ennWalkBoundaries.y, transform.position.y - 1), new Vector3(ennWalkBoundaries.y, transform.position.y + 1));
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(ennWalkBoundaries.x - transform.localScale.x * 0.5f, transform.position.y - 1), new Vector3(ennWalkBoundaries.x - transform.localScale.x * 0.5f, transform.position.y));
            Gizmos.DrawLine(new Vector3(ennWalkBoundaries.y + transform.localScale.x * 0.5f, transform.position.y - 1), new Vector3(ennWalkBoundaries.y + transform.localScale.x * 0.5f, transform.position.y));
        }

        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(transform.position, ennCheckSize);
        Gizmos.DrawLine(new Vector3(transform.position.x + transform.localScale.x * 0.5f + 0.05f, transform.position.y), new Vector3(transform.position.x + transform.localScale.x * 0.5f + 0.4f, transform.position.y));
        Gizmos.DrawLine(new Vector3(transform.position.x - transform.localScale.x * 0.5f - 0.05f, transform.position.y), new Vector3(transform.position.x - transform.localScale.x * 0.5f - 0.4f, transform.position.y));

        if(currentAttackTime > 0)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position + new Vector3(attackDifference.x, attackDifference.y), attackSize);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(chasePoint, 0.1f);
        Gizmos.DrawWireSphere(transform.position + leftPlatCheck, 0.1f);
        Gizmos.DrawWireSphere(transform.position + rightPlatCheck, 0.1f);


    }
}
