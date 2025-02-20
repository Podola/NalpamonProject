using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBubbleController : MonoBehaviour
{
    [Header("화자 (Speaker ID)")]
    public string speaker;

    [Header("UI 설정")]
    public TextMeshProUGUI textMeshPro;   // UI 텍스트
    public Image bgImage;                 // 말풍선 배경

    [Header("Fade 설정")]
    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 0.5f;

    [Header("말풍선 유지 시간")]
    public float showDuration = 1.3f;

    private Tween currentFadeTween;
    private Coroutine bubbleHideRoutine;

    /// <summary>
    /// 새 BubbleLine을 표시한다. continuous=true면 같은 화자의 연속 대사로 간주(페이드인 생략).
    /// </summary>
    public void UpdateBubble(BubbleLine newLine, bool continuous)
    {
        // 이미 Hide 예정 코루틴이 돌고 있다면 중단
        if (bubbleHideRoutine != null)
        {
            StopCoroutine(bubbleHideRoutine);
            bubbleHideRoutine = null;
        }
        // 페이드 트윈도 초기화
        currentFadeTween?.Kill();

        // 말풍선 오브젝트 활성화
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        // 텍스트 갱신
        textMeshPro.text = newLine.bubbleText;

        if (continuous)
        {
            // 같은 화자라면 페이드인 생략하고, 투명도 즉시 1로
            SetAlphaInstant(1f);
        }
        else
        {
            // 새 화자(또는 화자 교체)라면 페이드 인
            FadeInBubble();
        }

        // 일정 시간이 지나면 자동 페이드 아웃
        bubbleHideRoutine = StartCoroutine(AutoHideBubble(showDuration));
    }

    public void HideImmediate()
    {
        currentFadeTween?.Kill();
        bubbleHideRoutine = null;

        SetAlphaInstant(0f);
        gameObject.SetActive(false);
    }

    public void FadeOutBubble()
    {
        currentFadeTween?.Kill();

        // 텍스트
        currentFadeTween = textMeshPro.DOFade(0f, fadeOutDuration).SetUpdate(true);
        // 배경
        bgImage.DOFade(0f, fadeOutDuration).SetUpdate(true);
    }

    public void FadeInBubble()
    {
        SetAlphaInstant(0f);

        textMeshPro.DOFade(1f, fadeInDuration).SetUpdate(true);
        bgImage.DOFade(1f, fadeInDuration).SetUpdate(true);
    }

    private void SetAlphaInstant(float alpha)
    {
        var tc = textMeshPro.color;
        textMeshPro.color = new Color(tc.r, tc.g, tc.b, alpha);

        var bgc = bgImage.color;
        bgImage.color = new Color(bgc.r, bgc.g, bgc.b, alpha);
    }

    private IEnumerator AutoHideBubble(float duration)
    {
        yield return new WaitForSeconds(duration);
        FadeOutBubble();
    }
}
