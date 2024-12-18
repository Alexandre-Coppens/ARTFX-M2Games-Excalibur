using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public static Sword instance;

    [Header("ComeBack")]
    [Tooltip("The parent attached to the player where the sword wild come back to")]
    [SerializeField] private Transform parent;
    [Tooltip("The speed at which it comes back")]
    [SerializeField] private float speed;
    [Tooltip("The max time before the sword comes back automaticaly")]
    [SerializeField] private float maxTimerOut = 3f;

    private SpriteRenderer spriteRenderer;
    private Player_Behaviour player;

    private Vector3 velocity;
    [HideInInspector] public bool comeback = false;

    private float timerOut;

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
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
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

        if (transform.parent != null && !comeback) { timerOut = 0; }
        else { timerOut += Time.deltaTime; }
        if (timerOut > maxTimerOut) { comeback = true; }
       
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
        transform.localScale = Vector3.one;
    }

    public void ComeBack()
    {
        transform.localScale = Vector3.one;
        comeback = true;
        transform.parent = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.parent == null && !comeback)
        {
            if (collision.CompareTag("Ground"))
            {
                velocity = Vector2.zero;
                comeback = true ;
            }
            else if (collision.CompareTag("Interactible"))
            {
                if (collision.GetComponent<PuzzleLock>() != null)
                {
                    collision.GetComponent<PuzzleLock>().SwordInteracted();
                    GetComponent<Rigidbody2D>().velocity = Vector2.zero ;
                }
            }
            else if (collision.CompareTag("Enemies"))
            {
                collision.GetComponent<Enemy>().Attacked();
                velocity = Vector2.zero;
                comeback = true ;
            }
        }
    }
}
