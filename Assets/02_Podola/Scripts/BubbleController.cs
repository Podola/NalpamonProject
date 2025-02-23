using System.Collections;
using DG.Tweening;
//using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    [Header("화자")]
    public string speaker;
    
    [Header("UI 설정")]
    public TextMeshPro textMeshPro;
    public SpriteRenderer bgRenderer;

    [Header("Feel 설정")]
    //public MMF_Player mmfPlayer;

    [Header("Fade 설정")]
    public float fadeInDuration = 0.5f;
    public float fadeOutDuration = 0.5f;

    [Header("말풍선 유지 시간")]
    public float showDuration = 1.3f;

    // 진행중인 tween과 FadeOut 코루틴을 추적
    private Tween currentFadeTween;
    private Coroutine bubbleHideRoutine;  // showDuration 후에 해당 말풍선을 Hide하는 코루틴

    // 새로운 FadeIn효과와 함께 업데이트. continuous가 true면 같은 화자 연속 발언으로 간주하여 텍스트만 업데이트
    public void UpdateBubble(BubbleLine newLine, bool continuous)
    {
         if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        // 연속 대사라면 진행중인 autoHideRoutine을 취소
        if(continuous)
        {
            if(bubbleHideRoutine != null)
            {
                StopCoroutine(bubbleHideRoutine);
                bubbleHideRoutine = null;
            }
            currentFadeTween?.Kill();

            // 텍스트만 업데이트
            textMeshPro.text = newLine.bubbleText;

            // 텍스트와 배경의 알파를 즉시 1로 설정
            Color textColor = textMeshPro.color;
            textMeshPro.color = new Color(textColor.r, textColor.g, textColor.b, 1f);
            Color bgColor = bgRenderer.color;
            bgRenderer.color = new Color(bgColor.r, bgColor.g, bgColor.b, 1f);

            //mmfPlayer.PlayFeedbacks();
        }
        else
        {
            textMeshPro.text = newLine.bubbleText;
            FadeInBubble();
            //mmfPlayer.PlayFeedbacks();
        }

         // 말풍선 자동 Hide 타이머 재설정
        bubbleHideRoutine = StartCoroutine(AutoHideBubble(showDuration));
    }

    public void Active()
    {
        gameObject.SetActive(true);
    }
    public void InActive()
    {
        gameObject.SetActive(false);
    }

    public void FadeInBubble()
    {
        // 텍스트
        Color textColor = textMeshPro.color;
        textMeshPro.DOColor(new Color(textColor.r, textColor.g, textColor.b, 1f), fadeInDuration);

        // 배경 이미지
        Color bgColor = bgRenderer.color;
        bgRenderer.DOColor(new Color(bgColor.r, bgColor.g, bgColor.b, 1f), fadeInDuration);
    }

     public void FadeOutBubble()
    {
        // 텍스트
        Color textColor = textMeshPro.color;
        textMeshPro.DOColor(new Color(textColor.r, textColor.g, textColor.b, 0f), fadeOutDuration);

        // 배경 이미지
        Color bgColor = bgRenderer.color;
        bgRenderer.DOColor(new Color(bgColor.r, bgColor.g, bgColor.b, 0f), fadeOutDuration);
    }

    IEnumerator AutoHideBubble(float duration)
    {
        yield return new WaitForSeconds(duration);
        FadeOutBubble();
    }
}
