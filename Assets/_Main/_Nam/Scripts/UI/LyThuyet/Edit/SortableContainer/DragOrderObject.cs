using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragOrderObject : MonoBehaviour, IPointerEnterHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public System.Action onDrop;
    DragOrderContainer container = null;

    void Start()
    {
        container = GetComponentInParent<DragOrderContainer>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        container.objectBeingDragged = this.gameObject;
    }
    public void OnDrag(PointerEventData data)
    {
        // Do nothing
        // Apparently this interface needs to exist in order for BeginDrag and EndDrag to work,
        // but we don't actually have anything to do here
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (container.objectBeingDragged == this.gameObject)
        {
            container.objectBeingDragged = null;
            onDrop?.Invoke();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject objectBeingDragged = container.objectBeingDragged;
        if (objectBeingDragged != null && objectBeingDragged != this.gameObject)
        {
            objectBeingDragged.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
        }
    }
}
