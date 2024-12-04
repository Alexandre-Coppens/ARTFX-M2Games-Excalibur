using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Behaviour : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float jumpAirTime = 2f;

    [Header("Debug Components")]
    [SerializeField] private Player_Inputs inputs;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Debug Controller")]
    [SerializeField] private float movement;
    [SerializeField] private bool jumpPressed;
    [SerializeField] private float currentAirJumpTime;
    [SerializeField] private bool attackPressed;
    [SerializeField] private bool interactPressed;
    [SerializeField] private bool throwPressed; 

    void Start()
    {
        inputs = Player_Inputs.instance;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        Movements();
    }

    private void GetInputs()
    {
        movement = inputs.movement;
        jumpPressed = inputs.jumpPressed;
        attackPressed = inputs.attackPressed;
        interactPressed = inputs.interactionPressed;
        throwPressed = inputs.throwPressed;
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

        }
    }
}
