using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Muc5UI : MonoBehaviour, IPointerDownHandler
{
    [Header("Slides")]
    [SerializeField] SlideImage _slideImage;
    [SerializeField] SlideVideo _slideVideo;
    [SerializeField] SlideAnimation _slideAnimation;
    [Header("Content")]
    [SerializeField] TMPro.TextMeshProUGUI _title;
    [SerializeField] TMPro.TMP_Dropdown _pageList;
    [Header("Share Button")]
    [SerializeField] Button _nextBtn;
    [SerializeField] Button _backBtn;
    [SerializeField] Button _closeBtn;
    [SerializeField] Button _adminBtn;
    [Header("Edit")]
    [SerializeField] Button _addBtn;
    [SerializeField] Button _editBtn;
    [SerializeField] Button _delBtn;
    [SerializeField] GameObject _editBtnContainer;


    ContentData _data;
    ContentData Data => _data;
    int _currentIndex = 0;
    public bool HasAnySlide => _data.ChildrenContent != null && _data.ChildrenContent.Count > 0;
    private void Awake()
    {
        _nextBtn.onClick.AddListener(NextSlide);
        _backBtn.onClick.AddListener(PrevSlide);
        _closeBtn.onClick.AddListener(() => gameObject.SetActive(false));
        _adminBtn.onClick.AddListener(() => _editBtnContainer.SetActive(true));

        _addBtn.onClick.AddListener(OpenAddSlide);
        _editBtn.onClick.AddListener(OpenEditSlide);
        _delBtn.onClick.AddListener(() => NotiPanel.Instance.ShowNotify("Bạn có muốn xoá?", DeleteSlide));

        _pageList.onValueChanged.AddListener(OpenSlide);
    }
    public void InitData(ContentData data)
    {
        _data = data;
        UpdateSlideOrder();
        _title.text = _data.Title;
        OpenSlide(0);
    }
    public void OpenSlide(int index)
    {
        CloseAllSlides();
        if (HasAnySlide)
        {
            if (index < 0)
            {
                index = _data.ChildrenContent.Count - 1;
            }
            _currentIndex = index % _data.ChildrenContent.Count;
            var data = _data.ChildrenContent[_currentIndex];
            switch (data.Type)
            {
                case ContentType.Image:
                    {
                        _slideImage.gameObject.SetActive(true);
                        _slideImage.OpenSlide(data);
                        break;
                    }
                case ContentType.Video:
                    {
                        _slideVideo.gameObject.SetActive(true);
                        _slideVideo.OpenSlide(data);
                        break;
                    }
                case ContentType.Animation:
                    {
                        _slideAnimation.gameObject.SetActive(true);
                        //_slideAnimation.OpenSlide(data);
                        break;
                    }
            }
        }
        else
        {
            //Just to be safe
            _currentIndex = 0;
        }
        _pageList.SetValueWithoutNotify(_currentIndex);

    }

    private void CloseAllSlides()
    {
        _slideImage.gameObject.SetActive(false);
        _slideVideo.gameObject.SetActive(false);
        _slideAnimation.gameObject.SetActive(false);
    }
    private void NextSlide()
    {
        OpenSlide(_currentIndex + 1);
    }
    private void PrevSlide()
    {
        OpenSlide(_currentIndex - 1);
    }

    private void DeleteSlide()
    {
        if (HasAnySlide)
        {
            var data = _data.ChildrenContent[_currentIndex];
            StreamingAssetHelper.DeleteFile(data.MediaPath);
            _data.ChildrenContent.RemoveAt(_currentIndex);
            UpdateSlideOrder();
            OpenSlide(_currentIndex - 1);
        }
        else
        {
            Debug.LogError("No slide to delete!!");
        }
    }
    private void OpenEditSlide()
    {
        if (HasAnySlide)
        {
            _editBtnContainer.SetActive(false);
            EditTheory.Instance.Muc5.OpenEdit(_data.ChildrenContent[_currentIndex], SaveEditData);
        }
        else
        {
            Debug.LogError("No slide to edit!!");
        }
    }
    private void OpenAddSlide()
    {
        _editBtnContainer.SetActive(false);
        EditTheory.Instance.Muc5.OpenEdit(_data.ChildrenContent.Count + 1, AddSlide);
    }
    private void AddSlide(SlideData data)
    {
        data.Order--;
        _data.ChildrenContent.Add(data);
        UpdateSlideOrder();
        OpenSlide(data.Order);
    }
    private void SaveEditData(SlideData data)
    {
        var oldData = _data.ChildrenContent[_currentIndex];
        var oldMedia = oldData.MediaPath;
        if(oldMedia != data.MediaPath)
        {
            StreamingAssetHelper.DeleteFile(oldMedia);
        }
        _data.ChildrenContent[_currentIndex] = data;

        if (data.Order > oldData.Order)
            data.Order = data.Order + 1;
        else if (data.Order < oldData.Order)
            data.Order = data.Order - 1;

        UpdateSlideOrder();
        OpenSlide(data.Order);
    }

    private void UpdateSlideOrder()
    {
        _pageList.ClearOptions();
        if (_data.ChildrenContent == null)
        {
            //To be safe
            _data.ChildrenContent = new List<SlideData>();
        }
        List<string> options = new List<string>();
        _data.ChildrenContent = _data.ChildrenContent.OrderBy(x => x.Order).ToList();
        for (int i = 0; i < _data.ChildrenContent.Count; i++)
        {
            options.Add((i + 1).ToString());
            _data.ChildrenContent[i].Order = i;
        }
        _pageList.AddOptions(options);
        if (_pageList.options.Count < 1)
        {
            _pageList.gameObject.SetActive(false);
            _nextBtn.gameObject.SetActive(false);
            _backBtn.gameObject.SetActive(false);
        }
        else
        {
            _pageList.gameObject.SetActive(true);
            _nextBtn.gameObject.SetActive(true);
            _backBtn.gameObject.SetActive(true);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_editBtnContainer.activeSelf)
        {
            _editBtnContainer.SetActive(false);
        }
    }
}
