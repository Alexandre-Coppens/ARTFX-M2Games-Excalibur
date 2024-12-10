using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [SerializeField] private int currentSpeech = 0;
    [SerializeField] private string[] dialogueTexts;

    DialogueUIMain mainDialogue;

    // Start is called before the first frame update
    void Start()
    {
        mainDialogue = DialogueUIMain.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interacted()
    {
        if (currentSpeech == 0) { mainDialogue.StartDialogue(); }
        mainDialogue.ChangeDialogue(dialogueTexts[currentSpeech]);
        currentSpeech++;
        if (currentSpeech >= dialogueTexts.Length) { mainDialogue.EndDialogue(); currentSpeech = 0; };
    }
}
