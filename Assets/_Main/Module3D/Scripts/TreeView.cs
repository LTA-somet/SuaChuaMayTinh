using PCmodel.TreeView.Model;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TDLN.CameraControllers;
using TMPro;
using Tu.Animation.ThaoLap;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace PCmodel.TreeView
{
    public class TreeView : MonoBehaviour
    {
        [SerializeField] RectTransform _treeContainer;
        [SerializeField] GameObject _treeViewObjectRoot;
        [SerializeField] Material _transMat;
        [SerializeField] float _yScale = 50;
        [SerializeField] float _spacing = 10;
        [SerializeField] TreeViewItem _baseTreeViewItemPrefab;
        [SerializeField]
        Slider _slider;
        [SerializeField]
        TMP_InputField _inputField;
        public string HighLightObjectId;
        public Slider Slider { get { return _slider; } }

        List<TreeViewItem> _itemsCached = new List<TreeViewItem>();
        private void Start()
        {
            //GenerateTreeView();
            _inputField.onValueChanged.AddListener((string s) =>
            {
                foreach (Transform item in _treeContainer)
                {
                    if (s == string.Empty)
                    {
                        item.gameObject.SetActive(true);
                        if (item.GetComponent<TreeViewItem>().ChildItem.Count > 0)
                        {

                            item.GetComponent<TreeViewItem>().CollapesBtn.gameObject.SetActive(true);
                        }
                    }
                    else
                    {
                        bool check = CapitalizeFirstLetter(item.GetComponent<TreeViewItem>().TextMesh.text).Contains(CapitalizeFirstLetter(s));
                        if (check)
                        {
                            item.gameObject.SetActive(true);
                            item.GetComponent<TreeViewItem>().CollapesBtn.gameObject.SetActive(false);
                        }
                        else
                        {
                            item.gameObject.SetActive(false);
                            if (item.GetComponent<TreeViewItem>().ChildItem.Count > 0)
                            {

                                item.GetComponent<TreeViewItem>().CollapesBtn.gameObject.SetActive(true);
                            }
                        }
                    }
                }
            });
        }
        static string CapitalizeFirstLetter(string value)
        {
            value = value.ToLower();
            char[] array = value.ToCharArray();
            if (array.Length >= 1)
            {
                if (char.IsLower(array[0]))
                {
                    array[0] = char.ToUpper(array[0]);
                }
            }

            for (int i = 1; i < array.Length; i++)
            {
                if (array[i - 1] == ' ')
                {
                    if (char.IsLower(array[i]))
                    {
                        array[i] = char.ToUpper(array[i]);
                    }
                }
            }
            return new string(array);
        }
        //private void OnEnable()
        //{
        //    _camera.EnableCamera();
        //}
        private void ChangeMaterial(TreeViewObject currentBranchObject, int level)
        {
            bool isThisLevelTransparent = currentBranchObject.IsDeselect;
            //currentBranchObject.SetData(this);
            if (_slider.value == 1 || !isThisLevelTransparent)
            {
                currentBranchObject.SetTransparent(false);
            }
            else
            {
                currentBranchObject.SetTransparent(true);
                currentBranchObject.SetOpacity(_slider.value);
            }
            Transform currentBranchTr = currentBranchObject.transform;
            foreach (Transform t in currentBranchTr)
            {
                if (t.TryGetComponent(out TreeViewObject itemBranch))
                {
                    int a = level + 1;
                    ChangeMaterial(itemBranch, a);
                }
            }
        }
        int maxLevel = 0;
        IEnumerator UpdateContainerScale()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            var newSize = _treeContainer.sizeDelta;
            newSize.y = _yScale * _treeContainer.childCount * 1.2f;
            float maxSize = 0;
            for (int i = 0; i < _treeContainer.childCount; i++)
            {
                if (_treeContainer.GetChild(i).TryGetComponent(out TreeViewItem item))
                {
                    var curSize = item.XSize;
                    if (curSize > maxSize)
                    {
                        maxSize = curSize;
                    }
                }
            }
            newSize.x = maxSize + (_spacing + 3) * maxLevel;
            _treeContainer.sizeDelta = newSize;
        }
        public void GenerateTreeView()
        {
            _inputField.text = "";
            TreeViewObject firstBranch = _treeViewObjectRoot.GetComponentInChildren<TreeViewObject>();
            foreach (Transform t in _treeContainer)
            {
                Destroy(t.gameObject);
            }
            _slider.onValueChanged.AddListener((float f) => ChangeMaterial(firstBranch, 0));
            if (firstBranch != null)
            {
                SetupBranch(firstBranch, 0);
            }
            StopAllCoroutines();
            StartCoroutine(UpdateContainerScale());

            //ModelPartTreeView modelPartTreeView = _treeViewObjectRoot.GetComponent<ModelPartTreeView>();
            //GameManager.Instance.OrbitCamera.GetComponent<CameraController>().SetObject(modelPartTreeView.Renderers[modelPartTreeView.Renderers.Length - 1].transform);
            //sGameManager.Instance.OrbitCamera.GetComponent<CameraController>().SetObject(
            //firstBranch.SetTransparent(false);
        }
        public TreeViewItem SetupBranch(TreeViewObject currentBranchObject, int level)
        {
            TreeViewItem newItem = null;
            newItem = Instantiate(_baseTreeViewItemPrefab, _treeContainer);
            currentBranchObject.AssignTransMat(_transMat);
            currentBranchObject.SetData(this);
            newItem.SetSize(_yScale);
            newItem.SetTab(_spacing * level);
            newItem.SetName(currentBranchObject.Name);
            newItem.SetObject(currentBranchObject);
            newItem.SetRoot(this);
            Transform currentBranchTr = currentBranchObject.transform;
            _itemsCached.Add(newItem);
            List<TreeViewItem> items = new List<TreeViewItem>();
            foreach (Transform t in currentBranchTr)
            {

                if (t.TryGetComponent(out TreeViewObject itemBranch))
                {
                    maxLevel = level + 1;
                    items.Add(SetupBranch(itemBranch, maxLevel));
                }
            }
            newItem.AddChildrens(items);
            return newItem;
        }
        public void SetNewTreeviewObject(GameObject newTreeviewObject)
        {
            _treeViewObjectRoot = newTreeviewObject;
            GenerateTreeView();
        }
        public void HighLightTreeChild(TreeViewItem item)
        {
            foreach (var child in _itemsCached)
            {
                if (child != item)
                {
                    child.SetHighLight(false);
                }
                else
                {
                    child.SetHighLight(true);
                }
            }
        }

    }
}
