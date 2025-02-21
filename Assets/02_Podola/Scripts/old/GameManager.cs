using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Yarn.Unity;

public enum PlaylistItemType { Timeline, Dialogue }

[System.Serializable]
public class PlaylistItem {
    [Tooltip("이 컷신을 식별할 고유 ID입니다. Yarn 커맨드 호출 시 이 ID를 사용합니다.")]
    public string id;

    public PlaylistItemType itemType;

    [Tooltip("타임라인 컷신일 경우 할당할 PlayableDirector 컴포넌트입니다.")]
    public PlayableDirector timeline;

    [Tooltip("대화 컷신일 경우 실행할 Yarn 노드 이름입니다.")]
    public string dialogueNode;

    [HideInInspector]
    public bool selected; // 인스펙터에서 선택 여부를 관리
}

public class GameManager : MonoBehaviour
{
    [Header("타임라인 및 대화 컷신")]
    public List<PlaylistItem> playlist = new List<PlaylistItem>();

    [Header("Debug Logs (런타임)")]
    public List<string> debugLogs = new List<string>();
    private DialogueRunner dialogueRunner;

    void Awake()
    {
        dialogueRunner = FindFirstObjectByType<DialogueRunner>();
    }

    // 인스펙터에서 선택된 모든 컷신 아이템을 실행
    public void PlaySelectedItems() 
    {
        foreach (var item in playlist) 
        {
            if (item.selected) 
            {
                PlayCutsceneItem(item);
            }
        }
    }

    // 단일 PlaylistItem을 실행하는 내부 함수
    private void PlayCutsceneItem(PlaylistItem item) 
    {
        if (item.itemType == PlaylistItemType.Timeline) 
        {
            item.timeline.Play();
        } 
        else if (item.itemType == PlaylistItemType.Dialogue) 
        {
            dialogueRunner.StartDialogue(item.dialogueNode);
        }
    }

    // Yarn 스크립트에서 <<PlayCutscene "CutsceneID">> 형식으로 호출하면 해당 ID를 가진 컷신을 실행
    [YarnCommand("PlayCutscene")]
    public void PlayCutscene(string cutsceneID) 
    {
        PlaylistItem item = playlist.Find(x => x.id == cutsceneID);
        PlayCutsceneItem(item);
    }

     private void Log(string message) 
     {
        Debug.Log(message);
        debugLogs.Add(message);
        // 로그가 너무 많아지지 않도록 제한
        if (debugLogs.Count > 50) {
            debugLogs.RemoveAt(0);
        }
    }
}
