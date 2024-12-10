using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Variables")]
    [Tooltip("Define what the enemy is currently doing")]
    [SerializeField] private EnemyAction currentAction;
    [Tooltip("The level of difficulty of the enemy")]
    [SerializeField, Range(1,3)] private int ennemyLevel = 1;
    [Header("CheckPlayer")]
    [Tooltip("X is left range of enemy & Y is the right range of the enemy")]
    [SerializeField] private Vector2 ennCheckSize = Vector2.one;
    [Header("Roaming")]
    [Tooltip("X is left range of enemy & Y is the right range of the enemy")]
    [SerializeField] private float ennWalkSpeed = 2f;
    [Tooltip("X is left range of enemy & Y is the right range of the enemy")]
    [SerializeField] private Vector2 ennWalkRange;
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

    [Header("Debug Roaming")]
    private Vector2 ennWalkBoundaries;

    [Header("Debug Attack")]
    private float currentAttackTime;
    private bool hasAttacked = false;

    [Header("Elements")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;

    public enum EnemyAction
    {
        Idle,
        Roaming,
        Attacking,
        Hit,
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        ennWalkBoundaries = new Vector2(transform.position.x + ennWalkRange.x, transform.position.x + ennWalkRange.y);
    }

    void Update()
    {
        switch (currentAction)
        {
            case EnemyAction.Idle:
                CheckForPlayer();
                break;
            case EnemyAction.Roaming:
                CheckForPlayer();
                Roaming();
                break;
            case EnemyAction.Attacking:
                Attack();
                break;
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
        if(currentAttackTime == 0)
        {
            currentAttackTime += Time.deltaTime;
            attackDifference = new Vector3(Mathf.Abs(attackDifference.x) * (spriteRenderer.flipX ? 1 : -1), attackDifference.y);
        }
        else if (currentAttackTime < attackTime && !hasAttacked)
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
            currentAction = EnemyAction.Idle;
        }
    }

    private void CheckForPlayer()
    {
        Collider2D[] hit = Physics2D.OverlapBoxAll(transform.position, ennCheckSize, 0);
        bool playerInRange = false;
        bool playerIsLeft = false;
        foreach (Collider2D col in hit)
        {
            if (col.CompareTag("Player"))
            {
                playerInRange = true;
                playerIsLeft = transform.position.x > col.transform.position.x;
                break;
            }
        }
        if (playerInRange)
        {
            rb.velocity = Vector2.zero;
            currentAction = EnemyAction.Attacking;
            if(currentAttackTime == 0) { spriteRenderer.flipX = !playerIsLeft; }
        }
        else
        {
            currentAction = EnemyAction.Roaming;
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

    }
}
