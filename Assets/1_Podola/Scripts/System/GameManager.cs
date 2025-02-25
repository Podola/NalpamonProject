using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using Yarn.Unity;

/// <summary>
/// 게임 런타임 상태
/// </summary>
public enum GameState
{
    Explore,   // 2D 조사/이동 상태
    Dialogue,  // YarnSpinner 대화 상태
    Timeline   // Unity Timeline 재생 상태
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // 현재 진행할 스토리 스텝 목록
    private List<StoryStep> currentSteps;
    private int currentStepIndex = -1;

    [Header("현재 게임 상태")]
    public GameState currentState = GameState.Explore;

    [Header("Yarn 대화 러너")]
    private DialogueRunner dialogueRunner; 

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(transform.root.gameObject);   // DDOL GlobalManager Prefab
            dialogueRunner.onDialogueComplete.AddListener(OnDialogueComplete);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ChapterManager에서 스토리 스텝 목록을 전달받음
    /// </summary>
    public void LoadStorySteps(List<StoryStep> steps)
    {
        currentSteps = steps;
        currentStepIndex = -1;
    }

    /// <summary>
    /// 스토리 시퀀스 시작
    /// </summary>
    public void StartStorySequence()
    {
        NextStep();
    }

    private void NextStep()
    {
        currentStepIndex++;
        if (currentSteps == null || currentStepIndex >= currentSteps.Count)
        {
            Debug.Log("[GameManager] 모든 스텝을 완료했습니다.");
            // 여기서 챕터 끝
            ChapterManager.Instance?.OnChapterEnd();
            return;
        }

        var step = currentSteps[currentStepIndex];
        PlayStoryStep(step);
    }

    private void PlayStoryStep(StoryStep step)
    {
        Debug.Log($"[GameManager] 스텝 실행: {step.id} ({step.stepType})");
        switch (step.stepType)
        {
            case StoryStepType.Timeline:
                StartCoroutine(PlayTimelineStep(step));
                break;
            case StoryStepType.Dialogue:
                StartCoroutine(PlayDialogueStep(step));
                break;
            case StoryStepType.Explore:
                StartCoroutine(PlayExploreStep(step));
                break;
        }
    }

    /// <summary>
    /// Timeline 스텝
    /// </summary>
    private IEnumerator PlayTimelineStep(StoryStep step)
    {
        ChangeState(GameState.Timeline);

        // 씬 로드
        yield return LoadSceneRoutine(step.sceneName);

        // 타임라인 오브젝트 찾기 (timelineAsset.name 과 동일한 이름)
        GameObject directorGO = GameObject.Find(step.timelineAsset.name);
        if (directorGO == null)
        {
            Debug.LogWarning($"Timeline 오브젝트 '{step.timelineAsset.name}' 없음.");
            NextStep();
            yield break;
        }
        var director = directorGO.GetComponent<PlayableDirector>();
        if (director == null)
        {
            Debug.LogWarning("PlayableDirector 컴포넌트가 없습니다.");
            NextStep();
            yield break;
        }

        // 타임라인 재생
        director.playableAsset = step.timelineAsset;
        bool isStopped = false;
        director.stopped += _ => { isStopped = true; };
        director.time = 0;
        director.Play();
        Debug.Log($"[Timeline] {step.id} 재생 시작 (씬: {step.sceneName})");

        // 스킵 처리
        while (!isStopped)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                director.time = director.duration;
                Debug.Log($"[Timeline] {step.id} 스킵");
            }
            yield return null;
        }

        Debug.Log($"[Timeline] {step.id} 종료");
        NextStep();
    }

    /// <summary>
    /// Dialogue 스텝
    /// </summary>
    private IEnumerator PlayDialogueStep(StoryStep step)
    {
        ChangeState(GameState.Dialogue);

        // 씬 로드
        yield return LoadSceneRoutine(step.sceneName);

        Debug.Log($"[Dialogue] {step.id} (노드: {step.dialogueNode})");
        if (dialogueRunner != null && !string.IsNullOrEmpty(step.dialogueNode))
        {
            dialogueRunner.StartDialogue(step.dialogueNode);
        }
        else
        {
            Debug.LogWarning($"대화 노드가 유효하지 않음: {step.dialogueNode}");
            NextStep();
        }
    }

    /// <summary>
    /// Yarn 대화가 끝나면 호출 (dialogueRunner.onDialogueComplete)
    /// </summary>
    private void OnDialogueComplete()
    {
        Debug.Log("[Dialogue] 종료 -> NextStep");
        NextStep();
    }

    /// <summary>
    /// Explore 스텝 (자유조사)
    /// </summary>
    private IEnumerator PlayExploreStep(StoryStep step)
    {
        ChangeState(GameState.Explore);

        // 씬 로드
        yield return LoadSceneRoutine(step.sceneName);

        Debug.Log($"[Explore] {step.id} - 자유조사 시작 (씬: {step.sceneName}).");
        // 여기서 플레이어가 2D 이동 + 조사 가능
        // 조사 완료 시점에 OnExploreComplete()를 외부(씬 스크립트)에서 호출
    }

    /// <summary>
    /// 조사 완료(오브젝트 A와 상호작용) 시, 씬 스크립트에서 호출
    /// </summary>
    public void OnExploreComplete()
    {
        Debug.Log("[Explore] 조사 완료 -> NextStep");
        NextStep();
    }

    /// <summary>
    /// 씬 로딩 공통 루틴
    /// </summary>
    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        // 이미 그 씬이면 생략
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            yield break;
        }

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        yield return new WaitUntil(() => op.isDone);
        Debug.Log($"씬 로드 완료: {sceneName}");
    }

    private void ChangeState(GameState newState)
    {
        currentState = newState;
        Debug.Log($"[GameManager] GameState -> {newState}");
    }

    
}

