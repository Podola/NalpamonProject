using UnityEngine;

public class UIBubbleManager : MonoBehaviour
{
    [Header("대사 스크립트 (BubbleSO)")]
    public BubbleSO bubbleSO;

    [Header("화자별 UI 말풍선")]
    public UIBubbleController bubbleUp;    // 아자토스용 (위쪽)
    public UIBubbleController bubbleDown;  // 탐정용 (아래쪽)

    private int currentLineIndex = 0;   
    private bool bubbleActive = false;  
    private string previousSpeaker = "";  

    private UIBubbleController currentBubble = null; // 현재 표시중인 말풍선
    private UIBubbleController prevBubble = null;    // 이전에 표시했던 말풍선

    void Start()
    {
        // 시작 시 두 말풍선 전부 숨김
        bubbleUp.HideImmediate();
        bubbleDown.HideImmediate();

        currentLineIndex = 0;
        bubbleActive = false;
        previousSpeaker = "";
    }

    void Update()
    {
        // 임시로 LeftShift로 대사 넘기기(실제 게임에서는 타임라인 Signal 등으로 제어)
        if (bubbleActive && Input.GetKeyDown(KeyCode.LeftShift))
        {
            NextBubbleLine();
        }
    }

    public void StartBubble(int startIndex = 0)
    {
        if (bubbleSO == null || bubbleSO.lines.Length == 0)
        {
            Debug.LogWarning("BubbleSO가 없거나, lines가 비어있습니다.");
            return;
        }
        bubbleActive = true;
        currentLineIndex = startIndex;
        previousSpeaker = "";

        UpdateCurrentBubble();
    }

    private void UpdateCurrentBubble()
    {
        if (currentLineIndex >= bubbleSO.lines.Length)
        {
            EndBubble();
            return;
        }

        BubbleLine line = bubbleSO.lines[currentLineIndex];
        string currentSpeaker = line.speaker;

        // 현재 화자가 아자토스 or 탐정인지 판단
        UIBubbleController targetBubble = null;
        if (currentSpeaker == "아자토스")
        {
            targetBubble = bubbleUp;
        }
        else if (currentSpeaker == "탐정")
        {
            targetBubble = bubbleDown;
        }
        else
        {
            Debug.LogWarning($"지정된 화자({currentSpeaker})가 '아자토스'도 '탐정'도 아닙니다. 기본 처리 없음.");
            return;
        }

        // 같은 화자인지 체크
        bool continuous = (currentSpeaker == previousSpeaker);

        // 화자가 바뀌었다면, 이전 버블 페이드아웃 + 새 버블 페이드인
        if (!continuous && currentBubble != null)
        {
            // 이전 버블
            prevBubble = currentBubble;
            // 교차 페이드
            StartCoroutine(CrossFadeBubbles(prevBubble, targetBubble, line));
        }
        else
        {
            // 같은 화자면 그냥 UpdateBubble(continuous=true)
            targetBubble.UpdateBubble(line, continuous);
        }

        // currentBubble 갱신
        currentBubble = targetBubble;
        previousSpeaker = currentSpeaker;
    }

    private System.Collections.IEnumerator CrossFadeBubbles(UIBubbleController oldBubble, UIBubbleController newBubble, BubbleLine newLine)
    {
        // 우선 새 화자 말풍선 UpdateBubble(continuous=false => 페이드인)
        newBubble.UpdateBubble(newLine, false);

        // 잠시 동시에 페이드가 진행되도록 0.2~0.3초 기다림(적절히 조절)
        yield return new WaitForSeconds(0.3f);

        // 이전 말풍선을 완전히 페이드아웃되도록 대기
        oldBubble.FadeOutBubble();
        yield return new WaitForSeconds(oldBubble.fadeOutDuration);

        // 완전 투명화가 끝나면 즉시 Hide
        oldBubble.HideImmediate();
    }

    public void NextBubbleLine()
    {
        currentLineIndex++;
        if (currentLineIndex < bubbleSO.lines.Length)
        {
            UpdateCurrentBubble();
        }
        else
        {
            EndBubble();
        }
    }

    private void EndBubble()
    {
        bubbleActive = false;
        // 남아있는 말풍선도 모두 즉시 숨김
        bubbleUp.HideImmediate();
        bubbleDown.HideImmediate();
    }
}
