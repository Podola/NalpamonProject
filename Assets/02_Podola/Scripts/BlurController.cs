using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class BlurController : MonoBehaviour
{
    public Volume volume;

    void Start()
    {
        gameObject.SetActive(false);
    }

    // 대화가 시작될 때 블러 효과 활성화
    public void EnableBlur()
    {
        gameObject.SetActive(true);
    }

    // 대화가 종료될 때 블러 효과 해제
    public void DisableBlur()
    {
        gameObject.SetActive(false);
    }
}