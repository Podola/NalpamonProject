using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public enum GameState
{
    Explore,
    Dialogue,
    Timeline
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameState currentState = GameState.Explore;

    [Header("플레이리스트(타임라인 or 대화)")]
    public List<PlaylistItem> playlist = new List<PlaylistItem>();

    private DialogueRunner dialogueRunner;
    private BubbleManager bubbleManager;

    [Header("UI References")]
    public GameObject exploreUI;
    public GameObject interrogationUI;

    [Header("로그")]
    public List<string> debugLogs = new List<string>();

    // Playlist 연속 재생 제어용
    private List<PlaylistItem> currentSequence;
    private int currentSequenceIndex = -1;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.root.gameObject);

            // 혹시 오브젝트에 DialogueRunner나 BubbleManager가 같이 붙어있다면 직접 GetComponent해도 되고,
            // 계층 자식 등에 있으면 FindObjectOfType<BubbleManager>()로 찾아도 됩니다.
            dialogueRunner = FindFirstObjectByType<DialogueRunner>();
            bubbleManager = FindFirstObjectByType<BubbleManager>();

            // Yarn 대화 종료 이벤트
            if (dialogueRunner != null)
            {
                dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDialogueComplete()
    {
        Log("Yarn 대화 종료 → Explore로 복귀");
        ChangeState(GameState.Explore);

        // 연속 재생 중이면 다음 아이템 재생
        PlayNextItemInSequence();
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        UpdateUIForState();
        Log($"상태 전환: {newState}");
    }

    private void UpdateUIForState()
    {
        // 필요한 UI만 on/off
        exploreUI?.SetActive(currentState == GameState.Explore);
        interrogationUI?.SetActive(false);
    }

    public void PlayCutsceneItem(PlaylistItem item)
    {
        if (item == null) return;

        if (item.itemType == PlaylistItemType.Timeline)
        {
            ChangeState(GameState.Timeline);
            if (item.sceneMapping != null && item.sceneMapping.timelineAsset != null)
            {
                StartCoroutine(PlayTimelineCoroutine(item.sceneMapping, item.id, () =>
                {
                    // 타임라인 끝나면 Explore 복귀 후 연속 재생 체크
                    ChangeState(GameState.Explore);
                    PlayNextItemInSequence();
                }));
            }
            else
            {
                Log($"오류: 타임라인 '{item.id}' 설정이 잘못됨.");
            }
        }
        else if (item.itemType == PlaylistItemType.Dialogue)
        {
            ChangeState(GameState.Dialogue);
            if (dialogueRunner != null && !string.IsNullOrEmpty(item.dialogueNode))
            {
                dialogueRunner.StartDialogue(item.dialogueNode);
                Log($"대화 컷신 실행: {item.id}");
            }
            else
            {
                Log($"오류: 대화 '{item.id}'에 노드명이 없거나 dialogueRunner가 없음.");
            }
        }
    }

    /// <summary>
    /// Playlist 중 selected=true인 아이템만 연속 재생
    /// </summary>
    public void PlaySelectedItems()
    {
        currentSequence = playlist.FindAll(x => x.selected);
        currentSequenceIndex = -1;
        if (currentSequence.Count == 0)
        {
            Log("선택된 PlaylistItem이 없음.");
            return;
        }

        PlayNextItemInSequence();
    }

    private void PlayNextItemInSequence()
    {
        if (currentSequence == null) return;

        currentSequenceIndex++;
        if (currentSequenceIndex >= currentSequence.Count)
        {
            // 다 재생했으면 종료
            currentSequence = null;
            currentSequenceIndex = -1;
            Log("Playlist 시퀀스 재생 완료.");
            return;
        }

        var nextItem = currentSequence[currentSequenceIndex];
        PlayCutsceneItem(nextItem);
    }

    /// <summary>
    /// 타임라인 컷신 재생 코루틴
    /// </summary>
    private IEnumerator PlayTimelineCoroutine(SceneTimelineMapping mapping, string timelineID, System.Action onComplete)
    {
        // 씬이 다른 경우 로드
        Scene activeScene = SceneManager.GetActiveScene();
        if (!activeScene.name.Equals(mapping.sceneName))
        {
            Log($"씬 전환: {activeScene.name} → {mapping.sceneName}");
            AsyncOperation op = SceneManager.LoadSceneAsync(mapping.sceneName, LoadSceneMode.Single);
            yield return new WaitUntil(() => op.isDone);
            Log($"씬 {mapping.sceneName} 로드 완료.");
        }

        // 해당 씬에 있는 CharBubble들 다시 수집
        bubbleManager?.UpdateBubbles();

        // 타임라인 GameObject(이름=timelineAsset.name)를 찾는다
        GameObject directorGO = GameObject.Find(mapping.timelineAsset.name);
        if (directorGO == null)
        {
            Log($"오류: {mapping.timelineAsset.name} 이름의 PlayableDirector를 찾을 수 없음.");
            onComplete?.Invoke();
            yield break;
        }
        PlayableDirector director = directorGO.GetComponent<PlayableDirector>();
        if (director == null)
        {
            Log($"오류: PlayableDirector 컴포넌트 없음: {mapping.timelineAsset.name}");
            onComplete?.Invoke();
            yield break;
        }

        director.playableAsset = mapping.timelineAsset;

        bool isStopped = false;
        System.Action<PlayableDirector> onDirectorStopped = (PlayableDirector pd) => { isStopped = true; };
        director.stopped += onDirectorStopped;

        director.Play();
        Log($"타임라인 재생 시작: {timelineID}");

        // **타임라인 스킵 기능**: 코루틴 루프를 돌면서, ESC 입력 시 전체 시간을 끝으로 이동
        while (!isStopped)
        {
            if (Input.GetKeyDown(KeyCode.Escape))  // 원하는 키로 변경 가능
            {
                // 타임라인 스킵
                director.time = director.duration;
                Log($"타임라인 스킵: {timelineID}");
            }
            yield return null;
        }

        director.stopped -= onDirectorStopped;

        onComplete?.Invoke();
    }

    [YarnCommand("PlayCutscene")]
    public void PlayCutscene(string cutsceneID)
    {
        PlaylistItem item = playlist.Find(x => x.id == cutsceneID);
        if (item != null)
        {
            PlayCutsceneItem(item);
        }
        else
        {
            Log($"'{cutsceneID}' 컷신을 찾을 수 없음.");
        }
    }

    private void Log(string msg)
    {
        Debug.Log(msg);
        debugLogs.Add(msg);
        while (debugLogs.Count > 50)
            debugLogs.RemoveAt(0);
    }
}
