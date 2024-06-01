using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SlideImage : MonoBehaviour
{
    [SerializeField] RawImage _media;
    [SerializeField] TMPro.TextMeshProUGUI _title;
    [SerializeField] TMPro.TextMeshProUGUI _content;
    Texture _baseTex;
    private void Awake()
    {
        _baseTex = _media.mainTexture;

    }
    public void OpenSlide(SlideData data)
    {
        if (data.MediaPath != string.Empty)
        {
            _media.texture = StreamingAssetHelper.GetTextureFromStreamingAsset(data.MediaPath);
            _media.rectTransform.sizeDelta = SetSizeImage(910, 770, GetSizeSprite(Application.streamingAssetsPath + "\\" + data.MediaPath));
        }
        else
        {
            _media.texture = _baseTex;
            _media.SetNativeSize();
        }

        _title.text = data.Title;
        _content.text = data.Content;
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
