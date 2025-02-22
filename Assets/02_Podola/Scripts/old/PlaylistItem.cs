using UnityEngine;

public enum PlaylistItemType
{
    Timeline,
    Dialogue
}

[System.Serializable]
public class PlaylistItem
{
    [Tooltip("컷신을 식별할 고유 ID입니다. 타임라인 컷신의 경우 SceneTimelineMapping의 timelineAsset의 이름(확장자 제외)을 사용하고, 대화 컷신은 Yarn 노드명을 사용합니다.")]
    public string id;

    public PlaylistItemType itemType;

    [Tooltip("타임라인 컷신일 경우, 해당 매핑을 할당합니다.")]
    public SceneTimelineMapping sceneTimelineMapping;

    [Tooltip("대화 컷신일 경우 실행할 Yarn 노드 이름입니다.")]
    public string dialogueNode;

    [HideInInspector]
    public bool selected;  // 인스펙터에서 실행 여부 체크용
}
