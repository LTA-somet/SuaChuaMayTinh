using Nam.TreeView;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Tu.Mohinh3D;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Muc3childSimulate : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] TMPro.TextMeshProUGUI _title;
    [SerializeField] Image _contentMedia;
    [SerializeField] GameObject _outline;
    [SerializeField] Button _button;
    [SerializeField] UIType _type = UIType.Theory;


    List<ContentData> _contentDatas = new List<ContentData>();
    Muc3Data _Muc3Data;
    Muc3ChildData _data;
    public Muc3ChildData Data => _data;
    public Muc3Data data => _Muc3Data;
    string _titleMuc4;
    private void Awake()
    {
        _outline.SetActive(false);
        _button.onClick.AddListener(OpenSLide3D);

    }
    public void InitData(Muc3ChildData data, int part, bool isSymbol, string title)
    {
        _titleMuc4 = title;
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

        List<ContentData> contentDatas = data.ChildrenContent[0].ChildrenData;
        _contentDatas = contentDatas;
    }
    public void OpenSLide3D()
    {
        MainUI mainUI = MainUI.Instance;
        List<Muc3Data> muc3Datas = mainUI.Simulation3D.GetComponent<Simulation3DUI>().Muc3Simulate.Datas;
        mainUI.TreeView.SetActive(true);
        mainUI.TreeView.GetComponent<TreeView>().setData(muc3Datas, true);
        Muc3ChildData muc3ChildData = _data;
        Muc4Data muc4Data = muc3ChildData.ChildrenContent[0];
        GameManager gameManager = GameManager.Instance;
        for (int i = 0; i < gameManager.Rigs.Count; i++)
        {
            gameManager.Rigs[i].gameObject.SetActive(false);
        }
        gameManager.Rigs[int.Parse(muc4Data.uID)].gameObject.SetActive(true);
        if (_data.uID == "Mô hình 3D")
        {
            this.DispatchEvent(EventKey.REQUEST_OPEN_MODEL_Slide3D1, _titleMuc4);
            this.DispatchEvent(EventKey.REQUEST_OPEN_EXPAND_Slide3D, _contentDatas);
            this.DispatchEvent(EventKey.REQUEST_OPEN_MODEL_Slide3D, _data);
        }
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
