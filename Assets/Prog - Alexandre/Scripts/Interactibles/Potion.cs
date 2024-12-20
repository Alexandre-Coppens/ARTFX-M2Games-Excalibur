using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    private Player_Behaviour player;

    public void Interacted()
    {
        player = Player_Behaviour._instance;
        if (player.playerLife != 3) player.playerLife += 1;
        Destroy(gameObject);
    }
}
