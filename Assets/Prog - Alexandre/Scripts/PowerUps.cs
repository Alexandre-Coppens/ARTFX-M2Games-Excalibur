using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [SerializeField] private CurrentPowerUp powerUps;
    [SerializeField] private GameObject PowerUpsCentral;

    Player_Behaviour player;
    Sword sword;
    public enum CurrentPowerUp
    {
        Hand,
        Head,
        Heart,
    }

    public void Interacted()
    {
        player = Player_Behaviour._instance;
        sword = Sword.instance;
        PowerUpsCentral.SetActive(true);
        switch (powerUps)
        {
            case CurrentPowerUp.Hand:
                PowerUpsCentral.GetComponent<PowerUpsAnimations>().StartAnimPOW(1);
                player.canBreakPOW = true;
                sword.ChangeSkin(1);
                break;

            case CurrentPowerUp.Head:
                PowerUpsCentral.GetComponent<PowerUpsAnimations>().StartAnimPOW(2);
                player.canThrowPOW = true;
                sword.ChangeSkin(2);
                break;

            case CurrentPowerUp.Heart:
                PowerUpsCentral.GetComponent<PowerUpsAnimations>().StartAnimPOW(3);
                player.lastPOW = true;
                sword.ChangeSkin(3);
                break;
        }
        Destroy(gameObject);
    }
}
