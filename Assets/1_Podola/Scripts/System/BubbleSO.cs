using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBubbleSO", menuName = "SO/BubbleSO")]
public class BubbleSO : ScriptableObject
{
    [Tooltip("타임라인 컷신에서 순차적으로 표시될 말풍선 대사 목록")]
    public List<BubbleLine> lines = new();
}
