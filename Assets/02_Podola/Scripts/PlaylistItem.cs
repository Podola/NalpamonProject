using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[System.Serializable]
public class PlaylistItem
{
    [Tooltip("컷신 고유 ID (타입에 따라 자동 동기화됨)")]
    public string id;

    public PlaylistItemType itemType;

    [Tooltip("타임라인 컷신일 경우 Scene-TimelineMapping")]
    public SceneTimelineMapping sceneMapping;

    [Tooltip("대화 컷신일 경우 Yarn 노드명")]
    public string dialogueNode;

    [HideInInspector]
    public bool selected;
}

public enum PlaylistItemType
{
    Timeline,
    Dialogue
}

[System.Serializable]
public class SceneTimelineMapping
{
    [Tooltip("타임라인 컷신 실행에 필요한 씬 이름")]
    public string sceneName;

    [Tooltip("재생할 TimelineAsset (파일명으로 식별)")]
    public TimelineAsset timelineAsset;
}