using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
    private static MainUI instance;
    public static MainUI Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<MainUI>();
            return instance;
        }
    }
    [SerializeField] GameObject _muc2UI;
    public GameObject Muc2UI => _muc2UI;
    [SerializeField] GameObject _theory;
    public GameObject Theory => _theory;
    [SerializeField] GameObject _simulation3D;
    public GameObject Simulation3D => _simulation3D;
    [SerializeField] GameObject _top;
    public GameObject Top => _top;
    [SerializeField] Button _quitBtn;
    [SerializeField] GameObject _noti;

    [SerializeField] GameObject _treeView;
    public GameObject TreeView => _treeView;

    private void Awake()
    {
        _quitBtn.onClick.AddListener(CheckQuit);
        Application.targetFrameRate = 60;
    }
    private void CheckQuit()
    {
        if (_noti.activeSelf)
        {
            NotiPanel.Instance.ShowNotify("Bạn có muốn thoát?", Application.Quit);
        }
        else
        {
            _noti.gameObject.SetActive(true);
        }
    }

    private void Reset()
    {
        _muc2UI = transform.Find("Muc 2").gameObject;
        _theory = transform.Find("Theory").gameObject;
        _simulation3D = transform.Find("Simulation 3D").gameObject;
        _noti = GameObject.Find("NotiCanvas");
        _top = transform.Find("Top").gameObject;
        _quitBtn = _top.transform.Find("QuitBtn").GetComponent<Button>();
        _treeView = transform.Find("Tree View").gameObject;
    }
}
