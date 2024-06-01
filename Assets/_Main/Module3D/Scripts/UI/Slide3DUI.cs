using PCmodel.TreeView;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tu.Mohinh3D
{
    public class Slide3DUI : MonoBehaviour
    {
        [SerializeField] Button _editButtun;
        [SerializeField] GameObject _bottom;
        [SerializeField] Button _expandBtn;
        [SerializeField] Button _closeBtn;
        [SerializeField] GameObject detailTable;
        [Header("content")]
        [SerializeField] TMPro.TextMeshProUGUI _title;
        [SerializeField] TMPro.TextMeshProUGUI _childTitle;
        [SerializeField] TMPro.TextMeshProUGUI _content;
        [SerializeField] Image _contentMedia;
        [SerializeField] TreeView _treeView;
        Sprite _baseSprite;
        List<ContentData> _data;


        public List<ContentData> Data => _data;
        int _index = -1;
        public int Index => _index;
        private void OnEnable()
        {
            this.AddEvent(EventKey.REQUEST_OPEN_SELECT_UI, getIndex);
        }
        private void Start()
        {
            _bottom.SetActive(false);
            detailTable.SetActive(false);
            _closeBtn.onClick.AddListener(() => { gameObject.SetActive(false); MainUI.Instance.TreeView.GetComponent<Nam.TreeView.TreeView>().Init(); MainUI.Instance.TreeView.SetActive(false); });
            _expandBtn.onClick.AddListener(() =>
            {
                if (Index >= Data.Count) return;
                detailTable.SetActive(!detailTable.activeInHierarchy);
                Insert();
            });
            _editButtun.onClick.AddListener(() => EditSimulate3D.Instance.editDetail.OpenEdit(_data[_index], OnSaveEdit));
        }
        private void Update()
        {
            if (Index < 0)
            {
                _bottom.SetActive(false);
            }
            if (Index >= 0)
            {
                _bottom.SetActive(true);
            }
        }
        public void ReloadTreeView(GameObject go)
        {
            if (go == null) return;
            _treeView.SetNewTreeviewObject(go);
        }
        private void OnDisable()
        {
            this.DropEvent(EventKey.REQUEST_OPEN_SELECT_UI, getIndex);
        }
        public void InitTitle(string data)
        {
            _title.text = data;
        }
        public void Init(Muc3ChildData newData)
        {
            _childTitle.text = newData.Title;

        }
        public void getIndex(object o)
        {
            if ((string)o == string.Empty)
            {
                _index = -1;
            }
            else _index = Convert.ToInt32(o);
            Debug.Log("id 3d:" + _index);
        }
        public void InitContent(List<ContentData> data)
        {
            _data = data;
        }
        public void Insert()
        {

            if (_data[_index].MediaPath != string.Empty)
            {
                _contentMedia.sprite = StreamingAssetHelper.GetSpriteFromStreamingAsset(_data[_index].MediaPath);
                _contentMedia.transform.GetComponent<RectTransform>().sizeDelta = SetSizeImage(385, 325, GetSizeSprite(Application.streamingAssetsPath + "\\" + _data[_index].MediaPath));
            }
            else
            {
                var sprite = StreamingAssetHelper.GetSpriteFromStreamingAsset($"Images/icon_3/{1}.png");
                //if (sprite)
                //{

                //    _contentMedia.transform.GetComponent<RectTransform>().sizeDelta = SetSizeImage(385, 325, GetSizeSprite($"Images/icon_3/{1}.png"));
                //}
                _contentMedia.sprite = sprite;
                _contentMedia.SetNativeSize();
            }
            _content.text = _data[_index].Content;
            if (_content.transform.parent.GetComponent<RectTransform>().localPosition != Vector3.zero)
                _content.transform.parent.GetComponent<RectTransform>().localPosition = Vector3.zero;
        }

        private void OnSaveEdit(ContentData data)
        {
            if (_data[_index].MediaPath != data.MediaPath)
            {
                StreamingAssetHelper.DeleteFile(_data[_index].MediaPath);
                _data[_index].MediaPath = data.MediaPath;
            }
            _data[_index].Content = data.Content;
            AssignData(data);
        }
        private void AssignData(ContentData data)
        {
            if (data.MediaPath != string.Empty)
            {
                _contentMedia.sprite = StreamingAssetHelper.GetSpriteFromStreamingAsset(data.MediaPath);
                _contentMedia.rectTransform.sizeDelta = SetSizeImage(385, 325, GetSizeSprite(Application.streamingAssetsPath + "\\" + data.MediaPath));
                if (!_contentMedia.sprite)
                {
                    _contentMedia.sprite = _baseSprite;
                    _contentMedia.SetNativeSize();
                }
            }
            else
            {
                _contentMedia.sprite = _baseSprite;
            }
            _content.text = data.Content;
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
    }
}