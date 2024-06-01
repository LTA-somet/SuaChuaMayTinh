using System.Collections;
using System.Collections.Generic;
using Tu.Animation.ThaoLap;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace PCmodel.TreeView
{
    public static class RectTransformExtensions
    {
        public static void SetLeft(this RectTransform rt, float left)
        {
            rt.offsetMin = new Vector2(left, rt.offsetMin.y);
        }

        public static void SetRight(this RectTransform rt, float right)
        {
            rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
        }

        public static void SetTop(this RectTransform rt, float top)
        {
            rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
        }

        public static void SetBottom(this RectTransform rt, float bottom)
        {
            rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
        }
    }
    public class TreeViewItem : MonoBehaviour
    {
        [SerializeField] Toggle _toggle;
        [SerializeField] Button _selectItemBtn;
        [SerializeField] TMPro.TextMeshProUGUI _textMesh;
        public TMPro.TextMeshProUGUI TextMesh { get { return _textMesh; } }
        [SerializeField] RectTransform _contentRectTr;
        [SerializeField] RectTransform _contentSizeTr;
        [SerializeField] RectTransform _rectTr;
        [Header("HighLight")]
        [SerializeField] Color _normal = Color.white;
        [SerializeField] Color _highlight = Color.white;
        [Header("Expand")]
        [SerializeField] Button _collapesBtn;
        public Button CollapesBtn { get { return _collapesBtn; } }
        [SerializeField] Sprite _collapeSprite;
        [SerializeField] Sprite _expandSprite;
        TreeViewObject _treeViewObject;
        TreeView _treeView;
        [SerializeField] List<TreeViewItem> _childItems = new List<TreeViewItem>();
        public List<TreeViewItem> ChildItem { get { return _childItems; } }


        public float XSize => _contentSizeTr.sizeDelta.x;
        bool _isExpanded = true;
        private void Awake()
        {

            _selectItemBtn.onClick.AddListener(SelectItem);
            _toggle.onValueChanged.AddListener(OnToggle);
        }
        private void Update()
        {
            //_rectTr.sizeDelta = _contentSizeTr.sizeDelta;
        }
        public void AddChildrens(List<TreeViewItem> items)
        {
            _childItems = items;
            if (_childItems.Count < 1)
            {
                _collapesBtn.gameObject.SetActive(false);
            }
            else
            {
                _collapesBtn.onClick.AddListener(ToggleExpand);
            }
            //SetExpand(false);
        }
        public void SetSize(float ySize)
        {
            var sizeDelta = _contentSizeTr.sizeDelta;
            sizeDelta.y = ySize;
            _rectTr.sizeDelta = sizeDelta;
        }
        public void SetTab(float spacing)
        {
            _contentRectTr.SetLeft(spacing);
            _contentRectTr.SetRight(-spacing);
        }
        public void SetName(string newName)
        {
            _textMesh.text = newName;
        }
        public void SetObject(TreeViewObject newObject)
        {
            _treeViewObject = newObject;
        }
        public void SetRoot(TreeView treeView)
        {
            _treeView = treeView;
        }
        public void SetHighLight(bool newHighlight)
        {
            if (newHighlight)
            {
                _textMesh.fontStyle = TMPro.FontStyles.Underline;
                _textMesh.color = _highlight;
            }
            else
            {
                _textMesh.fontStyle = TMPro.FontStyles.Normal;
                _textMesh.color = _normal;
                _treeView.HighLightObjectId = _treeViewObject.Id;
            }
        }
        private void SelectItem()
        {
            this.DispatchEvent(EventKey.REQUEST_OPEN_SELECT_UI, _treeViewObject.Id); Debug.Log("id: " + _treeViewObject.Id);
            if (_treeView)
            {
                _treeView.HighLightTreeChild(this);
            }
            if (_treeViewObject.Id == string.Empty)
            {

                //UIManager.Instance.Muc4UI.GetComponent<Muc4UI>().ChangeIndex(-1);

                //_selectItemBtn.interactable = false;
            }
            else
            {
                //_selectItemBtn.interactable = true;
                //   UIManager.Instance.Muc4UI.GetComponent<Muc4UI>().ChangeIndex(int.Parse(_treeViewObject.Id));

            }
            // UIManager.Instance.Muc4UI.GetComponent<Muc4UI>().ChangeDataMuc4();

        }
        private void OnToggle(bool value)
        {
            if (value)
            {
                OnSelect();
            }
            else
            {
                OnDeselect();
            }
        }
        private void OnSelect()
        {
            _toggle.SetIsOnWithoutNotify(true);
            _treeViewObject.OnSelect();
            foreach (var item in _childItems)
            {
                item.OnSelect();
            }
        }
        private void OnDeselect()
        {
            _toggle.SetIsOnWithoutNotify(false);
            _treeViewObject.OnDeselect();
            foreach (var item in _childItems)
            {
                item.OnDeselect();
            }
        }
        private void ToggleExpand()
        {
            SetExpand(!_isExpanded);
        }
        private void SetExpand(bool newExpand)
        {
            Debug.Log(_childItems.Count);
            _isExpanded = newExpand;
            _collapesBtn.image.sprite = _isExpanded ? _collapeSprite : _expandSprite;
            foreach (var item in _childItems)
            {
                if (item == this) continue;
                if (!_isExpanded)
                {
                    item.SetExpand(newExpand);
                }
                item.gameObject.SetActive(_isExpanded);
                Debug.Log(item._textMesh.text);
            }
            Debug.Log(_childItems.Count);
        }
    }
}

