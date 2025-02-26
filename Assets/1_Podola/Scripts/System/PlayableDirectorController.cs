using UnityEngine;
using UnityEngine.Playables;
using Yarn.Unity;

[RequireComponent(typeof(PlayableDirector))]
public class PlayableDirectorController : MonoBehaviour
{
    [Header("이 타임라인에서 사용할 BubbleSO")]
    public BubbleSO bubbleSO;
    public string dialogueNode = "";
    private PlayableDirector director;
    private DialogueRunner dialogueRunner;
    private DialogueAdvanceInput advanceInput;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        dialogueRunner = GameManager.Instance.GetDialogueRunner();
        advanceInput = dialogueRunner.GetComponent<DialogueAdvanceInput>();
        advanceInput.enabled = false;
    }

    private void Start()
    {
        // bubbleSO가 등록된 경우 진행
        if(bubbleSO != null)
        {
            // 씬에서 바로 Play해볼 경우
            if (BubbleManager.Instance != null && bubbleSO != null)
            {
                // 씬에 있는 CharBubble 목록 갱신
                BubbleManager.Instance.UpdateBubbleControllers();

                // 전부 숨김
                BubbleManager.Instance.HideAllBubbles();

                // 2) bubbleSO 등록
            
                BubbleManager.Instance.RegisterBubbleSO(bubbleSO);
                Debug.Log($"[{name}]: BubbleSO 등록 완료 ({bubbleSO.name})");
            }
        }
       
    }

    private void OnValidate()
    {
        if (director == null)
        {
            director = GetComponent<PlayableDirector>();
        }
        if (director != null && director.playableAsset != null)
        {
            string assetName = director.playableAsset.name;
            if (!string.IsNullOrEmpty(assetName) && gameObject.name != assetName)
            {
                Debug.Log($"Renaming Director: {gameObject.name} -> {assetName}");
                gameObject.name = assetName;
            }
        }
    }
    public void OnBubbleSignal()
    {
        BubbleManager.Instance.DisplayBubble();
        Debug.Log($"[PlayableDirectorController] 말풍선 표시");

    }

    public void OnBeginDialogueSignal()
    {
      if (dialogueRunner != null && !string.IsNullOrEmpty(dialogueNode))
        {
            Debug.Log($"[PlayableDirectorController] Yarn 노드 '{dialogueNode}' 시작");
            dialogueRunner.StartDialogue(dialogueNode);
        }
    }

    public void OnNextDialogueSignal()
    {
        if (dialogueRunner != null && dialogueRunner.IsDialogueRunning)
        {
            Debug.Log("[PlayableDirectorController] NextDialogueSignal에 의해 다음 대화 진행");
            dialogueRunner.Dialogue.Continue();
        }
        else
        {
            Debug.LogWarning("[PlayableDirectorController] 현재 DialogueRunner.Dialogue가 null 또는 Running 상태가 아닙니다.");
        }
    }
}
