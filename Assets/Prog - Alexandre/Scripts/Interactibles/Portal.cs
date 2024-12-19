using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class Portal : MonoBehaviour
{
    [Tooltip("Ending VFX")]
    [SerializeField] private VisualEffect vfx;
    [SerializeField] private float scaleVFX;
    private bool isPlaying;

    private void Start()
    {
        vfx = GetComponentInChildren<VisualEffect>();
    }

    private void Update()
    {
        if (isPlaying) { vfx.gameObject.transform.localScale += new Vector3(scaleVFX, scaleVFX) * Time.deltaTime; }
    }

    public void DestroyPortal()
    {
        vfx.Play();
        isPlaying = true;
    }
}
