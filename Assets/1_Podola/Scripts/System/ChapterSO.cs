using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 한 챕터가 가진 스토리 스텝(타임라인/대화/조사)의 순서 등을 정의
/// </summary>
[CreateAssetMenu(fileName = "NewChapterSO", menuName = "SO/ChapterSO")]
public class ChapterSO : ScriptableObject
{
    public string chapterTitle;
    public List<StoryStep> storySteps;
}
