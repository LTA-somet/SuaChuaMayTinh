using System.Collections;
using System.Collections.Generic;
using System.IO;
using TDLN.CameraControllers;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameManager>();
            return instance;
        }
    }

    public static void DeleteAsset(string file)
    {
        if (File.Exists(file))
        {
            File.Delete(file);
        }
    }

    private void Awake()
    {
        DisableAllRigs();

    }
    public void ResetCamera()
    {
        _orbitCamera.GetComponent<OrbitCamera>().SetCameraView(394.4f, 45.19003f, 7);
    }
    [SerializeField] private Camera _orbitCamera;
    public Camera OrbitCamera { get { return _orbitCamera; } }
    [SerializeField] private List<GameObject> _rigs = new List<GameObject>();
    public List<GameObject> Rigs { get { return _rigs; } }
    public GameObject GetGameObject(string name)
    {
        for (int i = 0; i < _rigs.Count; i++)
        {
            if (_rigs[i].GetComponent<RigController>().RigName == name)
            {
                return _rigs[i];
            }
        }
        Debug.LogError("không tìm thấy GameObject");
        return null;


    }
    public void EnableRig(object o)
    {
        EnableRig((string)o);
    }
    public bool TryEnableRig(string rigName, out GameObject rig)
    {
        DisableAllRigs();
        rig = GetGameObject(rigName);
        if (rig == null) { return false; }
        rig.SetActive(true);
        return true;
    }
    public void DisableAllRigs()
    {
        foreach (var rig in _rigs)
        {
            rig.gameObject.SetActive(true);
        }
    }
}
