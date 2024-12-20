using UnityEngine;

public class Potion : MonoBehaviour
{
    private Player_Behaviour player;

    public void Interacted()
    {
        player = Player_Behaviour._instance;
        if (player.playerLife != 3) player.playerLife += 1;
        UIHealth health = UIHealth.instance;
        health.UpdateHealth(player.playerLife);
        Destroy(gameObject);
    }
}
