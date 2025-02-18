// 말풍선 하나의 화자, 대사 내용
using UnityEngine;

[System.Serializable]
public class BubbleLine
{
   [Tooltip("대사를 말하는 화자 ID (예: 조슈, 탐정 등)")]
    public string speaker;

    [TextArea]
    [Tooltip("대사 내용")]
    public string bubbleText;
}
