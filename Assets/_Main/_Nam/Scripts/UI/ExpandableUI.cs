using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpandableUI : MonoBehaviour
{
    [SerializeField] ToggleButton _expandBtn;
    [SerializeField] RectTransform _rectTr;
    RectTransform _baseParent;


    private Vector2 _baseAnchorMin;
    private Vector2 _baseAnchorMax;
    private Vector2 _basePivot;
    private Vector2 _baseSizeDelta;
    private Vector2 _baseAnchoredPosition;
    // Start is called before the first frame update
    void Start()
    {
        _expandBtn.onToggle.AddListener(ToggleFullScreen);
        _baseParent = (RectTransform)_rectTr.parent;
        _baseAnchorMin = _rectTr.anchorMin;
        _baseAnchorMax = _rectTr.anchorMax;
        _basePivot = _rectTr.pivot;
        _baseSizeDelta = _rectTr.sizeDelta;
        _baseAnchoredPosition = _rectTr.anchoredPosition;
    }

    private void ToggleFullScreen(bool newFullScreen)
    {
        if (newFullScreen)
        {
            ExpandableContainer.Instance.Expand(_rectTr);
        }
        else
        {
            _rectTr.SetParent(_baseParent);
            _rectTr.anchorMin = _baseAnchorMin;
            _rectTr.anchorMax = _baseAnchorMax;
            _rectTr.pivot = _basePivot;
            _rectTr.sizeDelta = _baseSizeDelta;
            _rectTr.anchoredPosition = _baseAnchoredPosition;
        }    
    }
}
