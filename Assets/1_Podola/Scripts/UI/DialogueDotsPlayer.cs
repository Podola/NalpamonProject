using UnityEngine;
using DG.Tweening;

public class DialogueDotsPlayer : MonoBehaviour
{
    public Animator dotsAnimator;
    public float duration = 0.5f;
    public RectTransform[] dotRects;
    private Vector2[] initPos;

    void Awake()
    {
        // dotRects 배열의 길이에 맞게 initPos 배열 초기화
        if (dotRects != null && dotRects.Length > 0)
        {
            initPos = new Vector2[dotRects.Length];
            for (int i = 0; i < dotRects.Length; i++)
            {
                if (dotRects[i] != null)
                {
                    initPos[i] = dotRects[i].anchoredPosition;
                }
            }
        }
    }

    private void OnEnable()
    {
        if (dotsAnimator != null)
        {
            dotsAnimator.enabled = false;
        }
        else
        {
            Debug.LogWarning("dotsAnimator is null");
        }
        
        // initPos가 정상적으로 초기화되어 있는지 확인
        if (initPos == null)
        {
            Debug.LogWarning("initPos is not initialized");
            return;
        }

        for (int i = 0; i < dotRects.Length; i++)
        {
            if (dotRects[i] != null)
            {
                dotRects[i].DOAnchorPos(initPos[i], duration);
            }
        }
    }

    private void OnDisable()
    {
        if (dotsAnimator != null)
        {
            dotsAnimator.enabled = true;
        }
    }
}
