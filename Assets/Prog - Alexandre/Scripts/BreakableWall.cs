using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class BreakableWall : MonoBehaviour
{
    [Tooltip("Number of time the player need to hit this wall")]
    [SerializeField] private int hitPoints = 3;

    private GameObject vfx;

    public void Attacked()
    {
        hitPoints--;
        if (hitPoints == 0)
        {
            vfx = GetComponentInChildren<VisualEffect>().gameObject;
            GameObject goVfx = Instantiate(vfx);
            goVfx.transform.position = transform.position;
            goVfx.transform.localScale = transform.localScale;
            goVfx.GetComponent<VisualEffect>().Play();
            Destroy(gameObject);
        }
    }
}
