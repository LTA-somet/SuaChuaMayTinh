using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SlideVideo : MonoBehaviour
{
    [SerializeField] VideoPlayer _videoPlayer;
    [SerializeField] RawImage _videoImage;
    [SerializeField] TMPro.TextMeshProUGUI _titleText;
    [SerializeField] TMPro.TextMeshProUGUI _contentText;
    [Header("Controller")]
    [SerializeField] ToggleButton _playBtn;
    [SerializeField] ToggleButton _soundBtn;
    [SerializeField] Button _replayButton;
    [SerializeField] RectTransform _controllerContainer;
    [SerializeField] Slider _videoSlider;

    Texture _baseTex;
    private void Awake()
    {
        _baseTex = _videoImage.mainTexture;
        _videoSlider.onValueChanged.AddListener(OnSliderValueChanged);
        _replayButton.onClick.AddListener(Replay);
        _soundBtn.onToggle.AddListener(ToggleSound);
        _playBtn.onToggle.AddListener(ToggleVideo);
        //SoundManager.Instance.StopOneShot();
    }
    private void OnEnable()
    {
        _videoImage.enabled = false;
        ToggleVideo(true);
        ToggleSound(true);
        _playBtn.SetOn(true);
        _soundBtn.SetOn(true);
    }
    private void OnDisable()
    {
        if (_videoPlayer.isPrepared)
        {
            _videoPlayer.Stop();
        }
    }
    [ContextMenu("PlayDemo")]
    private void OpenDemo()
    {
        //OpenSlide(VideoDemo);
    }
    private void Replay()
    {
        _videoPlayer.time = 0;
        if (_videoPlayer.isPrepared)
        {
            _videoPlayer.Play();
        }
    }    
    private void ToggleSound(bool unMute)
    {
        _videoPlayer.SetDirectAudioVolume(0,unMute ? 1 : 0);
    }
    private void ToggleVideo(bool newPlay)
    {
        if (newPlay)
        {
            _videoPlayer.Play();
        }
        else
        {
            _videoPlayer.Pause();
        }

    }
    private void OnSliderValueChanged(float value)
    {
        float totalTime = _videoPlayer.frameCount / _videoPlayer.frameRate;
        _videoPlayer.time = value * totalTime;
    }

    private void LateUpdate()
    {
        if (_videoPlayer.isPrepared && _videoPlayer.isPlaying)
        {
            float totalTime = _videoPlayer.frameCount / _videoPlayer.frameRate;
            _videoSlider.SetValueWithoutNotify((float)_videoPlayer.time / totalTime);
        }
    }

    public void OpenSlide(SlideData data)
    {
        if (data.MediaPath != string.Empty)
        {
            _videoPlayer.source = VideoSource.Url;
            _videoPlayer.url = StreamingAssetHelper.GetPath(data.MediaPath);
            StartCoroutine(PlayVideo());
        }
        else
        {
            _videoImage.texture = _baseTex;
        }

        _titleText.text = data.Title;
        _contentText.text = data.Content;


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

        _videoImage.texture = _videoPlayer.texture;

        _videoImage.enabled = true;
    }
    public void ShowControl()
    {
        _controllerContainer.DOKill();
        var oldPos = _controllerContainer.anchoredPosition;
        _controllerContainer.DOAnchorPosY(0, Mathf.Lerp(0, 0.5f, Mathf.Abs(oldPos.y) / 100));
    }
    public void HideControl()
    {
        _controllerContainer.DOKill();
        var oldPos = _controllerContainer.anchoredPosition;
        _controllerContainer.DOAnchorPosY(-100, Mathf.Lerp(0.5f, 0, Mathf.Abs(oldPos.y) / 100));
    }
}
