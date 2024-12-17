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

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        player = Player_Behaviour._instance;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
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
    }

    public IEnumerator ComeBack()
    {
        while (Vector3.Distance(transform.position, player.transform.position) > 0.5f)
        {
            yield return new WaitForFixedUpdate();
            transform.position = Vector3.SmoothDamp(transform.position, player.transform.position, ref velocity, speed * Time.deltaTime);
        }
        GetToPosition(null);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (transform.parent == null)
        {
            if (collision.CompareTag("Ground"))
            {
                velocity = Vector2.zero;
                StartCoroutine(ComeBack());
            }
        }
    }
}
