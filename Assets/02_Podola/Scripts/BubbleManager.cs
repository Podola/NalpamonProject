using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    public static BubbleManager Instance;

    [Tooltip("현재 타임라인 컷에 등록된 BubbleSO")]
    public BubbleSO currentBubble;

    [Tooltip("현재 씬의 캐릭터 말풍선 컨트롤러 목록")]
    public List<CharBubble> bubbles = new List<CharBubble>();
    private int currentIndex = 0;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateBubbles()
    {
        // 현재 씬에 있는 모든 CharBubble 다시 수집
        bubbles = new List<CharBubble>(FindObjectsOfType<CharBubble>());
    }

    public void RegisterBubbleSO(BubbleSO bubble)
    {
        currentBubble = bubble;
        currentIndex = 0;
        Debug.Log("Bubble registered: " + bubble.name);
    }

    /// <summary>
    /// Timeline 시그널 트랙 등에서 호출해, currentBubble의 특정 라인(혹은 전 라인)을 표시
    /// </summary>
    public void DisplayBubble()
    {
        var line = currentBubble.lines[currentIndex];
        // Speaker 이름으로 CharBubble 찾기
        CharBubble target = bubbles.Find(b => b.speaker.Equals(line.speaker));
        if (target != null)
        {
            // CharBubble가 말풍선을 표시
            // ShowBubble(string text, float duration) 형태에 맞게
            target.ShowBubble(line.text, target.duration);
        }
        else
        {
            Debug.LogWarning("No CharBubble found for speaker: " + line.speaker);
        }

        currentIndex++;
    }

    public void HideAllBubbles()
    {
        if (bubbles == null) return;
        foreach (var bubble in bubbles)
        {
            if (bubble != null)
            {
                bubble.HideImmediate();
            }
        }
    }
}
