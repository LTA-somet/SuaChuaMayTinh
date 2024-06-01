using Nam.TreeView;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public enum UIType : byte
{
    Theory = 0,
    Model = 1,
}
public class Muc3ChildContent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TMPro.TextMeshProUGUI _title;
    [SerializeField] Image _contentMedia;
    [SerializeField] GameObject _outline;
    [SerializeField] Button _button;
    [SerializeField] Button _editBtn;
    [SerializeField] Button _deleteBtn;
    [SerializeField] UIType _type = UIType.Theory;

    Muc3ChildData _data;
    public Muc3ChildData Data => _data;

    private void Awake()
    {
        _outline.SetActive(false);
        _button.onClick.AddListener(OpenMuc4);
        _deleteBtn.onClick.AddListener(Delete);
        _editBtn.onClick.AddListener(OpenEdit);
    }
    public void InitData(Muc3ChildData data, int part, bool isSymbol)
    {
        _data = data;
        _contentMedia.gameObject.SetActive(true);
        if (data.DemoMediaPath != string.Empty)
        {
            _contentMedia.sprite = StreamingAssetHelper.GetSpriteFromStreamingAsset(data.DemoMediaPath);
            _contentMedia.transform.parent.GetComponent<RectTransform>().sizeDelta = SetSizeImage(133, 136, GetSizeSprite(Application.streamingAssetsPath + "\\" + data.DemoMediaPath));
        }
        else
        {
            var sprite = StreamingAssetHelper.GetSpriteFromStreamingAsset($"Images/icon_3/{part + 1}.png");
            if (sprite)
            {
                _contentMedia.sprite = sprite;
                _contentMedia.transform.parent.GetComponent<RectTransform>().sizeDelta = SetSizeImage(133, 136, GetSizeSprite($"Images/icon_3/{part + 1}.png"));
            }
            else
            {
                _contentMedia.gameObject.SetActive(false);
            }
        }
        _title.text = data.Title;
    }
    public void OpenEdit()
    {
        EditTheory.Instance.Muc3.OpenEditData(Data, OnSave);
        void OnSave(Muc3ChildData data)
        {
            if (data.DemoMediaPath != StreamingAssetHelper.GetPath(_data.DemoMediaPath))
            {
                var newMediaPath = StreamingAssetHelper.ImportImageToStreamingAssets(data.DemoMediaPath, Path.GetFileName(data.DemoMediaPath));
                StreamingAssetHelper.DeleteFile(_data.DemoMediaPath);
                _data.DemoMediaPath = newMediaPath;
            }
            _data.Title = data.Title;
            _data.Order = data.Order;
            _data.ChildrenContent = data.ChildrenContent;
            this.DispatchEvent(EventKey.REQUEST_REFRESH_SEGMENT_LEVEL_3);
        }
    }
    public void Delete()
    {
        NotiPanel.Instance.ShowNotify("Bạn có muốn xoá bài học", DeleteData);
    }
    public void OpenMuc4()
    {
        this.DispatchEvent(EventKey.REQUEST_OPEN_THEORY_Level_4, _data);
        MainUI.Instance.TreeView.SetActive(true);
        MainUI.Instance.TreeView.GetComponent<TreeView>().setData(MainUI.Instance.Theory.GetComponent<TheoryUI>().Muc3UI.Datas, false);
    }
    private void DeleteData()
    {
        var muc3 = GetComponentInParent<Muc3UI>();
        muc3.RemoveChild(this);
    }
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        _outline.SetActive(true);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        _outline.SetActive(false);
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
