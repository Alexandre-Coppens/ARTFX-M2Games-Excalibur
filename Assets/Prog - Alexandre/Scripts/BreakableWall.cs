using UnityEngine;
using UnityEngine.VFX;

public class BreakableWall : MonoBehaviour
{
    [Tooltip("Number of time the player need to hit this wall")]
    [SerializeField] private int hitPoints = 3;
    [Tooltip("If the breakable object drop a potion when destroyed")]
    [SerializeField] private bool dropPotion = false;
    [SerializeField] private GameObject potionPREFAB;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip breakSound;

    private GameObject vfx;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Attacked()
    {
        hitPoints--;
        audioSource.clip = hitSound;
        if (hitPoints == 0)
        {
            audioSource.clip = breakSound;
            audioSource.Play();
            vfx = GetComponentInChildren<VisualEffect>().gameObject;
            GameObject goVfx = Instantiate(vfx);
            goVfx.transform.position = transform.position;
            goVfx.transform.localScale = transform.localScale;
            goVfx.GetComponent<VisualEffect>().Play();
            if (dropPotion)
            {
                GameObject potion = Instantiate(potionPREFAB);
                potion.transform.position = transform.position;
            }
            Destroy(gameObject);
        }
        audioSource.Play();
    }
}
