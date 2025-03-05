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

    public GameObject dialogueLog;
    public LineView lineView;

    public Sprite speed1x;
    public Sprite speed2x;
    public Sprite speed4x;
    
    private DialogueRunner dialogueRunner;

    private bool autoClicked = false;
    private int speedClicked = 0;
    // private float characterPerSecond = 30f;
    private bool logClicked = false;
    // private bool skipClicked = false;
    private float holdTime = 3f;
    private Coroutine autoAdvanceCoroutine;

    void Start()
    {
        dialogueRunner = GameManager.Instance.GetDialogueRunner();

        autoButton.onClick.AddListener(OnClickAutoButton);
        speedButton.onClick.AddListener(OnClickSpeedButton);
        logButton.onClick.AddListener(OnClickLogButton);
        skipButton.onClick.AddListener(OnClickSkipButton);
        
        dialogueLog.SetActive(false);
    }
    void Update()
    {
        if(!logClicked) return;

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            logClicked = false;
            dialogueLog.SetActive(false);
        }   
    }

    void OnClickAutoButton()
    {
        if(logClicked) return;

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
        if(logClicked) return;
        
        Debug.Log("[DialogueMenuSystem] Click Speed");
        speedClicked = (speedClicked + 1) % 3;

        switch(speedClicked)
        {
        case 0:
            lineView.typewriterEffectSpeed = 15;
            speedButton.image.sprite = speed1x;
            break;
        case 1:
            lineView.typewriterEffectSpeed = 32;
            speedButton.image.sprite = speed2x;
            break;
        case 2:
            lineView.typewriterEffectSpeed = 85;
            speedButton.image.sprite = speed4x;
            break;
        }
    }

    void OnClickLogButton()
    {
        if(logClicked) return;

        Debug.Log("[DialogueMenuSystem] Click Log");
        if (logClicked == false)
        {
            dialogueLog.SetActive(true);
            logClicked = true;
        }
    }

    void OnClickSkipButton()
    {
        if(logClicked) return;

        Debug.Log("[DialogueMenuSystem] Click Skip");
        Skip();

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

    private void Skip()
    {
        dialogueRunner.Stop();
    }
}
