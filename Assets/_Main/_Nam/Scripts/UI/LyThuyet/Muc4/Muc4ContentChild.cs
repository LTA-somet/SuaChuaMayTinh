using AsyncTextureImport;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Muc4ContentChild : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] Button _button;
    [SerializeField] Button _editButton;
    [SerializeField] Button _deleteButton;
    [SerializeField] private RawImage _contentRawImage;
    [SerializeField] private TMPro.TextMeshProUGUI _contentTitle;
    [SerializeField] private TMPro.TextMeshProUGUI _contentText;
    Transform _tr;
    Muc4UI _parent;
    ContentData _data;
    Texture _baseSprite;
    public ContentData Data => _data;
    private void Awake()
    {
        _tr = transform;
        _baseSprite = _contentRawImage.texture;
        _button.onClick.AddListener(OpenMuc5);
        _editButton.onClick.AddListener(() => EditTheory.Instance.Muc4.OpenEdit(_data, OnSaveEdit));
        _deleteButton.onClick.AddListener(() => NotiPanel.Instance.ShowNotify("Bạn có muốn xoá?", Delete));
    }

    private void Delete()
    {
        if (_parent)
        {
            if (_data.ChildrenContent != null)
            {
                foreach (var slide in _data.ChildrenContent)
                {
                    StreamingAssetHelper.DeleteFile(slide.MediaPath);
                }
            }
            StreamingAssetHelper.DeleteFile(_data.MediaPath);
            _parent.DeleteChild(_data.Order);
        }
    }

    private void OnSaveEdit(ContentData data)
    {
        Debug.LogWarning($"Update data: {_data.Title} | {data.Title}");
        _data.Title = data.Title;
        _data.Content = data.Content;
        if (_data.MediaPath != data.MediaPath)
        {
            StreamingAssetHelper.DeleteFile(_data.MediaPath);
            _data.MediaPath = data.MediaPath;
        }
        if (data.Order > _data.Order)
            _data.Order = data.Order + 1;
        else if (data.Order < _data.Order)
            _data.Order = data.Order - 1;
        //data.ChildrenContent = _data.ChildrenContent;
        AssignData(data);
    }
    public void InitData(Muc4UI parent, ContentData data)
    {
        _parent = parent;
        InitData(data);
    }
    public void InitData(ContentData data)
    {
        AssignData(data);
        _data = data;
    }
    Coroutine loadTexRoutine = null;
    private void AssignData(ContentData data)
    {
        if (data.MediaPath != string.Empty)
        {
            if (loadTexRoutine != null)
            {
                StopCoroutine(loadTexRoutine);
            }
            loadTexRoutine = StartCoroutine(LoadTexture(data.MediaPath));
        }
        else
        {
            _contentRawImage.texture = _baseSprite;
        }
        _contentTitle.text = data.Title;
        _contentText.text = data.Content;
    }
    IEnumerator LoadTexture(string path)
    {
        path = StreamingAssetHelper.GetPath(path);
        // Create texture importer
        TextureImporter importer = new TextureImporter();

        // Import texture async
        //yield return importer.ImportTexture(path, FREE_IMAGE_FORMAT.FIF_JPEG);
        if (path.Split(".")[path.Split(".").Length - 1] == "png")
        {
            yield return importer.ImportTexture(path, FREE_IMAGE_FORMAT.FIF_PNG);
        }
        else
        {
            yield return importer.ImportTexture(path, FREE_IMAGE_FORMAT.FIF_JPEG);
        }
        //yield return importer.ImportTexture(path, FREE_IMAGE_FORMAT.);

        // Fetch the result
        Texture2D tex = importer.texture;
        if (tex != null)
        {
            _contentRawImage.texture = tex;
        }
        else
        {
            _contentRawImage.texture = GetTexture(path);
        }

        yield return null;

        _contentRawImage.rectTransform.sizeDelta = SetSizeImage(371, 254, new Vector2(_contentRawImage.texture.width, _contentRawImage.texture.height));
        if (!_contentRawImage.texture)
        {
            _contentRawImage.texture = _baseSprite;
            _contentRawImage.SetNativeSize();
        }
    }
    private void OpenMuc5()
    {
        this.DispatchEvent(EventKey.REQUEST_OPEN_THEORY_Level_5, _data);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        _tr.localScale = Vector3.one * 1.1f;
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        _tr.localScale = Vector3.one;
    }
    #region sử lý load Image
    // lấy size của ảnh được load lên
    public Vector2 GetSizeSprite(string imagePath)
    {
        var filePath = imagePath;  // Lấy đường dẫn của tập tin
        if (!File.Exists(filePath))
        {
            filePath = Application.streamingAssetsPath + "/PMTL-VK-ICON.2_muc4-anhlithuyet_3d.png";
        }
        // Chuyển đổi đường dẫn mong muốn thành mảng byte
        byte[] pngBytes = System.IO.File.ReadAllBytes(filePath);

        // Tạo texture và tải dữ liệu mảng byte để tạo hình ảnh
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(pngBytes);
        return new Vector2(tex.width, tex.height);

    }
    public Texture GetTexture(string imagePath)
    {
        var filePath = imagePath;  // Lấy đường dẫn của tập tin
        if (!File.Exists(filePath))
        {
            filePath = Application.streamingAssetsPath + "/PMTL-VK-ICON.2_muc4-anhlithuyet_3d.png";
        }
        // Chuyển đổi đường dẫn mong muốn thành mảng byte
        byte[] pngBytes = System.IO.File.ReadAllBytes(filePath);

        // Tạo texture và tải dữ liệu mảng byte để tạo hình ảnh
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(pngBytes);
        return tex;

    }
    public Vector2 SetSizeImage(float maxX, float maxY, Vector2 spriteSize)
    {
        float x = spriteSize.x;
        float y = spriteSize.y;

        // Kiểm tra nếu kích thước của sprite vượt quá giới hạn cho phép
        if (spriteSize.x > maxX || spriteSize.y > maxY)
        {
            // Tính tỷ lệ co dãn cần thiết theo chiều ngang và dọc
            float scaleX = maxX / spriteSize.x;
            float scaleY = maxY / spriteSize.y;

            // So sánh tỷ lệ co dãn và áp dụng tỷ lệ nhỏ hơn
            if (scaleX < scaleY)
            {
                x *= scaleX;
                y *= scaleX;
            }
            else
            {
                x *= scaleY;
                y *= scaleY;
            }
        }

        // Trả về kích thước mới của sprite sau khi đã thay đổi
        return new Vector2(x, y);
    }
    #endregion
}
