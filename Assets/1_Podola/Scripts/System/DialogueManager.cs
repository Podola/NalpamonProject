using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Yarn.Unity;
using TMPro;  // YarnSpinner Unity 통합 패키지 네임스페이스

public class DialogueManager : MonoBehaviour
{
    private DialogueRunner dialogueRunner;

    // 스탠딩 이미지 슬롯들들
    public Image standingLeft;
    public Image standingRight;
    public Image standingCenter;
    public Image standingLeft2;
    public Image standingRight2;

    [Header("스탠딩 애니메이션 설정")]
    // 페이드 애니메이션 재생 시간 설정
    public float fadeDuration = 0.5f;             // 등장/퇴장 페이드 시간
    public float expressionFadeDuration = 0.2f;   // 표정 변경 시 전환 시간

    // 스프라이트 캐시: 이미 로드된 스프라이트 재사용을 위해 저장
    private Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();

    // 현재 화면에 표시된 캐릭터 정보 저장 (위치 -> (캐릭터이름, 표정))
    private Dictionary<string, (string charName, string expression)> currentStanding = new Dictionary<string, (string, string)>();

    void Awake()
    {
        dialogueRunner = FindFirstObjectByType<DialogueRunner>();
    }
    void Start() {
        // 시작 시 모든 스탠딩 이미지 오브젝트 비활성화
        if (standingLeft) standingLeft.gameObject.SetActive(false);
        if (standingRight) standingRight.gameObject.SetActive(false);
        if (standingCenter) standingCenter.gameObject.SetActive(false);
        if (standingLeft2) standingLeft2.gameObject.SetActive(false);
        if (standingRight2) standingRight2.gameObject.SetActive(false);
    }

    /// <summary>
    /// 특정 캐릭터의 특정 표정을 원하는 위치에 표시한다.
    /// </summary>
    public IEnumerator ShowStanding(string position, string characterName, string expression)
    {
        position = position.ToLower(); // "left", "right" 소문자로 통일

        // 위치별로 어떤 Image를 쓸지
        Image targetImage = GetImageByPosition(position);
        if (targetImage == null)
        {
            Debug.LogError($"[DialogueManager] 알 수 없는 위치: {position}");
            yield break;
        }

        // 스프라이트 로드(캐시 사용)
        string resourcePath = $"스탠딩/{characterName}/{expression}";
        Debug.Log($"{resourcePath}");
        Sprite newSprite = null;
        if (!spriteCache.TryGetValue(resourcePath, out newSprite))
        {
            newSprite = Resources.Load<Sprite>(resourcePath);
            if (newSprite == null)
            {
                Debug.LogError($"[DialogueManager] 리소스 경로 '{resourcePath}'에 스탠딩 이미지가 없습니다!");
                yield break;
            }
            spriteCache[resourcePath] = newSprite;
        }

        // 현재 그 위치에 다른 캐릭터가 있나?
        bool hasStanding = currentStanding.ContainsKey(position);
        if (!hasStanding)
        {
            // 1) 해당 위치가 비어있다면 => 새 캐릭터 스탠딩 등장 (페이드인)
            currentStanding[position] = (characterName, expression);
            targetImage.sprite = newSprite;
            targetImage.color = new Color(1, 1, 1, 0);
            targetImage.gameObject.SetActive(true);

            yield return StartCoroutine(FadeAlpha(targetImage, 0f, 1f, fadeDuration));
        }
        else
        {
            // 이미 해당 위치에 어떤 캐릭터가 서 있음
            var (currCharName, currExpr) = currentStanding[position];

            if (currCharName != characterName)
            {
                // 2) 다른 캐릭터였다면 => 기존 캐릭터 페이드아웃 -> 새 캐릭터로 교체 (페이드인)
                yield return StartCoroutine(FadeAlpha(targetImage, targetImage.color.a, 0f, fadeDuration));

                // 교체
                currentStanding[position] = (characterName, expression);
                targetImage.sprite = newSprite;
                targetImage.color = new Color(1, 1, 1, 0);
                targetImage.gameObject.SetActive(true);

                yield return StartCoroutine(FadeAlpha(targetImage, 0f, 1f, fadeDuration));
            }
            else
            {
                // 3) 같은 캐릭터이고, 표정만 바꾸는 경우 => 크로스페이드(약간 투명화 -> 교체 -> 다시 불투명)
                if (currExpr != expression)
                {
                    yield return StartCoroutine(FadeAlpha(targetImage, 1f, 0.5f, expressionFadeDuration * 0.5f));
                    currentStanding[position] = (characterName, expression);

                    targetImage.sprite = newSprite;
                    yield return StartCoroutine(FadeAlpha(targetImage, 0.5f, 1f, expressionFadeDuration * 0.5f));
                }
                // 동일 표정이면 아무 변화 없음
            }
        }
    }

    /// <summary>
    /// 해당 위치의 캐릭터 스탠딩을 숨긴다.
    /// </summary>
    public IEnumerator HideStanding(string position)
    {
        position = position.ToLower();
        Image targetImage = GetImageByPosition(position);
        if (targetImage == null)
        {
            Debug.LogError($"[DialogueManager] 알 수 없는 위치: {position}");
            yield break;
        }

        if (targetImage.gameObject.activeSelf)
        {
            // 페이드아웃
            yield return StartCoroutine(FadeAlpha(targetImage, targetImage.color.a, 0f, fadeDuration));
            targetImage.gameObject.SetActive(false);
        }

        // 현재 표시 정보 제거
        if (currentStanding.ContainsKey(position))
            currentStanding.Remove(position);
    }

    // 위치 -> Image 매핑
    private Image GetImageByPosition(string position)
    {
        switch (position)
        {
            case "left":
                return standingLeft;
            case "right":
                return standingRight;
            case "left2":
                return standingLeft2;
            case "right2":
                return standingLeft2;
            case "center":
                return standingCenter;
            // 필요하다면 "center", "left2", "right2" 등 추가
        }
        return null;
    }

    // 페이드 보조 코루틴
    private IEnumerator FadeAlpha(Image image, float startAlpha, float endAlpha, float duration)
    {
        float time = 0f;
        Color c = image.color;
        c.a = startAlpha;
        image.color = c;

        // duration 동안 서서히 보간
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, t);
            c.a = currentAlpha;
            image.color = c;
            yield return null;
        }

        // 마지막 보정
        c.a = endAlpha;
        image.color = c;
    }

    // ── 여기부터 대화창 관련 ──
    [Header("대화창 Fade 설정")]
    public CanvasGroup dialogueCanvasGroup;   // 대화창 전체를 감싸는 CanvasGroup (Inspector에 할당)
    public float dialogueFadeDuration = 0.5f;   // 대화창 Fade In/Out 지속시간
    
     public IEnumerator FadeOutDialogue()
    {
        float time = 0f;
        while (time < dialogueFadeDuration)
        {
            time += Time.deltaTime;
            dialogueCanvasGroup.alpha = Mathf.Lerp(1f, 0f, time / dialogueFadeDuration);
            yield return null;
        }
        dialogueCanvasGroup.alpha = 0f;
    }

    public IEnumerator FadeInDialogue()
    {
        float time = 0f;
        while (time < dialogueFadeDuration)
        {
            time += Time.deltaTime;
            dialogueCanvasGroup.alpha = Mathf.Lerp(0f, 1f, time / dialogueFadeDuration);
            yield return null;
        }
        dialogueCanvasGroup.alpha = 1f;
    }
}
