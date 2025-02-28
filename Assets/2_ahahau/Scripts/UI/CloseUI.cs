using UnityEngine;
using UnityEngine.EventSystems;

public class CloseUI : MonoBehaviour, IPointerClickHandler
{
    public GameObject TopUI;
    public void OnPointerClick(PointerEventData eventData)
    {
        TopUI.SetActive(false);
    }
}
