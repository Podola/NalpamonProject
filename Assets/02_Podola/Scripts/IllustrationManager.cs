using DG.Tweening;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class IllustrationManager : MonoBehaviour
{
    public Image illustrationImage;
    public float fadeDuration = 0.5f;

    [Header("Events")]
    public UnityEvent onIllustrationEnd;
    void Awake()
    {
        illustrationImage.gameObject.SetActive(false);   
    }

    public void ChangeIllustrationImage(string illustrationName)
    {
        if(!illustrationImage.gameObject.activeSelf)
        {
            illustrationImage.gameObject.SetActive(true);
        }
        Sprite sprite = Resources.Load<Sprite>("일러스트/" + illustrationName);
        illustrationImage.sprite = sprite;

        FadeIn();
    }
    public void FadeIn()
    {
        Color c = illustrationImage.color;
        c.a = 0f;
        illustrationImage.color = c;

        illustrationImage.DOFade(1f, fadeDuration);
    }

    public void FadeOut()
    {
        illustrationImage.DOFade(0f, fadeDuration)
            .OnComplete(() => illustrationImage.gameObject.SetActive(false));
    }

    public void StopIllustration()
    {
        FadeOut();
        onIllustrationEnd.Invoke();
    }
}
