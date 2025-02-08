using UnityEngine;
using Yarn.Unity;

public class YarnEventHandler : MonoBehaviour
{
    private DialogueRunner dr;

    private void Awake()
    {
        dr = FindFirstObjectByType<DialogueRunner>();

        // 이벤트 등록
        dr.onNodeStart.AddListener(OnNodeStart);
        dr.onNodeComplete.AddListener(OnNodeComplete);
        dr.onDialogueComplete.AddListener(OnDialogueComplete);

        // 명령(Command) 등록
        dr.AddCommandHandler<string>("LoadScene", Command_LoadScene);
        dr.AddCommandHandler("BeginFreeRoam", Command_BeginFreeRoam);
        dr.AddCommandHandler("AdvanceDay", Command_AdvanceDay);
        dr.AddCommandHandler<string>("PlaySFX", Command_PlaySFX);
        dr.AddCommandHandler<float>("CameraShake", Command_CameraShake);
    }

    // -----------------------
    // 1) DialogueRunner Events
    // -----------------------
    void OnNodeStart(string nodeName)
    {
        Debug.Log($"[YarnEvent] NodeStart: {nodeName}");

        // 예: 특정 노드 진입 시 카메라 전환, 특정 UI 열기 등
        if (nodeName == "Mansion_1F_Explorer")
        {
            Debug.Log("파스타 저택에 입장. 자유 조사 모드 시작");
            // FreeRoam Mode

        }
    }
    void OnNodeComplete(string nodeName)
    {
        Debug.Log($"[YarnEvent] NodeComplete: {nodeName}");
        // 노드 종료 시점에 처리할 로직
    }

    void OnDialogueComplete()
    {
        Debug.Log("[YarnEvent] DialogueComplete: entire conversation ended.");
        // 대화 전체가 끝난 뒤 처리 (UI 닫기 등)
    }


    // -----------------------
    // 2) Yarn Command Handlers
    // -----------------------

    // 예: <<LoadScene Mansion>>
    void Command_LoadScene(string sceneName)
    {
        Debug.Log($"[YarnCommand] LoadScene => {sceneName}");
        GameManager.Instance.LoadScene(sceneName);
    }

    // 예: <<BeginFreeRoam>>
    void Command_BeginFreeRoam()
    {
        // 자유 조사 모드 돌입
        Debug.Log($"[YarnCommand] BeginFreeRoam");


    }

    // 예: <<AdvanceDay>>
    void Command_AdvanceDay()
    {
        Debug.Log($"[YarnCommand] AdvanceDay");
        GameManager.Instance.AdvanceDay();
    }

    // 예: <<PlaySFX "DoorOpen">>
    void Command_PlaySFX(string sfxName)
    {
        Debug.Log($"[YarnCommand] PlaySFX => {sfxName}");
        // AudioManager.Instance.PlaySFX(sfxName);
    }

    // 예: <<CameraShake 1.2>>
    void Command_CameraShake(float intensity)
    {
        Debug.Log("$[YarnCommand] CameraShake => intensity: {intensity}");
        // CameraShaker.Shake(intensity);
    }
}
