using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSoundHandler : MonoBehaviour, IPointerUpHandler
{
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("On Pointer enter");
    }
}
