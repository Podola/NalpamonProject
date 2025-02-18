// 전체 말풍선 대사 시퀀스를 관리하는 SO
using UnityEngine;

[CreateAssetMenu(fileName = "BubbleSO", menuName = "Bubble/BubbleSO")]
public class BubbleSO : ScriptableObject
{
    [Tooltip("대화 시퀀스에 포함될 대사 줄들 (순서대로)")]
    public BubbleLine[] lines;
}