using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [Tooltip("Number of time the player need to hit this wall")]
    [SerializeField] private int hitPoints = 3;

    public void Attacked()
    {
        hitPoints--;
        if (hitPoints == 0)
        {
            Destroy(gameObject);
        }
    }
}
