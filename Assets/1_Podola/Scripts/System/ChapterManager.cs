using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 여러 챕터 중 어떤 챕터를 시작할지, 끝나면 어떻게 할지 관리하는 상위 매니저
/// GlobalManagers(DDOL)에 존재
/// </summary>
public class ChapterManager : MonoBehaviour
{
    public static ChapterManager Instance;

    [Header("챕터 데이터들(챕터1, 챕터2, ...)")]
    public List<ChapterSO> chapters;

    private int currentChapterIndex = -1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 예: ChapterManager.Instance.StartChapter(0) -> 챕터1 시작
    /// </summary>
    public void StartChapter(int chapterIndex)
    {
        if (chapterIndex < 0 || chapterIndex >= chapters.Count)
        {
            Debug.LogError($"[ChapterManager] 잘못된 챕터 인덱스: {chapterIndex}");
            return;
        }

        currentChapterIndex = chapterIndex;
        ChapterSO data = chapters[chapterIndex];

        Debug.Log($"[ChapterManager] 챕터 {chapterIndex} 시작: {data.chapterTitle}");

        // GameManager에게 이 챕터의 스텝 목록을 넘겨주고 실행
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoadStorySteps(data.storySteps);
            GameManager.Instance.StartStorySequence();
        }
    }

    /// <summary>
    /// 챕터를 전부 마쳤을 때 NextChapter()로 넘길 수 있음
    /// </summary>
    public void NextChapter()
    {
        int nextIndex = currentChapterIndex + 1;
        if (nextIndex < chapters.Count)
        {
            StartChapter(nextIndex);
        }
        else
        {
            Debug.Log("[ChapterManager] 모든 챕터를 완료했습니다!");
        }
    }

    /// <summary>
    /// GameManager가 "현재 챕터 끝"을 알려줄 때
    /// (현재 단일 챕터만 테스트하므로, 나중에 확장)
    /// </summary>
    public void OnChapterEnd()
    {
        Debug.Log($"[ChapterManager] 챕터 {currentChapterIndex+1} 끝!");
        // NextChapter(); // 필요 시 자동 진행
    }
}
