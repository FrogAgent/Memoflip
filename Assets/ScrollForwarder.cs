using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollForwarder : MonoBehaviour, IScrollHandler
{
    public ScrollRect scrollRect;

    public void OnScroll(PointerEventData eventData)
    {
        if (scrollRect != null)
        {
            scrollRect.OnScroll(eventData);
        }
    }
}
