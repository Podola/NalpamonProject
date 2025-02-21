using System.Collections;
using TMPro;
using UnityEngine;

public class BubbleController2 : MonoBehaviour
{
    public SpriteRenderer bubbleBackground;

    public TextMeshPro bubbleText;

    public void UpdateBubble()
    {

    }

    IEnumerator AutoHideBubble(float duration)
    {
        yield return new WaitForSeconds(duration);
        FadeOut();    }

    private void FadeIn()
    {

    }

    private void FadeOut()
    {
        
    }
}
