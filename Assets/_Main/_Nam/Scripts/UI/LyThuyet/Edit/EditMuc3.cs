using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class EditMuc3 : MonoBehaviour
{
    public RawImage Media;
    public TMPro.TMP_InputField TitleField;
    public Button SaveBtn;
    public Button CancelBtn;
    public Button AddBtn;
    public RectTransform ChildrenContainer;
    public ChildListItem ChildItem;

    public Button LoadMediaBtn;
    public Button RemoveMediaBtn;

    private System.Action<Muc3ChildData> onSave;
    Texture _baseTex;
    string _mediaPath;
    public void Awake()
    {
        AddBtn.onClick.AddListener(Add);
        SaveBtn.onClick.AddListener(Save);
        CancelBtn.onClick.AddListener(Close);
        LoadMediaBtn.onClick.AddListener(LoadMedia);
        RemoveMediaBtn.onClick.AddListener(DeleteMedia);
        _baseTex = Media.mainTexture;
    }
    public void OpenAddNewData(System.Action<Muc3ChildData> onSave)
    {
        gameObject.SetActive(true);
        this.onSave = onSave;
    }
    public void OpenEditData(Muc3ChildData data, System.Action<Muc3ChildData> onSave)
    {

        var media = StreamingAssetHelper.GetTextureFromStreamingAsset(data.DemoMediaPath);
        if (media)
        {
            Media.texture = media;
            _mediaPath = StreamingAssetHelper.GetPath(data.DemoMediaPath);
        }
        TitleField.text = data.Title;

        for (int i = 0; i < data.ChildrenContent.Count; i++)
        {
            var kidData = data.ChildrenContent[i];
            var kidView = Instantiate(ChildItem, ChildrenContainer);
            kidView.GenericData = kidData.ChildrenData;
            kidView.SetContent(kidData.Title);
            AddBtn.transform.SetAsLastSibling();
            kidView.UpdateIndex();
        }

        gameObject.SetActive(true);
        this.onSave = onSave;
    }
    private void Add()
    {
        var child = Instantiate(ChildItem, ChildrenContainer);
        AddBtn.transform.SetAsLastSibling();
        child.UpdateIndex();
    }
    private void LoadMedia()
    {
        if (Loader.OpenMediaFileFromDevice(false, out var mediaPath))
        {
            _mediaPath = mediaPath;
            Media.texture = Loader.LoadTexture(mediaPath);
        }
    }
    private void DeleteMedia()
    {
        _mediaPath = string.Empty;
        Media.texture = _baseTex;
    }
    private void Save()
    {
        Muc3ChildData data = new Muc3ChildData();
        data.Title = TitleField.text;
        data.ChildrenContent = new List<Muc4Data>();
        data.DemoMediaPath = _mediaPath;
        for (int i = 0; i < ChildrenContainer.childCount; i++)
        {
            if (ChildrenContainer.GetChild(i).TryGetComponent(out ChildListItem item))
            {
                var newItem = new Muc4Data();
                newItem.Title = item.Content;
                newItem.Order = i;
                if (item.GenericData != null && item.GenericData is List<ContentData>)
                {
                    newItem.ChildrenData = (item.GenericData) as List<ContentData>;
                }
                data.ChildrenContent.Add(newItem);
            }
        }
        onSave?.Invoke(data);
        Close();
    }
    private void Close()
    {
        onSave = null;
        Media.texture = _baseTex;
        _mediaPath = TitleField.text = string.Empty;
        for (int i = 0; i < ChildrenContainer.childCount; i++)
        {
            var childGO = ChildrenContainer.GetChild(i).gameObject;
            if(childGO != AddBtn.gameObject)
            {
                Destroy(childGO.gameObject);
            }
        }
        gameObject.SetActive(false);
    }
}
