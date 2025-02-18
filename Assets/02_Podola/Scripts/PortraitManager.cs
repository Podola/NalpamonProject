using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PortraitManager : MonoBehaviour
{
    public RawImage leftPortraitImage;
    public RawImage rightPortraitImage;

    public float fadeDuration;

    void Awake()
    {
        leftPortraitImage.gameObject.SetActive(false);
        rightPortraitImage.gameObject.SetActive(false);
    }

    public void ChangeLeftPortraitImage(string portraitName)
    {
        if(!leftPortraitImage.gameObject.activeSelf)
        {
            leftPortraitImage.gameObject.SetActive(true);
        }
        Sprite sprite = Resources.Load<Sprite>("초상화/" + portraitName);
         leftPortraitImage.texture = sprite.texture;

        FadeIn(leftPortraitImage);
    }

    public void ChangeRightPortraitImage(string portraitName)
    {
          if(!rightPortraitImage.gameObject.activeSelf)
        {
            rightPortraitImage.gameObject.SetActive(true);
        }
        Sprite sprite = Resources.Load<Sprite>("초상화/" + portraitName);
        rightPortraitImage.texture = sprite.texture;

        FadeIn(rightPortraitImage);
    }

    public void HideLeftPortraitImage()
    {
        FadeOut(leftPortraitImage);
    }

    public void HideRightPortraitImage()
    {
        FadeOut(rightPortraitImage);
    }

    void FadeIn(RawImage image)
    {
        Color c = image.color;
        c.a = 0f;
        image.color = c;

        image.DOFade(1f, fadeDuration);
    }

    void FadeOut(RawImage image)
    {
        image.DOFade(0f, fadeDuration);
        image.gameObject.SetActive(false);
    }
}
