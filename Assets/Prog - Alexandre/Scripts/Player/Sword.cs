using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public static Sword instance;

    [Header("ComeBack")]
    [Tooltip("The parent attached to the player where the sword wild come back to")]
    [SerializeField] private Transform parent;
    [Tooltip("The speed at which it comes back")]
    [SerializeField] private float speed;

    private SpriteRenderer spriteRenderer;
    private Player_Behaviour player;

    private Vector3 velocity;
    [HideInInspector] public bool comeback = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        player = Player_Behaviour._instance;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(comeback)
        {
            if (Vector3.Distance(transform.position, player.transform.position) > 1.5f)
            {
                transform.position = Vector3.Lerp(transform.position, player.transform.position, speed * Time.deltaTime);
            }
            else
            {
                GetToPosition(null);
                comeback = false ;
            }
        }
    }

    public void Interacted()
    {
        GetToPosition(null);
    }

    public void GetToPosition(Transform pos)
    {
        if (pos == null)
        {
            pos = parent;
            player.hasSword = true;
        }
        else
        {
            player.hasSword = false;
        }
        GetComponent<Rigidbody2D>().simulated = false;
        transform.SetParent(pos);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
<<<<<<< Updated upstream
        transform.localScale = Vector3.one;
=======
>>>>>>> Stashed changes
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.parent == null)
        {
            if (collision.CompareTag("Ground"))
            {
                velocity = Vector2.zero;
                comeback = true ;
<<<<<<< Updated upstream
            }
            else if (collision.CompareTag("Interactible"))
            {
                if (collision.GetComponent<PuzzleLock>() != null)
                {
                    collision.GetComponent<Interactible>().Interacted();
                }
=======
>>>>>>> Stashed changes
            }
        }
    }
}
