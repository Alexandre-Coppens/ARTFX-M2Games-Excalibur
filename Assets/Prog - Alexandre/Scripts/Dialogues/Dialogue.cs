using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private int currentSpeech = 0;
    [SerializeField] private string[] dialogueTexts;

    DialogueUIMain mainDialogue;
    private bool waitInteract;

    Player_Behaviour playerBehaviour;

    // Start is called before the first frame update
    void Start()
    {
        mainDialogue = DialogueUIMain.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentSpeech != 0)
        {
            if (Vector3.Distance(playerBehaviour.transform.position, transform.position) > 2)
            {
                mainDialogue.EndDialogue(); 
                currentSpeech = 0;
                playerBehaviour = null;
            }
        }
    }

    public void Interacted()
    {
        if (waitInteract) return;
        if (currentSpeech == 0) { mainDialogue.StartDialogue(); playerBehaviour = Player_Behaviour._instance; }
        mainDialogue.ChangeDialogue(dialogueTexts[currentSpeech]);
        currentSpeech++;
        if (currentSpeech >= dialogueTexts.Length) { mainDialogue.EndDialogue(); currentSpeech = 0; StartCoroutine("WaitBeforeInteract"); };
    }

    private IEnumerator WaitBeforeInteract()
    {
        waitInteract = true;
        yield return new WaitForSeconds(2);
        waitInteract = false;
    }
}