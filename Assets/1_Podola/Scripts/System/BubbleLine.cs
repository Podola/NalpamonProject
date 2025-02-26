using UnityEngine;

[System.Serializable]
public class BubbleLine
{
    [Tooltip("대사를 말하는 화자")]
    public string speaker;

    [Tooltip("실제로 표시될 대사 텍스트")]
    [TextArea]
    public string text;

    [Tooltip("이 줄을 몇 초 동안 표시할지")]
    public float duration = 2f;
}
