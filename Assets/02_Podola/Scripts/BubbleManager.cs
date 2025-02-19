using UnityEngine;
using UnityEngine.Rendering;

public class BubbleManager : MonoBehaviour
{
    public BubbleSO bubbleSO;
    public BubbleController[] bubbleControllers;

    public int currentLineIndex = 0;            // bubbleSO의 index
    public bool bubbleActive = false;           // 말풍선 기능 사용 여부
    private string previousSpeaker = "";        // 이전 대사의 화자 기록

    void Start()
    {
        // 시작 시 모든 말풍선을 숨김
        foreach(var c in bubbleControllers)
        {
            c.InActive();
        }
        currentLineIndex = 0;
        bubbleActive = false;
        previousSpeaker = "";
    }

    void Update()
    {
        if(bubbleSO.lines.Length == 0)
        {
            return;
        }

        // LeftShift 입력으로 대사 진행 (실제 구현은 타임라인 Signal 이벤트로 제어)
        if(bubbleActive && Input.GetKeyDown(KeyCode.LeftShift))
        {
            NextBubbleLine();
        }
    }

    // 현재 인덱스의 대사를 업데이트
    void UpdateCurrentBubble()
    {
        BubbleLine line = bubbleSO.lines[currentLineIndex];
        
        // 현재 대사의 화자
        string currentSpeaker = line.speaker;

        // 해당 화자의 BubbleController 찾기
        BubbleController target = System.Array.Find(bubbleControllers, s => s.speaker.Equals(currentSpeaker));
        if(target == null)
        {
            Debug.LogWarning("Speaker " + line.speaker + "에 해당하는 BubbleController를 찾을 수 없습니다.");
            return;
        }

        // 같은 화자의 연속 대사인지 확인
        bool continuous = currentSpeaker.Equals(previousSpeaker);
        target.UpdateBubble(line, continuous);

        previousSpeaker = currentSpeaker;
    }

    // 다음 대사로 전환
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

    // 외부에서 이벤트로 대화 시작 시 호출
    public void StartBubble(int index = 0)
    {
         bubbleActive = true;
         currentLineIndex = index;
         previousSpeaker = "";
         UpdateCurrentBubble();
    }

    void EndBubble()
    {
        bubbleActive = false;
        foreach(var c in bubbleControllers)
        {
            c.InActive();
        }
    }
}
