using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class Portal : MonoBehaviour
{
    [Tooltip("Ending VFX")]
    [SerializeField] private VisualEffect vfx;
    [SerializeField] private float scaleVFX;
    [SerializeField] private Animation theEnd;
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
        theEnd.Play();
        StartCoroutine(Ending());
    }

    private IEnumerator Ending()
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(4);
    }
}
