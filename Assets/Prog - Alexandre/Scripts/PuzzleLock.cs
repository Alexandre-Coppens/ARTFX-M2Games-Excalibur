using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleLock : MonoBehaviour
{
    private bool hasSword = false;

    Player_Behaviour player;
    Sword sword;

    [SerializeField] private UnityEvent[] functionsOpen;
    [SerializeField] private UnityEvent[] functionsClose;

    private void Start()
    {
        player = Player_Behaviour._instance;
        sword = Sword.instance;
    }

    public void Interacted()
    {
        if (!hasSword)
        {
            if (!player.hasSword){ return; }
            EnterSword();
        }
        else
        {
            RemoveSword();
        }
    }

    private void EnterSword()
    {
        hasSword = true;
        sword.GetToPosition(transform, true);
        foreach (var func in functionsOpen)
        {
            func.Invoke();
        }
    }

    private void RemoveSword()
    {
        hasSword = false;
        sword.GetToPosition(null, false);
        foreach (var func in functionsClose)
        {
            func.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasSword) { return; }
        Sword swordColl = collision.GetComponent<Sword>();
        if (swordColl == null) { return; }
        if(player.hasSword){ return; }
        EnterSword();
    }
}
