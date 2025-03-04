using UnityEngine;
using UnityEngine.EventSystems;

public class ShowProviso : MonoBehaviour, IPointerClickHandler
{
    public GameObject ProvisoInfor;
    public void OnPointerClick(PointerEventData eventData)
    {
        ProvisoState provisoState = ProvisoInfor.GetComponentInParent<ProvisoState>();
        if(provisoState == null) return;
        string name = gameObject.name;
        string num = name.Replace("ProvisoIcon", "");
        int provisoNum = int.Parse(num);
        provisoNum--;
        provisoState.ChangeProviso(provisoNum); 
        ProvisoInfor.SetActive(true);
        print(provisoNum);
    }
}
