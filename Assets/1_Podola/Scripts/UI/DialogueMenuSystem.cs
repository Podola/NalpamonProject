using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class DialogueMenuSystem : MonoBehaviour
{
    public Button autoButton;
    public Button speedButton;
    public Button logButton;
    public Button skipButton;

    private DialogueRunner dialogueRunner;
    private bool autoClicked = false;
    private int speedClicked = 0;
    private bool logClicked = false;
    private bool skipClicked = false;
    private float holdTime = 3f;
    private Coroutine autoAdvanceCoroutine;

    void Start()
    {
        dialogueRunner = GameManager.Instance.GetDialogueRunner();

        autoButton.onClick.AddListener(OnClickAutoButton);
        speedButton.onClick.AddListener(OnClickSpeedButton);
        logButton.onClick.AddListener(OnClickLogButton);
        skipButton.onClick.AddListener(OnClickSkipButton);
    }

    void OnClickAutoButton()
    {
        Debug.Log("[DialogueMenuSystem] Click Auto");
        autoClicked = !autoClicked;

        if(autoClicked == true)
        {
            StartAutoAdvance();
        }
        else
        {
            StopAutoAdavance();
        }
    }

    void OnClickSpeedButton()
    {
        Debug.Log("[DialogueMenuSystem] Click Speed");
        speedClicked = (speedClicked + 1) % 3;

        switch(speedClicked)
        {
        case 0:
            break;
        case 1:
            break;
        case 2:
            break;
        }
    }

    void OnClickLogButton()
    {
        Debug.Log("[DialogueMenuSystem] Click Log");
    }

    void OnClickSkipButton()
    {
        Debug.Log("[DialogueMenuSystem] Click Skip");

    }

    private void StartAutoAdvance()
    {
        if(autoAdvanceCoroutine == null)
        {
            autoAdvanceCoroutine = StartCoroutine(AutoAdvanceRoutine());
        }
    }

    private void StopAutoAdavance()
    {
        if(autoAdvanceCoroutine != null)
        {
            StopCoroutine(autoAdvanceCoroutine);
            autoAdvanceCoroutine = null;
        }
    }

    private IEnumerator AutoAdvanceRoutine()
    {
        while (dialogueRunner != null && dialogueRunner.IsDialogueRunning)
        {
            yield return new WaitForSeconds(holdTime);

            dialogueRunner.dialogueViews[0].UserRequestedViewAdvancement();
            yield return null;
        }
    }
}
