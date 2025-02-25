using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class PlayableDirectorController : MonoBehaviour
{
    [Header("이 타임라인에서 사용할 BubbleSO")]
    public BubbleSO bubbleSO;

    private PlayableDirector director;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();

    }

    private void Start()
    {
        // 씬에서 바로 Play해볼 경우
        if (BubbleManager.Instance != null && bubbleSO != null)
        {
            // 씬에 있는 CharBubble 목록 갱신
            BubbleManager.Instance.UpdateBubbleControllers();

            // 전부 숨김
            BubbleManager.Instance.HideAllBubbles();

            // 2) bubbleSO 등록
            if (bubbleSO != null)
            {
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
}
