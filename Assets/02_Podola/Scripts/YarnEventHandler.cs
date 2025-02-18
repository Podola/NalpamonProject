using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class YarnEventHandler : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public TimelineManager timelineManager;
    public IllustrationManager illustrationManager;
    public PortraitManager portraitManager;

    private bool isEventFinished = false;
    private UnityAction eventHandler;

    void Awake()
    {
         eventHandler = OnEventEnded;
        dialogueRunner.AddCommandHandler<string>("PlayCutscene", Command_PlayCutscene);
        dialogueRunner.AddCommandHandler<string>("ShowIllustration", Command_ShowIllustration);
        dialogueRunner.AddCommandHandler<string, string>("ShowPortrait", Command_ShowPortrait);
        dialogueRunner.AddCommandHandler<string>("HidePortrait", Command_HidePortrait);

    }

    public IEnumerator Command_PlayCutscene(string cutsceneName)
    {
        isEventFinished = false;
        
        // 종료 이벤트에 핸들러를 구독
        timelineManager.onCutsceneEnd.AddListener(eventHandler);

        // 컷씬 재생
        timelineManager.PlayCutscene(cutsceneName);

        // 컷씬이 끝날 때까지 대기
        yield return new WaitUntil(() => isEventFinished);

        // 이벤트 구독 해제
        timelineManager.onCutsceneEnd.RemoveListener(eventHandler);

        // Yarn 다음줄 실행
        yield break;
    }

    public IEnumerator Command_ShowIllustration(string illustrationName)
    {
        isEventFinished = false;
        illustrationManager.onIllustrationEnd.AddListener(eventHandler);
        illustrationManager.ChangeIllustrationImage(illustrationName);
        yield return new WaitUntil(() => isEventFinished);
        illustrationManager.onIllustrationEnd.RemoveListener(eventHandler);
        yield break;

    }

  public void Command_ShowPortrait(string portraitName, string direction)
    {
        if(direction == "left")
        {
            portraitManager.ChangeLeftPortraitImage(portraitName);
        }
        else if(direction == "right")
        {
            portraitManager.ChangeRightPortraitImage(portraitName);
        }
        else
        {
            Debug.LogWarning("Wrong portrait direction: " + direction);
        }
    }

    public void Command_HidePortrait(string direction)
    {
        if (direction == "both")
        {
            portraitManager.HideLeftPortraitImage();
            portraitManager.HideRightPortraitImage();
        }
        else if (direction == "left")
        {
            portraitManager.HideLeftPortraitImage();
        }
        else if (direction == "right")
        {
            portraitManager.HideRightPortraitImage();
        }
        else
        {
            Debug.LogWarning("Wrong portrait direction: " + direction);
        }
    }

    // 이벤트 종료 시 호출되는 핸들러
    private void OnEventEnded()
    {
        isEventFinished = true;
    }
}
