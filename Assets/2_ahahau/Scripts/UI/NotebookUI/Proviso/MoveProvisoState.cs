using UnityEngine;
using UnityEngine.EventSystems;

public class MoveProvisoState : MonoBehaviour, IPointerClickHandler
{
    public int Move;
    public void OnPointerClick(PointerEventData eventData)
    {
        ProvisoState provisoState = GetComponentInParent<ProvisoState>();
        int curProviso = (int)provisoState.proviso + Move;
        provisoState.ChangeProviso(curProviso);
    }
}
