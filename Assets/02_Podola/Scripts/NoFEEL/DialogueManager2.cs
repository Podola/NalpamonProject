using UnityEngine;
using Yarn.Unity;

public class DialogueManager2 : MonoBehaviour
{
    private DialogueRunner dialogueRunner;
    private BubbleManager2 bubbleManager;
    
    void Awake()
    {
        dialogueRunner = FindFirstObjectByType<DialogueRunner>();
        bubbleManager = FindFirstObjectByType<BubbleManager2>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
