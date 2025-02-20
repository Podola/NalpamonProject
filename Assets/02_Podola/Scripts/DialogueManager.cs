using UnityEngine;
using Yarn.Unity;

public class DialogueManager : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public BlurController blurController;

    public void ApplyBlur()
    {
        blurController.EnableBlur();
    }

    public void CancleBlur()
    {
        blurController.DisableBlur();
    }
}
