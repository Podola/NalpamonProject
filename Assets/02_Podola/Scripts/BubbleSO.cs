using System.Collections.Generic;
using UnityEngine;

public enum BubbleMode
{
    Character,  // 캐릭터 머리 위 말풍선
    Dialogue    // (필요 시) 다른 방식의 말풍선 UI
}

[CreateAssetMenu(fileName = "NewBubbleSO", menuName = "Bubble/BubbleSO")]
public class BubbleSO : ScriptableObject
{
    [Tooltip("말풍선 표시 모드 (캐릭터 머리 위 / 대화 상자 등)")]
    public BubbleMode mode = BubbleMode.Character;

    [Tooltip("이 말풍선(타임라인 컷신)에서 순차적으로 표시될 대사 목록")]
    public List<BubbleLine> lines = new();
}
