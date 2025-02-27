using System.Collections.Generic;
using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    public static BubbleManager Instance;

    private BubbleSO currentBubbleSO; // 현재 타임라인 컷신에 등록된 BubbleSO

    private List<BubbleController> bubbleControllers = new List<BubbleController>();    // 현재 씬에 존재하는 BubbleController 목록
    private int currentIndex = -1;
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

    public void UpdateBubbleControllers()
    {
        // 현재 씬에 존재하는 BubbleController 재수집
        bubbleControllers = new List<BubbleController>(Object.FindObjectsByType<BubbleController>(FindObjectsInactive.Include, FindObjectsSortMode.None));
    }

    public void RegisterBubbleSO(BubbleSO bubbleSO)
    {
        currentBubbleSO = bubbleSO;
        currentIndex = -1;
        Debug.Log("Bubble registered: " + bubbleSO.name);
    }

    /// <summary>
    /// Timeline 시그널 트랙 등에서 호출해, currentBubble의 특정 라인(혹은 전 라인)을 표시
    /// </summary>
    public void DisplayBubble()
    {
        if(currentBubbleSO == null)
        {
            Debug.LogWarning("[BubbleManager] BubbleSO가 null입니다다");
            return;
        }

        currentIndex++;
        if(currentIndex <0 || currentIndex >= currentBubbleSO.lines.Count)
        {
            Debug.LogWarning("[BubbleManager] currentIndex가 범위 밖입니다");
            return;

        }
        var line = currentBubbleSO.lines[currentIndex];
        // Speaker 이름으로 BubbleController 찾기
        BubbleController target = bubbleControllers.Find(b => b.speaker.Equals(line.speaker));
        if (target != null)
        {
            // BubbleController가 말풍선을 표시
            // ShowBubble(string text, float duration) 형태에 맞게
            target.ShowBubble(line.text, line.duration);
        }
        else
        {
            Debug.LogWarning($"[BubbleManager] line.speaker: {line.speaker}에 해당하는 BubbleController가 없습니다.: ");
        }
    }

    public void HideAllBubbles()
    {
        if (bubbleControllers == null) 
        {
            return;
        }

        foreach (var bubble in bubbleControllers)
        {
            if (bubble != null)
            {
                bubble.HideImmediate();
            }
        }
    }
}