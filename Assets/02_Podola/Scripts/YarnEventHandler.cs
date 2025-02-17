using UnityEngine;
using Yarn.Unity;

public class YarnEventHandler : MonoBehaviour
{
    private DialogueRunner dr;
    private PortraitLoader pl;

    private void Awake()
    {
        dr = FindFirstObjectByType<DialogueRunner>();
        pl = FindFirstObjectByType<PortraitLoader>();

        // 명령(Command) 등록
        dr.AddCommandHandler<string>("LoadScene", Command_LoadScene);
        dr.AddCommandHandler<string>("PlaySFX", Command_PlaySFX);
        dr.AddCommandHandler<float>("CameraShake", Command_CameraShake);
        dr.AddCommandHandler<string, string>("LoadLeftPortrait", Command_LoadLeftPortrait);
        dr.AddCommandHandler("UnLoadLeftPortrait", Command_UnLoadLeftPortrait);
        dr.AddCommandHandler<string>("PlayTimeline", Command_PlayTimeline);
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

    // 예: <<PlaySFX "DoorOpen">>
    void Command_PlaySFX(string sfxName)
    {
        Debug.Log($"[YarnCommand] PlaySFX => {sfxName}");
        // AudioManager.Instance.PlaySFX(sfxName);
    }

    // 예: <<PlayTimeline "JoshuEnter">>
    void Command_PlayTimeline(string timelineName)
    {
        Debug.Log($"[YarnCommand] PlayTimeline => {timelineName}");

    }
    // 예: <<CameraShake 1.2>>
    void Command_CameraShake(float intensity)
    {
        Debug.Log($"[YarnCommand] CameraShake => intensity: {intensity}");
        // CameraShaker.Shake(intensity);
    }

    // 예: <<LoadLeftPortrait "Joshu" "1">>
    void Command_LoadLeftPortrait(string characterName, string expression)
    {
        Debug.Log($"[YarnCommand] LoadLeftPortrait => characterName: {characterName}, expression: {expression}");
        pl.LoadLeftPortrait(characterName, expression);
    }

    // 예: <<UnLoadLeftPortrait>>
    void Command_UnLoadLeftPortrait()
    {
        Debug.Log($"[YarnCommand] UnLoadLeftPortrait");
    }
}
