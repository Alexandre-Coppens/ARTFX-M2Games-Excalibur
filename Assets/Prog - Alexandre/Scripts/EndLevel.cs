using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    private bool interacted = false;
    private Rigidbody2D player;
    [SerializeField] private Animator animator;
    [SerializeField] private int sceneLoaded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (interacted)
        {
            player.velocity = new Vector3(2, player.velocity.y);
        }
    }

    private IEnumerator changeScene()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneLoaded);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player_Behaviour>().isInInteraction = true;
            player = collision.GetComponent<Rigidbody2D>();
            animator.SetTrigger("Ended");
            interacted = true;
            StartCoroutine(changeScene());
        }
    }
}
