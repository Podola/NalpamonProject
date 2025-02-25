using System;
using UnityEngine;
using UnityEngine.Playables;

public enum StoryStepType
{
    Timeline,
    Dialogue,
    Explore
}

/// <summary>
/// 한 스텝(단계)에 필요한 데이터: (어떤 씬에서) (타임라인/대화/조사)을 진행할지
/// </summary>
[Serializable]
public class StoryStep
{
    public string id;                 // 스텝 ID (디버그용)
    public StoryStepType stepType;    // Timeline / Dialogue / Explore

    [Header("공통")]
    public string sceneName;          // 이 스텝이 실행될 씬

    [Header("Timeline 전용")]
    public PlayableAsset timelineAsset;  // 타임라인 파일 (ex. a-1, b-1 등)

    [Header("Dialogue 전용")]
    public string dialogueNode;       // Yarn 대화 노드명 (ex. "Dialogue-a1")
}
