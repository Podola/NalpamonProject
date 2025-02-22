using UnityEngine;
using UnityEngine.Timeline;

[System.Serializable]
public class SceneTimelineMapping
{
    [Tooltip("해당 컷신 실행에 필요한 씬 이름입니다. (씬이 로드되지 않았다면 동적으로 로드됩니다.)")]
    public string sceneName;

    [Tooltip("재생할 TimelineAsset입니다. 이 에셋의 파일명(확장자 제외)이 식별자로 사용됩니다.")]
    public TimelineAsset timelineAsset;
}