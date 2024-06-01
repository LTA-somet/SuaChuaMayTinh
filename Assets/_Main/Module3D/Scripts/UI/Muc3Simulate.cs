using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Muc3Simulate : MonoBehaviour
{

    [SerializeField] Muc3childSimulate[] _childrenContainerSimulate;
    [SerializeField] TMPro.TMP_Dropdown _muc3DataContainer;
    [SerializeField] TMPro.TextMeshProUGUI _title;
    [SerializeField] Button _nextBtn, _prevBtn;
    private List<Muc3Data> _datas;
    public List<Muc3Data> Datas { get { return _datas; } }
    private List<Muc3ChildData> _datasChild;
    public List<Muc3ChildData> DatasChild { get { return _datasChild; } }
    private Muc3Data _currentSelectedData;
    private Muc3Data CurrentData => _currentSelectedData;

    ContentData _content;
    int _currentSegment = 0;
    [SerializeField] int _totalSegment = 0;
    int _currentSlideIndex = 0;
    private void Awake()
    {
        _muc3DataContainer.onValueChanged.AddListener(OpenSlide);
        _nextBtn.onClick.AddListener(NextSegment);
        _prevBtn.onClick.AddListener(PrevSegment);
        this.AddEvent(EventKey.REQUEST_REFRESH_SEGMENT_LEVEL_3, RefreshSegment);
        // this.AddEvent(EventKey.REQUEST_OPEN_SELECT_UI, ChangeIndex);

    }
    private void OnDestroy()
    {
        this.DropEvent(EventKey.REQUEST_REFRESH_SEGMENT_LEVEL_3, RefreshSegment);
    }
    public void InitData(List<Muc3Data> newData)
    {
        List<string> options = new List<string>();
        _muc3DataContainer.ClearOptions();
        _datas = newData.OrderBy(x => x.Order).ToList();
        for (int i = 0; i < _datas.Count; i++)
        {
            options.Add(_datas[i].Title);
            _datas[i].Order = i;
        }
        _muc3DataContainer.AddOptions(options);
        OpenSlide(0);
    }

    public void RefreshSlide()
    {
        OpenSlide(_currentSlideIndex);
    }
    public void RefreshSegment(object o)
    {
        //OpenSegment(_currentSegment);
    }
    public void OpenSlide(int index)
    {
        if (_datas == null) return;
        if (index > -1 && index < _datas.Count)
        {
            _currentSlideIndex = index;
            _currentSelectedData = _datas[_currentSlideIndex];
            _totalSegment = _currentSelectedData.ChildrenContent.Count / _childrenContainerSimulate.Length + 1;
            if (_totalSegment > 1)
            {
                _nextBtn.gameObject.SetActive(true);
                _prevBtn.gameObject.SetActive(true);
            }
            else
            {
                _nextBtn.gameObject.SetActive(false);
                _prevBtn.gameObject.SetActive(false);
            }
            OpenSegment(0);
            _title.text = _currentSelectedData.Title;
        }
    }
    public void RemoveChild(Muc3ChildContent child)
    {
        if (_datas != null)
        {
            var childData = child.Data;
            _currentSelectedData.ChildrenContent.Remove(childData);
            childData.Destroy();
            _currentSelectedData.ChildrenContent.OrderBy(x => x.Order);
            for (int i = 0; i < _currentSelectedData.ChildrenContent.Count; i++)
            {
                _currentSelectedData.ChildrenContent[i].Order = i;
            }
            int oldSegment = _currentSegment;
            OpenSlide(_currentSlideIndex);
            OpenSegment(Mathf.Clamp(oldSegment, 0, _totalSegment));
        }
    }
    private void NextSegment()
    {
        OpenSegment(_currentSegment + 1);
    }
    private void PrevSegment()
    {
        OpenSegment(_currentSegment - 1);
    }
    private void OpenSegment(int segment)
    {
        _currentSegment = segment;
        if (_currentSegment < 0)
        {
            _currentSegment = _totalSegment;
        }
        else if (_currentSegment >= _totalSegment)
        {
            _currentSegment = 0;
        }
        int offSet = _currentSegment * 6;
        int ceil = _childrenContainerSimulate.Length;
        if (_currentSegment >= _totalSegment - 1)
        {
            ceil = _currentSelectedData.ChildrenContent.Count % _childrenContainerSimulate.Length;
        }
        for (int i = 0; i < _childrenContainerSimulate.Length; i++)
        {
            var child = _childrenContainerSimulate[i];
            if (i < ceil)
            {
                child.gameObject.SetActive(true);
                child.InitData(_currentSelectedData.ChildrenContent[i + offSet], _currentSlideIndex, _currentSlideIndex == _datas.Count - 1, _currentSelectedData.Title);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

}
