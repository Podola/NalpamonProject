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

        // �̺�Ʈ ���
        dr.onNodeStart.AddListener(OnNodeStart);
        dr.onNodeComplete.AddListener(OnNodeComplete);
        dr.onDialogueComplete.AddListener(OnDialogueComplete);

        // ���(Command) ���
        dr.AddCommandHandler<string>("LoadScene", Command_LoadScene);
        dr.AddCommandHandler("BeginFreeRoam", Command_BeginFreeRoam);
        dr.AddCommandHandler("AdvanceDay", Command_AdvanceDay);
        dr.AddCommandHandler<string>("PlaySFX", Command_PlaySFX);
        dr.AddCommandHandler<float>("CameraShake", Command_CameraShake);
        dr.AddCommandHandler<string, string>("LoadLeftPortrait", Command_LoadLeftPortrait);
        dr.AddCommandHandler("UnLoadLeftPortrait", Command_UnLoadLeftPortrait);
    }

    // -----------------------
    // 1) DialogueRunner Events
    // -----------------------
    void OnNodeStart(string nodeName)
    {
        Debug.Log($"[YarnEvent] NodeStart: {nodeName}");

        // ��: Ư�� ��� ���� �� ī�޶� ��ȯ, Ư�� UI ���� ��
        if (nodeName == "Mansion_1F_Explorer")
        {
            Debug.Log("�Ľ�Ÿ ���ÿ� ����. ���� ���� ��� ����");
            // FreeRoam Mode

        }
    }
    void OnNodeComplete(string nodeName)
    {
        Debug.Log($"[YarnEvent] NodeComplete: {nodeName}");
        // ��� ���� ������ ó���� ����
    }

    void OnDialogueComplete()
    {
        Debug.Log("[YarnEvent] DialogueComplete: entire conversation ended.");
        // ��ȭ ��ü�� ���� �� ó�� 
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

    // ��: <<BeginFreeRoam>>
    void Command_BeginFreeRoam()
    {
        // ���� ���� ��� ����
        Debug.Log($"[YarnCommand] BeginFreeRoam");


    }

    // ��: <<AdvanceDay>>
    void Command_AdvanceDay()
    {
        Debug.Log($"[YarnCommand] AdvanceDay");
        GameManager.Instance.AdvanceDay();
    }

    // ��: <<PlaySFX "DoorOpen">>
    void Command_PlaySFX(string sfxName)
    {
        Debug.Log($"[YarnCommand] PlaySFX => {sfxName}");
        // AudioManager.Instance.PlaySFX(sfxName);
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
