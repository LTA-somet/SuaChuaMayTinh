using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
namespace Tu.Mohinh3D
{
    public class EditDetail : MonoBehaviour
    {
        [SerializeField] Button _saveBtn;
        [SerializeField] Button _cancelBtn;
        [SerializeField] Button _loadMediaBtn;
        [SerializeField] Button _editMediaBtn;
        [SerializeField] GameObject _menuContainer;
        [SerializeField] Button _clearMediaBtn;
        [SerializeField] RawImage _loadedTexture;
        [SerializeField] TMPro.TMP_InputField _content;
        Texture _baseTex = null;

        string _mediaPath = string.Empty;
        System.Action<ContentData> _onEditCompelete = null;


        private void Awake()
        {
            _loadMediaBtn.onClick.AddListener(() => _menuContainer.SetActive(!_menuContainer.activeInHierarchy));
            _editMediaBtn.onClick.AddListener(LoadMedia);
            _clearMediaBtn.onClick.AddListener(ClearMedia);
            _saveBtn.onClick.AddListener(Save);
            _cancelBtn.onClick.AddListener(Close);
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
            _content.text = data.Content;
            _onEditCompelete = (onEditComplete);
        }
        public void Close()
        {
            gameObject.SetActive(false);
            _menuContainer.SetActive(false);
        }
        public void Save()
        {
            string path = string.Empty;
            if (File.Exists(_mediaPath))
            {
                path = StreamingAssetHelper.ImportImageToStreamingAssets(_mediaPath, Path.GetFileName(_mediaPath));
            }
            if (_onEditCompelete != null)
            {
                _onEditCompelete.Invoke(new ContentData()
                {
                    Content = _content.text,
                    MediaPath = path
                });
                _onEditCompelete = null;
            }
            Close();
        }

        private void LoadMedia()
        {

            if (Loader.OpenMediaFileFromDevice(false, out var path))
            {
                _mediaPath = path;
                if (_mediaPath != "")
                {
                    _loadedTexture.texture = Loader.LoadTexture(_mediaPath);
                    Debug.LogWarning("Loaded Image for Edit Muc4: " + _mediaPath);
                }
            }
            else
            {
                _loadedTexture.texture = _baseTex;
            }
        }
        private void ClearMedia()
        {
            if (_mediaPath == string.Empty) return;
            _mediaPath = string.Empty;
            //Destroy(_loadedTexture.texture);
            _loadedTexture.texture = _baseTex;
        }
    }
}
