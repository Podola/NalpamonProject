using UnityEngine;
using UnityEngine.EventSystems;


public class NoteStateMove : MonoBehaviour, IPointerClickHandler
{
    public Note MyNote;
    public void OnPointerClick(PointerEventData eventData)
    {
        NoteState noteState = GetComponentInParent<NoteState>();
        if(noteState == null) return;
        noteState.note = MyNote;
        print(MyNote);
    }
}
