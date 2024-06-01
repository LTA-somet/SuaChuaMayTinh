using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Muc3UI : MonoBehaviour
{

    [SerializeField] Muc3ChildContent[] _childrenContainer;
    [SerializeField] TMPro.TMP_Dropdown _muc3DataContainer;
    [SerializeField] TMPro.TextMeshProUGUI _title;
    [SerializeField] Button _nextBtn, _prevBtn;
    [SerializeField] Button _addBtn;
    private List<Muc3Data> _datas;
    public List<Muc3Data> Datas { get { return _datas; } }
    private List<Muc3ChildData> _datasChild;
    public List<Muc3ChildData> DatasChild { get { return _datasChild; } }
    private Muc3Data _currentSelectedData;
    private Muc3Data CurrentData => _currentSelectedData;

    int _currentSegment = 0;
    [SerializeField] int _totalSegment = 0;
    int _currentSlideIndex = 0;
    private void Awake()
    {
        _muc3DataContainer.onValueChanged.AddListener(OpenSlide);
        _nextBtn.onClick.AddListener(NextSegment);
        _prevBtn.onClick.AddListener(PrevSegment);
        _addBtn.onClick.AddListener(OpenAddData);
        this.AddEvent(EventKey.REQUEST_REFRESH_SEGMENT_LEVEL_3, RefreshSegment);
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
    public void RefreshSegment()
    {
        OpenSegment(_currentSegment);
    }
    public void OpenSlide(int index)
    {
        if (_datas == null)
        {
            return;
        }
        if (index > -1 && index < _datas.Count)
        {
            _currentSlideIndex = index;
            _currentSelectedData = _datas[_currentSlideIndex];
            _totalSegment = _currentSelectedData.ChildrenContent.Count / _childrenContainer.Length + 1;
            //if (_currentSelectedData.ChildrenContent.Count % _childrenContainer.Length == 0)
            //{
            //    _totalSegment += 1;
            //}
            if (_currentSelectedData.Title == "Mô hình 3D" || _currentSelectedData.Title == "Thực hành lắp ráp")
            {
                this.DispatchEvent(EventKey.REQUEST_OPEN_MODEL_Level_4, _currentSelectedData);
            }
            else
            {

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
            _muc3DataContainer.value = index;
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
        _addBtn.gameObject.SetActive(false);
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
        int ceil = _childrenContainer.Length;
        if (_currentSegment >= _totalSegment - 1)
        {
            ceil = _currentSelectedData.ChildrenContent.Count % _childrenContainer.Length;
            _addBtn.gameObject.SetActive(true);
        }
        for (int i = 0; i < _childrenContainer.Length; i++)
        {
            var child = _childrenContainer[i];
            if (i < ceil)
            {
                child.gameObject.SetActive(true);
                child.InitData(_currentSelectedData.ChildrenContent[i + offSet], _currentSlideIndex, _currentSlideIndex == _datas.Count - 1);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }
    private void OpenAddData()
    {
        EditTheory.Instance.Muc3.OpenAddNewData(OnSave);
        void OnSave(Muc3ChildData data)
        {
            data.DemoMediaPath = StreamingAssetHelper.ImportImageToStreamingAssets(data.DemoMediaPath, Path.GetFileName(data.DemoMediaPath));
            _currentSelectedData.ChildrenContent.Add(data);
            OpenSlide(_currentSlideIndex);
        }
    }
}
