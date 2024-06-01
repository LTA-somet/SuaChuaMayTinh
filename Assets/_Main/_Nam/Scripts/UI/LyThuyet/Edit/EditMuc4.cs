using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class EditMuc4 : MonoBehaviour
{
    [SerializeField] Button _saveBtn;
    [SerializeField] Button _cancelBtn;
    [SerializeField] Button _loadMediaBtn;
    [SerializeField] Button _clearMediaBtn;
    [SerializeField] RawImage _loadedTexture;
    [SerializeField] TMPro.TMP_InputField _order;
    [SerializeField] TMPro.TMP_InputField _title;
    [SerializeField] TMPro.TMP_InputField _content;
    string _mediaPath = string.Empty;
    System.Action<ContentData> _onEditCompelete = null;

    Texture _baseTex = null;
    private void Awake()
    {
        _baseTex = _loadedTexture.mainTexture;
        _saveBtn.onClick.AddListener(Save);
        _cancelBtn.onClick.AddListener(Close);
        _loadMediaBtn.onClick.AddListener(LoadMedia);
        _clearMediaBtn.onClick.AddListener(ClearMedia);
    }
    private void Save()
    {
        string path = string.Empty;
        if(File.Exists(_mediaPath))
        {
            path = StreamingAssetHelper.ImportImageToStreamingAssets(_mediaPath, Path.GetFileName(_mediaPath));
        }
        if(_onEditCompelete != null)
        {
            _onEditCompelete.Invoke(new ContentData()
            {
                Title = _title.text,
                Content = _content.text,
                Order = int.TryParse((_order).text, out int number) ? number - 1 : 10000,
                MediaPath = path
            });
            _onEditCompelete = null;
        }
        Close();
    }
    private void Close()
    {
        _onEditCompelete = null;
        _mediaPath = string.Empty;
        _order.text = string.Empty;
        _title.text = string.Empty;
        _content.text = string.Empty;
        _loadedTexture.texture = _baseTex;
        gameObject.SetActive(false);
    }
    private void LoadMedia()
    {
        if (Loader.OpenMediaFileFromDevice(false, out var path))
        {
            _mediaPath = path;
            if (_mediaPath != "")
            {
                _loadedTexture.texture = Loader.LoadTexture(_mediaPath);
                Debug.LogWarning("Loaded Image for Edit Muc4: " +  _mediaPath);
            }
        }
        else
        {
            _loadedTexture.texture = _baseTex;
        }
    }
    public void OpenEdit(int order, System.Action<ContentData> onEditComplete)
    {
        gameObject.SetActive(true);
        _mediaPath = string.Empty;
        if (_loadedTexture.texture != _baseTex)
        {
            //Texture.Destroy(_loadedTexture.texture);
            _loadedTexture.texture = _baseTex;
        }
        _order.text = order.ToString();
        _onEditCompelete = (onEditComplete);
    }
    public void OpenEdit(ContentData data, System.Action<ContentData> onEditComplete)
    {
        gameObject.SetActive(true);
        _mediaPath = StreamingAssetHelper.GetPath(data.MediaPath);
        if (_mediaPath != "")
        {
            var texture = Loader.LoadTexture(_mediaPath);
            if (texture)
            {
                _loadedTexture.texture = texture;
            }
        }
        _order.text = (data.Order + 1).ToString();
        _content.text = data.Content;
        _title.text = data.Title;
        _onEditCompelete = (onEditComplete);
    }
    private void ClearMedia()
    {
        if (_mediaPath == string.Empty) return;
        _mediaPath = string.Empty;
        //Destroy(_loadedTexture.texture);
        _loadedTexture.texture = _baseTex;
    }
}
