using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class BubbleController : MonoBehaviour
{
    [Tooltip("말풍선 배경")]
    public SpriteRenderer bubbleSprite;

    [Tooltip("말풍선 텍스트")]
    public TMP_Text bubbleText;

    private float duration = 1.2f;  // 말풍선 기본 유지 시간(초), BubbleSO에서 커스텀 가능

    private float fadeOutDuration = 0.3f;   // 페이드아웃 시간(초)

    public string speaker;  // 화자

    public void ShowBubble(string text, float durationOverride)
    {
        StopAllCoroutines();
        DOTween.Kill(bubbleSprite);
        DOTween.Kill(bubbleText);

        bubbleText.text = text;
        Color spColor = bubbleSprite.color;
        spColor.a = 1f;
        bubbleSprite.color = spColor;

        Color txtColor = bubbleText.color;
        txtColor.a = 1f;
        bubbleText.color = txtColor;

        float displayTime = (durationOverride > 0f) ? durationOverride : duration;
        StartCoroutine(FadeOutAfter(displayTime));
    }

    private IEnumerator FadeOutAfter(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        bubbleSprite.DOFade(0f, fadeOutDuration);
        bubbleText.DOFade(0f, fadeOutDuration);
        yield return new WaitForSeconds(fadeOutDuration);
    }

    private void OnValidate()
    {
        speaker = gameObject.name;
    }

    public void HideImmediate()
    {
        StopAllCoroutines();
        DOTween.Kill(bubbleSprite);
        DOTween.Kill(bubbleText);

        var spColor = bubbleSprite.color; 
        spColor.a = 0f;
        bubbleSprite.color = spColor;

        var txtColor = bubbleText.color;
        txtColor.a = 0f;
        bubbleText.color = txtColor;
    }
}
