using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnimationUI : MonoBehaviour
{
    [SerializeField] ToggleButton _playBtn;
    [SerializeField] ToggleButton _soundBtn;
    [SerializeField] Button _replayButton;
    [SerializeField] RectTransform _controllerContainer;
    [SerializeField] Slider _animationSlider;
    private void Awake()
    {

    }
    private void OnDestroy()
    {
        
    }
    private void OnEnable()
    {
        UpdatePause(true);
    }
    private void UpdatePause(object o)
    {
        _playBtn.SetOn((bool)o);
    }

    private void UpdateSliderNormalizer(object o)
    {
        float normalizeValue =(float)o;
        _animationSlider.SetValueWithoutNotify(normalizeValue);
    }
    public void ShowControl()
    {
        _controllerContainer.DOKill();
        var oldPos = _controllerContainer.anchoredPosition;
        _controllerContainer.DOAnchorPosY(0,Mathf.Lerp(0,0.5f, Mathf.Abs(oldPos.y)/100));
    }
    public void HideControl()
    {
        _controllerContainer.DOKill();
        var oldPos = _controllerContainer.anchoredPosition;
        _controllerContainer.DOAnchorPosY(-100, Mathf.Lerp(0.5f, 0, Mathf.Abs(oldPos.y) / 100));
    }
}
