using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using Yarn.Unity;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game State")]
    public GameState currentState = GameState.Explore;

    [Header("플레이리스트(타임라인 or 대화)")]
    [Tooltip("게임의 컷신 흐름을 정의합니다. 각 항목은 타임라인 또는 대화 컷신으로 구성됩니다.")]
    public List<PlaylistItem> playlist = new List<PlaylistItem>();

    private DialogueRunner dialogueRunner;

    [Header("UI References")]
    [Tooltip("조사 파트 UI")]
    public GameObject exploreUI;
    [Tooltip("심문 파트 UI")]
    public GameObject interrogationUI;

    [Header("로그")]
    [Tooltip("컷신 실행 시 기록된 디버그 로그입니다.")]
    public List<string> debugLogs = new List<string>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            dialogueRunner = FindFirstObjectByType<DialogueRunner>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;
        UpdateUIForState();
        Log($"상태 전환: {newState}");
    }

    private void UpdateUIForState()
    {
        exploreUI.SetActive(currentState == GameState.Explore);
        interrogationUI.SetActive(currentState == GameState.Interrogation);
    }

    public void PlayCutsceneItem(PlaylistItem item)
    {
        GameState previousState = currentState;
        ChangeState(GameState.Cutscene);

        if (item.itemType == PlaylistItemType.Timeline)
        {
            if (item.sceneTimelineMapping != null && item.sceneTimelineMapping.timelineAsset != null)
            {
                StartCoroutine(PlayTimelineWithPreplacedDirector(item.sceneTimelineMapping, item.id, () =>
                {
                    ChangeState(previousState);
                }));
            }
            else
            {
                Log($"오류: 타임라인 컷신 '{item.id}'에 필요한 정보가 누락됨.");
                ChangeState(previousState);
            }
        }
        else if (item.itemType == PlaylistItemType.Dialogue)
        {
            if (dialogueRunner != null && !string.IsNullOrEmpty(item.dialogueNode))
            {
                dialogueRunner.StartDialogue(item.dialogueNode);
                Log($"대화 컷신 실행: {item.id}");
                // Yarn 대화 종료 후 상태 복귀 처리는 YarnDialogueRunner 이벤트와 연동 필요
            }
            else
            {
                Log($"오류: 대화 컷신 '{item.id}'에 유효한 노드명 또는 dialogueRunner가 없음.");
                ChangeState(previousState);
            }
        }
    }

    public void PlaySelectedItems()
    {
        foreach (var item in playlist)
        {
            if (item.selected)
            {
                PlayCutsceneItem(item);
            }
        }
    }

    private IEnumerator PlayTimelineWithPreplacedDirector(SceneTimelineMapping mapping, string timelineID, System.Action onComplete)
    {
        // 현재 활성 씬(active scene)이 mapping.sceneName과 일치하는지 확인
        Scene activeScene = SceneManager.GetActiveScene();
        if (!activeScene.name.Equals(mapping.sceneName))
        {
            Log($"현재 활성 씬({activeScene.name})과 요구 씬({mapping.sceneName})이 다릅니다. 자동 씬 전환 진행...");
            // Single 모드로 씬 전환 (기존 씬은 자동 언로드됨)
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(mapping.sceneName, LoadSceneMode.Single);
            yield return new WaitUntil(() => asyncLoad.isDone);
            Log($"씬 전환 완료: {mapping.sceneName} 로드 완료.");
        }

        // 이제 활성 씬은 mapping.sceneName이어야 합니다.
        // 미리 배치된 PlayableDirector GameObject를 찾습니다.
        // GameObject의 이름은 TimelineAsset의 이름과 동일해야 합니다.
        GameObject directorGO = GameObject.Find(mapping.timelineAsset.name);
        if (directorGO == null)
        {
            Log($"오류: '{mapping.timelineAsset.name}' 이름의 PlayableDirector GameObject를 찾을 수 없습니다.");
            yield break;
        }

        PlayableDirector director = directorGO.GetComponent<PlayableDirector>();
        if (director == null)
        {
            Log($"오류: '{mapping.timelineAsset.name}' GameObject에 PlayableDirector 컴포넌트가 없습니다.");
            yield break;
        }

        // 재생 전에 TimelineAsset을 재할당 (필요한 경우)
        director.playableAsset = mapping.timelineAsset;

        bool isStopped = false;
        director.stopped += (PlayableDirector pd) =>
        {
            isStopped = true;
        };

        director.Play();
        Log($"타임라인 컷신 실행: {timelineID}");

        yield return new WaitUntil(() => isStopped);

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
            Log($"경고: '{cutsceneID}' ID를 가진 컷신을 찾을 수 없음.");
        }
    }

    private void Log(string message)
    {
        Debug.Log(message);
        debugLogs.Add(message);
        if (debugLogs.Count > 50)
        {
            debugLogs.RemoveAt(0);
        }
    }
}
