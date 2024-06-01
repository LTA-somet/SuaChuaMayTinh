using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class EditMuc5 : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Button _loadMediaBtn, _imageBtn, _videoBtn, _clearMediaBtn;
    [SerializeField] Button _saveBtn;
    [SerializeField] Button _cancelBtn;
    [SerializeField] GameObject _editBtnContainer;
    [SerializeField] VideoPlayer _videoPlayer;
    [SerializeField] RawImage _mediaRender;
    [SerializeField] TMPro.TMP_InputField _order, _title, _content;
    ContentType _type = ContentType.Image;
    string _mediaPath = string.Empty;
    UnityEvent<SlideData> _onEditCompelete = new UnityEvent<SlideData>();
    Texture _baseTex = null;

    private void Awake()
    {
        _baseTex = _mediaRender.mainTexture;
        _saveBtn.onClick.AddListener(Save);
        _cancelBtn.onClick.AddListener(Close);
        _loadMediaBtn.onClick.AddListener(() => _editBtnContainer.gameObject.SetActive(true));
        _imageBtn.onClick.AddListener(LoadImage);
        _videoBtn.onClick.AddListener(LoadVideo);
        _clearMediaBtn.onClick.AddListener(ClearMedia);
    }
    private void Save()
    {
        string path = string.Empty;
        if (_mediaPath != string.Empty)
        {
            if (_type == ContentType.Video)
            {
                path = StreamingAssetHelper.ImportVideoToStreamingAssets(_mediaPath, Path.GetFileName(_mediaPath));
            }
            else if (_type == ContentType.Image)
            {
                path = StreamingAssetHelper.ImportImageToStreamingAssets(_mediaPath, Path.GetFileName(_mediaPath));
            }
        }
        _onEditCompelete.Invoke(new SlideData()
        {
            Title = _title.text,
            Content = _content.text,
            Order = int.Parse((_order).text) - 1,
            MediaPath = path,
            Type = _type
        });
        Close();
    }
    private void Close()
    {
        if (_mediaRender.texture != _baseTex)
        {
            //Texture.Destroy(_mediaRender.texture);
            _mediaRender.texture = _baseTex;
        }
        _mediaPath = string.Empty;
        _order.text = string.Empty;
        _title.text = string.Empty;
        _content.text = string.Empty;

        _onEditCompelete.RemoveAllListeners();
        gameObject.SetActive(false);
    }
    private void LoadImage()
    {

        if(LoadMedia(ContentType.Image))
        {
            _type = ContentType.Image;
        }
    }
    private void LoadVideo()
    {

        if(LoadMedia(ContentType.Video))
        {
            _type = ContentType.Video;
        }
    }
    private bool LoadMedia(ContentType type)
    {
        if (type == ContentType.Image)
        {
            if (Loader.OpenMediaFileFromDevice(false, out var path))
            {
                _mediaPath = path;
                if (_mediaPath != "")
                {
                    if (_mediaRender.texture != _baseTex)
                    {
                        //Texture.Destroy(_mediaRender.texture);
                        _mediaRender.texture = _baseTex;
                    }
                    _mediaRender.texture = Loader.LoadTexture(_mediaPath);
                    return true;
                }
            }
        }
        else if (type == ContentType.Video)
        {
            if (Loader.OpenMediaFileFromDevice(true, out var path))
            {
                _mediaPath = path;
                if (_mediaPath != "")
                {
                    _videoPlayer.source = VideoSource.Url;
                    _videoPlayer.url = (_mediaPath);
                    StartCoroutine(PlayVideo());
                    return true;
                }
            }
        }
        return false;
    }
    public void OpenEdit(int order, UnityAction<SlideData> onEditComplete)
    {
        gameObject.SetActive(true);
        _mediaRender.texture = _baseTex;
        _mediaPath = string.Empty;
        _order.text = (order).ToString();
        _title.text = string.Empty;
        _content.text = string.Empty;
        _onEditCompelete.AddListener(onEditComplete);
    }
    public void OpenEdit(SlideData data, UnityAction<SlideData> onEditComplete)
    {
        gameObject.SetActive(true);
        _mediaPath = StreamingAssetHelper.GetPath(data.MediaPath);
        _type = data.Type;
        if (_mediaPath != string.Empty)
        {
            if (_type == ContentType.Image)
            {
                var tex = Loader.LoadTexture(_mediaPath);
                if (tex)
                {
                    _mediaRender.texture = tex;
                }
            }
            else if (_type == ContentType.Video)
            {
                _videoPlayer.source = VideoSource.Url;
                _videoPlayer.url = _mediaPath;
                StartCoroutine(PlayVideo());
            }
        }
        _title.text = data.Title;
        _order.text = (data.Order + 1).ToString();
        _content.text = data.Content;
        _onEditCompelete.AddListener(onEditComplete);
    }
    private IEnumerator PlayVideo()
    {

        // We must set the audio before calling Prepare, otherwise it won't play the audio

        //_videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        //_videoPlayer.controlledAudioTrackCount = 1;

        //_videoPlayer.EnableAudioTrack(0, true);

        //_videoPlayer.SetTargetAudioSource(0, SoundManager.Instance.Source);



        // Wait until ready

        _videoPlayer.Prepare();

        while (!_videoPlayer.isPrepared)
            yield return Yielder.WaitForEndOfFrame;



        _videoPlayer.Play();

        _mediaRender.texture = _videoPlayer.texture;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_editBtnContainer.activeSelf)
        {
            _editBtnContainer.SetActive(false);
        }
    }
    private void ClearMedia()
    {
        if (_mediaPath == string.Empty) return;
        _videoPlayer.Stop();
        //Destroy(_mediaRender.texture);
        _mediaPath = string.Empty;
        _mediaRender.texture = _baseTex;
    }
}
