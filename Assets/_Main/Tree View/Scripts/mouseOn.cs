using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.EventSystems;

public class mouseOn : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
       
        Scroll.IsPanelLeft = true;
        Scroll.TogglePanelPosition();
        Rotate.IsRotated = true;
        Rotate.OnButtonClick();
    }


    public scroll Scroll;
    public rotateButton Rotate;
    private void OnMouseEnter()
    {
        Scroll.IsPanelLeft = true;
        Scroll.TogglePanelPosition();
       Rotate.IsRotated = true;
        Rotate.OnButtonClick();

    }
    private void Reset()
    {
        Scroll = transform.Find("TreeViewMovent").GetComponent<scroll>();
        Rotate = transform.Find("TreeViewMovent").GetComponent<rotateButton>();
      
    }
}
