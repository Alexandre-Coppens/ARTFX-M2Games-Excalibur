using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public static Sword instance;

    private SpriteRenderer spriteRenderer;
    private Player_Behaviour player;
    private Transform parent;

    [SerializeField] private Sprite normalState;
    [SerializeField] private Sprite lockState;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        player = Player_Behaviour._instance;
        parent = transform.parent.transform;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    public void Interacted()
    {
        GetToPosition(null, false);
    }

    public void GetToPosition(Transform pos, bool isLock)
    {
        if (pos == null)
        {
            pos = parent;
            player.hasSword = true;
        }
        GetComponent<Rigidbody2D>().simulated = false;
        transform.SetParent(pos);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        if (isLock) { spriteRenderer.sprite = lockState; }
        else { spriteRenderer.sprite = normalState; }
        }
}
