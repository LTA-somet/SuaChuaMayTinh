using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class EditSymbolChild : MonoBehaviour
{
    [SerializeField] RawImage _symbolMedia;
    [SerializeField] TMPro.TMP_InputField _symbolContext;
    [SerializeField] Button _mediaLoaderBtn;
    [SerializeField] Button _deleteBtn;

    System.Action<ContentData> _onDelete;
    //[SerializeField] TMPro.TextMeshProUGUI _order;
    string currentMediaPath;
    ContentData _symbolData;
    public ContentData Data => _symbolData;
    bool _mediaDirty = false;
    private void Awake()
    {
        _mediaLoaderBtn.onClick.AddListener(LoadImage);
        _deleteBtn.onClick.AddListener(Delete);
    }
    private void OnDisable()
    {
        _mediaDirty = false;
    }
    private void Delete()
    {
        if(Data != null)
        {
            _onDelete?.Invoke(Data);
        }    
        Destroy(gameObject);
    }
    private void LoadImage()
    {
        if(Loader.OpenMediaFileFromDevice(false,out var newpath))
        {
            currentMediaPath = newpath;
            _symbolMedia.texture = Loader.LoadTexture(currentMediaPath);
            _mediaDirty = true;
        }
    }
    public void InitData(ContentData data, System.Action<ContentData> onDelete)
    {
        _symbolData = data;
        currentMediaPath = StreamingAssetHelper.GetPath(data.MediaPath);
        _symbolMedia.texture = StreamingAssetHelper.GetTextureFromStreamingAsset(data.MediaPath);
        _symbolContext.text = data.Content;
        _onDelete += onDelete;
    }
    public void CreateSymbol(string mediaPath)
    {
        _mediaDirty = true;
        currentMediaPath = mediaPath;
        _symbolMedia.texture = Loader.LoadTexture(currentMediaPath);
        _symbolContext.text = "Tên kí hiệu";
        _symbolData = new ContentData();
    }

    public void Save()
    {
        if (_mediaDirty && File.Exists(currentMediaPath))
        {
            var newMediaPath = StreamingAssetHelper.ImportSymbolToStreamingAssets(currentMediaPath, Path.GetFileName(currentMediaPath));
            StreamingAssetHelper.DeleteFile(_symbolData.MediaPath);
            _symbolData.MediaPath = newMediaPath;
        }
        //_symbolData.Order = int.Parse(_order.text);
        _symbolData.Content = _symbolContext.text;
        return;
    }
}
