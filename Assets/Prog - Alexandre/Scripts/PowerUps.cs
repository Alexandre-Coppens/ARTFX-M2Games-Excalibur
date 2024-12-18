using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField] private CurrentPowerUp powerUps;
    [SerializeField] private GameObject PowerUpsCentral;

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
        PowerUpsCentral.SetActive(true);
        switch (powerUps)
        {
            case CurrentPowerUp.Hand:
                PowerUpsCentral.GetComponent<PowerUpsAnimations>().StartAnimPOW(1);
                player.canBreakPOW = true;
                break;

            case CurrentPowerUp.Head:
                PowerUpsCentral.GetComponent<PowerUpsAnimations>().StartAnimPOW(2);
                player.canThrowPOW = true;
                break;

            case CurrentPowerUp.Heart:
                PowerUpsCentral.GetComponent<PowerUpsAnimations>().StartAnimPOW(3);
                player.lastPOW = true;
                break;
        }
        Destroy(gameObject);
    }
}
