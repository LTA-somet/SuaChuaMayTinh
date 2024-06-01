using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NotiPanel : Singleton<NotiPanel>
{
    [SerializeField] TMPro.TextMeshProUGUI _contentText;
    [SerializeField] Button _yesBtn;
    [SerializeField] Button _noBtn;
    [SerializeField] GameObject _container;
    UnityEvent _onYes, _onNo;
    protected override void SingletonStarted()
    {
        _onYes = new UnityEvent();
        _onNo = new UnityEvent();
        _yesBtn.onClick.AddListener(Yes);
        _noBtn.onClick.AddListener(No);
    }
    public void ShowNotify(string noti, UnityAction onYes, UnityAction onNo = null)
    {
        _onYes.RemoveAllListeners();
        _onNo.RemoveAllListeners();
        _container.SetActive(true);
        _contentText.text = noti;
        if(onYes != null)
        {
            _onYes.AddListener(onYes);
        }
        if(onNo != null)
        {
            _onNo.AddListener(onNo);
        }
    }
    public void Yes()
    {
        _onYes?.Invoke();
        _onYes?.RemoveAllListeners();
        _onNo?.RemoveAllListeners();
        _container.SetActive(false);
    }
    public void No()
    {
        _onNo?.Invoke();
        _onYes?.RemoveAllListeners();
        _onNo?.RemoveAllListeners();
        _container.SetActive(false);
    }

}
