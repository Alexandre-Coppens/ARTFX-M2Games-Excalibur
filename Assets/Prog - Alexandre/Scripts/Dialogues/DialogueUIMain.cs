using TMPro;
using UnityEngine;

public class DialogueUIMain : MonoBehaviour
{
    [HideInInspector] public static DialogueUIMain Instance;

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private Player_Behaviour playerBehaviour;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        playerBehaviour = Player_Behaviour._instance;
        dialogueBox.SetActive(false);
    }

    public void StartDialogue()
    {
        dialogueBox.SetActive(true);
        playerBehaviour.isInInteraction = true;
    }

    public void ChangeDialogue(string dialogue)
    {
        dialogueText.text = dialogue;
    }

    public void EndDialogue()
    {
        dialogueBox.SetActive(false);
        playerBehaviour.isInInteraction = false;
    }
}
