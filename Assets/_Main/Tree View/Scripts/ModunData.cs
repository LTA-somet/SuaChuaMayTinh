using Nam.TreeView;
using System.Collections.Generic;
using System.Data;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ModunData : MonoBehaviour
{
    private List<Muc3Data> _datas;
    [SerializeField] private GameObject itemLesson;
    private Transform _listLesson;
    [SerializeField] private Button button;
    private int _index = 0;
    private TreeView _TreeView;
    private bool _is3D;
    ToggleButton _toggleButton;
    private void Start()
    {
        button.onClick.AddListener(ModunClick);

    }
    public void setData(int index, List<Muc3Data> datas, Transform listLesson, TreeView treeView, bool is3D)
    {
        _datas = datas;
        _listLesson = listLesson;
        _index = index;
        _TreeView = treeView;
        _is3D = is3D;
    }
    public List<GameObject> menuObjects = new List<GameObject>();
    public void ModunClick()
    {
        MainUI.Instance.Theory.GetComponent<TheoryUI>().Muc3UI.OpenSlide(_index);
        Debug.Log(gameObject);
        _TreeView.MenuSearch.ResetLis();
        //foreach (Transform child in _listLesson)
        //{
        //    child.gameObject.SetActive(false);
        //}
        if (_TreeView.ListActiveLesson.Count > 0)
        {
            for (int i = 0; i < _TreeView.ListActiveLesson.Count; i++)

            {
                _TreeView.ListActiveLesson[i].SetActive(false);
                _TreeView.ListActiveLesson[i].transform.SetParent(null);
                _TreeView.ListUnactiveLesson.Add(_TreeView.ListActiveLesson[i]);
                _TreeView.ListActiveLesson[i].GetComponent<DropChilList>().MenuList.Clear();
            }
            if (_TreeView.LisActivePart.Count > 0)
            {
                for (int i = 0; i < _TreeView.LisActivePart.Count; i++)
                {
                    _TreeView.LisActivePart[i].SetActive(false);
                    _TreeView.ListUnactivePart.Add(_TreeView.LisActivePart[i]);
                    _TreeView.LisActivePart[i].transform.SetParent(null);
                }
                _TreeView.LisActivePart.Clear();
            }
            _TreeView.ListActiveLesson.Clear();
        }

        GameObject menuChildObject = null;
        for (int j = 0; j < _datas[_index].ChildrenContent.Count; j++)
        {
            if (_TreeView.ListUnactiveLesson.Count <= 0)
            {
                menuChildObject = Instantiate(itemLesson, _listLesson);
            }
            else
            {
                menuChildObject = _TreeView.ListUnactiveLesson[0];
                _TreeView.ListUnactiveLesson.Remove(menuChildObject);
                menuChildObject.transform.SetParent(_listLesson);
            }
            _TreeView.ListActiveLesson.Add(menuChildObject);
            TMPro.TextMeshProUGUI menuChildText = menuChildObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
            menuChildText.text = "" + _datas[_index].ChildrenContent[j].Title.ToString();
            menuChildObject.SetActive(true);
            menuChildObject.GetComponent<DropChilList>().setData(_datas, _listLesson, j, _index, _TreeView, _is3D);

        }
        onToggle();
    }

    void onToggle()
    {

        if (_TreeView.CurrSelect != null)
        {
            _TreeView.CurrSelect.SetOn(false);
        }
        _TreeView.CurrSelect = transform.GetComponent<ToggleButton>();
    }

}

