using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleLock : MonoBehaviour
{
    [SerializeField] private bool hasSword = false;

    Player_Behaviour player;
    Sword sword;

    [SerializeField] private UnityEvent[] functionsOpen;
    [SerializeField] private UnityEvent[] functionsClose;
    [SerializeField] private AudioClip audioOpen;
    [SerializeField] private AudioClip audioClose;

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
            StartCoroutine(EnterSword());
        }
        else
        {
            StartCoroutine(RemoveSword());
        }
    }

    public void SwordInteracted()
    {
        StartCoroutine(EnterSword());
    }

    private IEnumerator EnterSword()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponent<AudioSource>().clip = audioOpen;
        GetComponent<AudioSource>().Play();

        hasSword = true;
        sword.GetToPosition(transform);
        foreach (var func in functionsOpen)
        {
            func.Invoke();
        }
    }

    private IEnumerator RemoveSword()
    {
        yield return new WaitForSeconds(0.2f);
        GetComponent<AudioSource>().clip = audioClose;
        GetComponent<AudioSource>().Play();
        hasSword = false;
        sword.GetToPosition(null);
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
