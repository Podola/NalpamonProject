using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StandingManager : MonoBehaviour
{
    public Image leftStandingImage;
    public Image rightStandingImage;

    public float fadeDuration;
    public float focusDuration;
    public Color normalColor = Color.white;
    public Color dimColor = new Color(0.8f, 0.8f, 1f, 1f);
    void Awake()
    {
        leftStandingImage.gameObject.SetActive(false);
        rightStandingImage.gameObject.SetActive(false);
    }

    public void ChangeLeftStandingImage(string StandingName)
    {
        if(!leftStandingImage.gameObject.activeSelf)
        {
            leftStandingImage.gameObject.SetActive(true);
        }

        Sprite sprite = Resources.Load<Sprite>("스탠딩/" + StandingName);

        // 일단 알파값을 0으로 만들어둔 뒤 교체
        Color c = leftStandingImage.color;
        c.a = 0f;
        leftStandingImage.color = c;

        // 스프라이트 교체
        leftStandingImage.sprite = sprite;

        // 그런 다음 FadeIn
        leftStandingImage.DOFade(1f, fadeDuration);
    }

    public void ChangeRightStandingImage(string StandingName)
    {
          if(!rightStandingImage.gameObject.activeSelf)
        {
            rightStandingImage.gameObject.SetActive(true);
        }

        Sprite sprite = Resources.Load<Sprite>("스탠딩/" + StandingName);
        
        Color c = rightStandingImage.color;
        c.a = 0f;
        rightStandingImage.color = c;
        rightStandingImage.sprite = sprite;
        rightStandingImage.DOFade(1f, fadeDuration);
    }

    public void HideLeftStandingImage()
    {
        FadeOut(leftStandingImage);
    }

    public void HideRightStandingImage()
    {
        FadeOut(rightStandingImage);
    }

    void FadeOut(Image image)
    {
        image.DOFade(0f, fadeDuration);
    }

    public void FocusSpeaker(string direction)
    {
        if(direction == "left")
        {
            leftStandingImage.DOColor(normalColor, focusDuration);
            rightStandingImage.DOColor(dimColor, focusDuration);
        }
        else if (direction == "right")
        {
            rightStandingImage.DOColor(normalColor, focusDuration);
            leftStandingImage.DOColor(dimColor, focusDuration);
        }
        else
        {
            leftStandingImage.DOColor(normalColor, focusDuration);
            rightStandingImage.DOColor(normalColor, focusDuration);
        }
    }
}
