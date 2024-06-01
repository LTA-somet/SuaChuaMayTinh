using Nam.TreeView;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Muc4UI : MonoBehaviour, IPointerUpHandler
{
    [SerializeField] TMPro.TextMeshProUGUI _title;
    [SerializeField] TMPro.TextMeshProUGUI _childTitle;
    [SerializeField] private ScrollRect _contentScroll;
    [SerializeField] private Muc4ContentChild _contentChildPrefab;

    [SerializeField] GameObject _bottom;
    [SerializeField] Button _nextBtn, _prevBtn;
    [SerializeField] TMPro.TMP_Dropdown _pageDropdown;

    [SerializeField] Button _addDataButton;
    [SerializeField] Button _closeButton;

    [Header("Big Edit Frame")]
    [SerializeField] Button _openFrameBtn;
    [SerializeField] Button _addPageBtn;
    [SerializeField] Button _delPageBtn;
    [SerializeField] GameObject _pageEditFrame;

    Muc3ChildData _data;
    public Muc3ChildData Data => _data;
    public Muc4Data CData => (currentIndex < _data.ChildrenContent.Count && currentIndex > -1) ? _data.ChildrenContent[currentIndex] : null;
    int currentIndex = 0;
    private void Awake()
    {
        _addDataButton.onClick.AddListener(OpenAddNewData);
        _closeButton.onClick.AddListener(() => { gameObject.SetActive(false); MainUI.Instance.TreeView.GetComponent<TreeView>().Init(); MainUI.Instance.TreeView.SetActive(false); });
        _pageDropdown.onValueChanged.AddListener(OpenSlide);
        _nextBtn.onClick.AddListener(NextPage);
        _prevBtn.onClick.AddListener(PrevPage);
        _childTitle.text = string.Empty;
        _addPageBtn.onClick.AddListener(AddPage);
        _delPageBtn.onClick.AddListener(DeletePage);
        _openFrameBtn.onClick.AddListener(() => _pageEditFrame.SetActive(_pageEditFrame.activeSelf));
        _pageEditFrame.SetActive(false);

        this.AddEvent(EventKey.REQUEST_OPEN_THEORY_Level_4_Index, (object o) => OpenLevel4Index((int)o));

    }
    public void DeleteChild(int index)
    {
        CData.ChildrenData.RemoveAt(index);
        for (int i = 0; i < CData.ChildrenData.Count; i++)
        {
            var childData = CData.ChildrenData[i];
            childData.Order = i;
        }
        UpdateContentView();
    }
    private void OpenAddNewData()
    {
        EditTheory.Instance.Muc4.OpenEdit(CData != null ? CData.ChildrenData.Count + 1 : 1, OnAddMuc4Edit);
    }
    private void OnAddMuc4Edit(ContentData data)
    {
        if (data.Order > 0 && data.Order < CData.ChildrenData.Count)
        {
            CData.ChildrenData.Insert(data.Order, data);
        }
        else
        {
            CData.ChildrenData.Add(data);
        }
        for (int i = 0; i < CData.ChildrenData.Count; i++)
        {
            var childData = CData.ChildrenData[i];
            childData.Order = i;
        }
        UpdateContentView();
    }


    public void Init(Muc3ChildData newData)
    {
        _data = newData;
        _title.text = newData.Title;
        if (newData.ChildrenContent == null)
        {
            newData.ChildrenContent = new List<Muc4Data>();
        }
        if (newData.ChildrenContent.Count < 1)
        {
            _bottom.gameObject.SetActive(false);
            _addDataButton.gameObject.SetActive(false);
        }
        else
        {
            _pageDropdown.ClearOptions();
            var options = new List<string>();
            for (int i = 0; i < newData.ChildrenContent.Count; i++)
            {
                options.Add(NumbersToRoman.IntToRoman(i + 1));
            }
            _pageDropdown.AddOptions(options);
            _addDataButton.gameObject.SetActive(true);
            _bottom.gameObject.SetActive(true);
        }
        OpenSlide(0);
    }
    private void NextPage()
    {
        OpenSlide(currentIndex + 1);
    }
    private void PrevPage()
    {
        OpenSlide(currentIndex - 1);
    }
    public void OpenSlide(int index)
    {
        if (index < 0)
        {
            index = _data.ChildrenContent.Count - 1;
        }
        else if (index >= _data.ChildrenContent.Count)
        {
            index = 0;
        }
        currentIndex = index;
        if (_data.ChildrenContent == null)
        {
            _data.ChildrenContent = new List<Muc4Data>();
            currentIndex = 0;
        }
        if (_data.ChildrenContent.Count > 0)
        {
            _pageDropdown.SetValueWithoutNotify(index);
            _childTitle.text = CData.GetTitleWithRomanIndex();
        }
        else
        {
            _childTitle.text = string.Empty;
        }
        UpdateContentView();
    }
    private void OpenLevel4Index(int index)
    {
        this.gameObject.SetActive(true);
        this.OpenSlide(index);
    }
    private void UpdateContentView()
    {
        var contentContainer = _contentScroll.content;
        foreach (Transform t in contentContainer)
        {
            if (t.gameObject != _addDataButton.gameObject)
            {
                Destroy(t.gameObject);
            }
        }
        _title.text = _data.Title;
        if (_data.ChildrenContent != null && _data.ChildrenContent.Count > 0)
        {
            CData.ChildrenData = CData.ChildrenData.OrderBy(x => x.Order).ToList();
            for (int i = 0; i < CData.ChildrenData.Count; i++)
            {
                var childData = CData.ChildrenData[i];
                childData.Order = i;
                var contentChild = Instantiate(_contentChildPrefab, contentContainer);
                contentChild.GetComponent<Muc4ContentChild>().InitData(this, childData);
                //Debug.Log($"{i} | {contentChild.Data.MediaPath}"); 
            }
        }
        _addDataButton.transform.SetAsLastSibling();

        //var contentContainer = _contentScroll.content;
        ////foreach (Transform t in contentContainer)
        ////{
        ////    if (t.gameObject != _addDataButton.gameObject)
        ////    {
        ////        //Destroy(t.gameObject);
        ////        t.gameObject.SetActive(false);
        ////    }
        ////}
        //_title.text = _data.Title;
        //if (_data.ChildrenContent != null && _data.ChildrenContent.Count > 0)
        //{
        //    CData.ChildrenData = CData.ChildrenData.OrderBy(x => x.Order).ToList();
        //    if (contentContainer.childCount - 2 < CData.ChildrenData.Count)
        //    {
        //        for (int i = 0; i < contentContainer.childCount - 1; i++)
        //        {
        //            var childData = CData.ChildrenData[i];
        //            contentContainer.GetChild(i).gameObject.SetActive(true);
        //            contentContainer.GetChild(i).GetComponent<Muc4ContentChild>().InitData(this, childData);
        //        }
        //        for (int i = contentContainer.childCount - 1; i < CData.ChildrenData.Count; i++)
        //        {
        //            var childData = CData.ChildrenData[i];
        //            childData.Order = i;
        //            var contentChild = Instantiate(_contentChildPrefab, contentContainer);
        //            contentChild.InitData(this, childData);
        //            Debug.Log($"{i} | {contentChild.Data.MediaPath}");
        //        }
        //        _addDataButton.transform.SetAsLastSibling();
        //    }
        //    else
        //    {
        //        for (int i = 0; i < CData.ChildrenData.Count; i++)
        //        {
        //            var childData = CData.ChildrenData[i];
        //            //contentContainer.GetChild(i).gameObject.SetActive(true);
        //            contentContainer.GetChild(i).GetComponent<Muc4ContentChild>().InitData(this, childData);
        //        }
        //        for (int i = CData.ChildrenData.Count; i < contentContainer.childCount - 1; i++)
        //        {
        //            if (contentContainer.GetChild(i).gameObject != _addDataButton.gameObject)
        //            {
        //                //Destroy(t.gameObject);
        //                contentContainer.GetChild(i).gameObject.SetActive(false);
        //            }
        //        }
        //    }
        //}

    }
    public void AddPage()
    {
        _pageEditFrame.SetActive(false);
        NotiPanel.Instance.ShowNotify("Chưa hoàn thiện tính năng này", null);
    }
    public void DeletePage()
    {
        _pageEditFrame.SetActive(false);
        NotiPanel.Instance.ShowNotify("Bạn có muốn xóa trang hiện tại", Delete);
        void Delete()
        {
            _data.ChildrenContent.Remove(CData);
            for (int i = 0; i < _data.ChildrenContent.Count; i++)
            {
                var childContent = _data.ChildrenContent[i];
                childContent.Order = i;
            }
            OpenSlide(0);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _pageEditFrame.SetActive(false);
    }
    private void OnDestroy()
    {
        this.DropEvent(EventKey.REQUEST_OPEN_THEORY_Level_4_Index, (object o) => OpenLevel4Index((int)o));
    }
}
