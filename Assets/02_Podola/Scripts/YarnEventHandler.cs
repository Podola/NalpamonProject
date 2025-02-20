using System.Collections;
using UnityEngine;
using Yarn.Unity;

public class YarnEventHandler : MonoBehaviour
{
    // 외부 매니저들
    public DialogueManager dialogueManager;
    public TimelineManager timelineManager;
    public IllustrationManager illustrationManager;
    public StandingManager standingManager;
    public EffectManager effectManager;

    // 코루틴이 끝날 때까지 대기하는 플래그
    private bool isCutsceneFinished = false;

    void Awake()
    {
        var dialogueRunner = dialogueManager.dialogueRunner;

        //--- [PlayCutscene] 코루틴 커맨드 등록 (int cutsceneIndex, string nextNode)
        dialogueRunner.AddCommandHandler<int, string>("PlayCutscene", Command_PlayCutscene);

        //--- 나머지 기존 명령어들 등록 (ex: HideIllustration, ShowStanding 등)
        dialogueRunner.AddCommandHandler("HideIllustration", Command_HideIllustration);
        dialogueRunner.AddCommandHandler<string, string>("ShowStanding", Command_ShowStanding);
        dialogueRunner.AddCommandHandler<string>("Focus", Command_Focus);
        dialogueRunner.AddCommandHandler<string>("HideStanding", Command_HideStanding);
        dialogueRunner.AddCommandHandler("EnableBlur", Command_EnableBlur);
        dialogueRunner.AddCommandHandler("DisableBlur", Command_DisableBlur);
        dialogueRunner.AddCommandHandler("ShakeCamera", Command_ShakeCamera);
        dialogueRunner.AddCommandHandler("FadeIn", Command_FadeIn);
        dialogueRunner.AddCommandHandler("FadeOut", Command_FadeOut);
    }

    //------------------------------------------------------------------------
    // 1) 코루틴 형태의 PlayCutscene 명령
    //------------------------------------------------------------------------
    public IEnumerator Command_PlayCutscene(int cutsceneIndex, string nextNode)
    {
        // 1) 타임라인이 끝날 때까지 대기하기 위한 플래그 초기화
        isCutsceneFinished = false;

        // 2) 타임라인 종료 이벤트 구독
        timelineManager.OnCutsceneFinished += OnCutsceneEnded;

        // 3) 컷씬 재생
        timelineManager.PlayCutscene(cutsceneIndex);

        // 4) "isCutsceneFinished == true" 가 될 때까지 대기
        yield return new WaitUntil(() => isCutsceneFinished);

        // 5) 이벤트 구독 해제
        timelineManager.OnCutsceneFinished -= OnCutsceneEnded;

        // 6) 만약 컷씬 끝난 뒤 특정 Yarn 노드로 점프하고 싶다면:
        if (!string.IsNullOrEmpty(nextNode))
        {
            dialogueManager.dialogueRunner.Stop();
            dialogueManager.dialogueRunner.StartDialogue(nextNode);
        }

        // 7) 커맨드 종료 -> Yarn은 '다음 줄'로 진행
        yield break;
    }

    // 타임라인 종료 시점에 호출될 콜백
    private void OnCutsceneEnded(int index)
    {
        // 코루틴을 깨우기 위해 플래그 true
        isCutsceneFinished = true;
    }

    //------------------------------------------------------------------------
    // [2] 나머지 명령어들 (기존 기능 그대로)
    //------------------------------------------------------------------------
    public void Command_HideIllustration()
    {
        illustrationManager.StopIllustration();
    }

    public void Command_ShowStanding(string standingName, string direction)
    {
        if(direction == "left")
        {
            standingManager.ChangeLeftStandingImage(standingName);
        }
        else if(direction == "right")
        {
            standingManager.ChangeRightStandingImage(standingName);
        }
        else
        {
            Debug.LogWarning("Wrong Standing direction: " + direction);
        }
        standingManager.FocusSpeaker(direction);
    }

    public void Command_Focus(string direction)
    {
        standingManager.FocusSpeaker(direction);
    }

    public void Command_HideStanding(string direction)
    {
        if (direction == "both")
        {
            standingManager.HideLeftStandingImage();
            standingManager.HideRightStandingImage();
        }
        else if (direction == "left")
        {
            standingManager.HideLeftStandingImage();
        }
        else if (direction == "right")
        {
            standingManager.HideRightStandingImage();
        }
        else
        {
            Debug.LogWarning("Wrong Standing direction: " + direction);
        }
    }

    public void Command_EnableBlur()
    {
        dialogueManager.blurController.EnableBlur();
    }

    public void Command_DisableBlur()
    {
        dialogueManager.blurController.DisableBlur();
    }

    public void Command_ShakeCamera()
    {
        effectManager.ShakeCamera();
    }

    public void Command_FadeIn()
    {
        effectManager.FadeIn();
    }

    public void Command_FadeOut()
    {
        effectManager.FadeOut();
    }
}
