using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [Tooltip("Where the player will be teleported to")]
    [SerializeField] private Vector3 teleportTo;
    private Player_Behaviour player;

    public void Interacted()
    {
        player = Player_Behaviour._instance;
        player.transform.position = teleportTo;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(teleportTo, 1.0f);
    }
}
