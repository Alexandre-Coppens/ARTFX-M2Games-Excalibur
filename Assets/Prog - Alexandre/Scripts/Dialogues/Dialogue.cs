using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private int currentSpeech = 0;
    [SerializeField] private float maxDialogueDist;
    [SerializeField] private string[] dialogueTexts;

    [SerializeField] private UnityEvent functionAssigned;

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
            if (Vector3.Distance(playerBehaviour.transform.position, transform.position) > maxDialogueDist)
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
        if (currentSpeech == dialogueTexts.Length) { mainDialogue.EndDialogue(); functionAssigned.Invoke(); currentSpeech++; };
    }

    private IEnumerator WaitBeforeInteract()
    {
        waitInteract = true;
        yield return new WaitForSeconds(2);
        waitInteract = false;
    }
}