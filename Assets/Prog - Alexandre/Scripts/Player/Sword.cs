using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    
    private Player_Behaviour player;
    private Transform parent;

    void Start()
    {
        player = Player_Behaviour._instance;
        parent = transform.parent.transform;
    }

    public void Interacted()
    {
        GetComponent<Rigidbody2D>().simulated = false;
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        player.hasSword = true;
    }
}
