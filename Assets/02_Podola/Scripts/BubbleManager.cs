using UnityEngine;

public class BubbleManager : MonoBehaviour
{
    public BubbleSO bubbleSO;
    public BubbleController[] bubbleControllers;

    public int currentLineIndex = 0;
    public bool bubbleActive = false;

    void Start()
    {
        currentLineIndex = 0;
        bubbleActive = true;
        UpdateCurrentBubble();    
    }

    void Update()
    {
        if(bubbleActive && Input.GetKeyDown(KeyCode.Space))
        {
            NextBubbleLine();
        }
    }

    // 현재 인덱스의 대사를 해당 캐릭터(bubbleController)에 업데이트
    void UpdateCurrentBubble()
    {
        BubbleLine line = bubbleSO.lines[currentLineIndex];
        BubbleController target = 
            System.Array.Find(bubbleControllers, s => s.speaker.Equals(line.speaker));
        if(target != null)
        {
            target.UpdateBubble(line);
        }
        else
        {
            Debug.LogWarning("Speaker " + line.speaker + "에 해당하는 BubbleController를 찾을 수 없습니다.");
        }
    }

    public void NextBubbleLine()
    {
        if(currentLineIndex < bubbleSO.lines.Length -1)
        {
            currentLineIndex++;
            UpdateCurrentBubble();
        }
        else
        {
            EndBubble();
        }
    }

    void EndBubble()
    {
        bubbleActive = false;
        foreach(var c in bubbleControllers)
        {
            c.HideBubble();
        }
    }
}
