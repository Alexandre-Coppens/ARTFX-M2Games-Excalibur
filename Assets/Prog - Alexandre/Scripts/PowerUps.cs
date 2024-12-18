using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField] private CurrentPowerUp powerUps;

    Player_Behaviour player;
    public enum CurrentPowerUp
    {
        Hand,
        Head,
        Heart,
    }

    public void Interacted()
    {
        player = Player_Behaviour._instance;
        switch (powerUps)
        {
            case CurrentPowerUp.Hand:
                player.canBreakPOW = true;
                break;

            case CurrentPowerUp.Head:
                player.canThrowPOW = true;
                break;

            case CurrentPowerUp.Heart:
                player.lastPOW = true;
                break;
        }
        Destroy(gameObject);
    }
}
