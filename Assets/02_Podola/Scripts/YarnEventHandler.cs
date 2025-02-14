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

        // ���(Command) ���
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

    // ��: <<LoadScene Mansion>>
    void Command_LoadScene(string sceneName)
    {
        Debug.Log($"[YarnCommand] LoadScene => {sceneName}");
        GameManager.Instance.LoadScene(sceneName);
    }

    // ��: <<PlaySFX "DoorOpen">>
    void Command_PlaySFX(string sfxName)
    {
        Debug.Log($"[YarnCommand] PlaySFX => {sfxName}");
        // AudioManager.Instance.PlaySFX(sfxName);
    }

    // ��: <<PlayTimeline "JoshuEnter">>
    void Command_PlayTimeline(string timelineName)
    {
        Debug.Log($"[YarnCommand] PlayTimeline => {timelineName}");

    }
    // ��: <<CameraShake 1.2>>
    void Command_CameraShake(float intensity)
    {
        Debug.Log($"[YarnCommand] CameraShake => intensity: {intensity}");
        // CameraShaker.Shake(intensity);
    }

    // ��: <<LoadLeftPortrait "Joshu" "1">>
    void Command_LoadLeftPortrait(string characterName, string expression)
    {
        Debug.Log($"[YarnCommand] LoadLeftPortrait => characterName: {characterName}, expression: {expression}");
        pl.LoadLeftPortrait(characterName, expression);
    }

    // ��: <<UnLoadLeftPortrait>>
    void Command_UnLoadLeftPortrait()
    {
        Debug.Log($"[YarnCommand] UnLoadLeftPortrait");
    }
}
