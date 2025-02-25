using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CharBubble : MonoBehaviour
{
    [Tooltip("말풍선 배경 SpriteRenderer")]
    public SpriteRenderer bubbleSprite;

    [Tooltip("말풍선 텍스트를 표시할 TextMeshPro 컴포넌트")]
    public TMP_Text bubbleText;

    [Tooltip("말풍선 기본 유지 시간 (초)")]
    public float duration = 1.2f;

    [Tooltip("페이드아웃 시간 (초)")]
    public float fadeOutDuration = 0.3f;

    public string speaker;

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
