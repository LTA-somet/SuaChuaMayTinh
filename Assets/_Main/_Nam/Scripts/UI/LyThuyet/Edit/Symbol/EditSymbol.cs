using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditSymbol : MonoBehaviour
{
    [SerializeField] RectTransform _symbolContainer;
    [SerializeField] EditSymbolChild _symbolChildPrefab;
    [SerializeField] Button _addBtn;
    [SerializeField] Button _cancelBtn;
    [SerializeField] Button _saveBtn;
    List<EditSymbolChild> _children = new List<EditSymbolChild>();
    List<EditSymbolChild> _extraChildren = new List<EditSymbolChild>();
    List<ContentData> _pendingDeleteSymbol = new List<ContentData>();

    Muc4Data _data;

    System.Action _onSaveEdit;
    private void Awake()
    {
        _addBtn.onClick.AddListener(AddNewData);
        _cancelBtn.onClick.AddListener(() => gameObject.SetActive(false));
        _saveBtn.onClick.AddListener(Save);
    }
    private void AddNewData()
    {
        if (Loader.OpenMediaFileFromDevice(false, out string path))
        {
            var symbol = Instantiate(_symbolChildPrefab, _symbolContainer);
            symbol.CreateSymbol(path);
            _extraChildren.Add(symbol);
            _addBtn.transform.SetAsLastSibling();
        }
    }
    private void DeleteSymbol(ContentData symbol)
    {
        _pendingDeleteSymbol.Add(symbol);
    }
    public void OpenEdit(Muc4Data symbolDatas, System.Action onSave)
    {
        gameObject.SetActive(true);
        _pendingDeleteSymbol.Clear();
        for (int i = 0; i < _children.Count; i++)
        {
            Destroy(_children[i].gameObject);
        }
        for (int i = 0; i < _extraChildren.Count; i++)
        {
            Destroy(_extraChildren[i].gameObject);
        }
        _children.Clear();
        if(symbolDatas.ChildrenData != null)
        {
            for (int i = 0; i < symbolDatas.ChildrenData.Count; i++)
            {
                var symbol = Instantiate(_symbolChildPrefab, _symbolContainer);
                symbol.InitData(symbolDatas.ChildrenData[i], DeleteSymbol);
                _children.Add(symbol);
            }
        }    

        _addBtn.transform.SetAsLastSibling();
        _data = symbolDatas;

        _onSaveEdit += onSave;
    }
    public void Save()
    {
        for (int i = 0; i < _children.Count; i++)
        {
            var child = _children[i];
            if(child)
            {
                child.Save();
                Destroy(child.gameObject);
            }    
        }
        for (int i = 0; i < _extraChildren.Count; i++)
        {
            var child = _extraChildren[i];
            if(child)
            {
                child.Save();
                _data.ChildrenData.Add(child.Data);
                Destroy(child.gameObject);
            }    
        }
        for (int i = 0; i < _pendingDeleteSymbol.Count; i++)
        {
            _data.ChildrenData.Remove(_pendingDeleteSymbol[i]);
        }

        _children.Clear();
        _extraChildren.Clear();
        _pendingDeleteSymbol.Clear();
        _onSaveEdit.Invoke();
        _onSaveEdit = null;
        gameObject.SetActive(false);
    }
}
