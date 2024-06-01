using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TheoryUI : MonoBehaviour
{
    string DATA_PATH = "TheoryData.txt";
    public TheoryData Data;
    [SerializeField] Muc3UI muc3;
    public Muc3UI Muc3UI => muc3;
    [SerializeField] Muc4UI muc4;
    public Muc4UI Muc4UI => muc4;
    [SerializeField] Muc5UI muc5;
    public Muc5UI Muc5UI => muc5;

    [SerializeField] bool _load, _save;
    private void Awake()
    {
        this.AddEvent(EventKey.REQUEST_OPEN_THEORY_Level_4, OpenLevel4);
        this.AddEvent(EventKey.REQUEST_OPEN_THEORY_Level_5, OpenLevel5);
        this.AddEvent(EventKey.REQUEST_OPEN_THEORY_SYMBOL, OpenSymbol);
        StartCoroutine(AutoSaveRoutine());
#if UNITY_EDITOR
        if (_load)
        {
            LoadData();
        }
#else
            LoadData();
#endif

        muc3.InitData(Data.Muc3Datas);
    }
    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        if (_save)
        {
            SaveData();
        }
#else
            SaveData();
#endif
    }
    private void OnEnable()
    {
        muc4.gameObject.SetActive(false);
        muc5.gameObject.SetActive(false);
        muc3.gameObject.SetActive(true);
        Debug.Log(Data.name);
    }
    private void OnDestroy()
    {
        this.DropEvent(EventKey.REQUEST_OPEN_THEORY_Level_4, OpenLevel4);
        this.DropEvent(EventKey.REQUEST_OPEN_THEORY_Level_5, OpenLevel5);
        this.DropEvent(EventKey.REQUEST_OPEN_THEORY_SYMBOL, OpenSymbol);

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(Data);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        if (_save)
        {
            SaveData();
        }
#else
            SaveData();
#endif
    }
    private void OpenLevel4(object o)
    {
        muc4.gameObject.SetActive(true);
        muc4.Init((Muc3ChildData)o);
    }
    private void OpenLevel5(object o)
    {
        muc5.gameObject.SetActive(true);
        muc5.InitData((ContentData)o);
    }
    private void OpenSymbol(object o)
    {

    }
    private void LoadData()
    {
        DATA_PATH = $"{Data.name}.txt";
        string jsonData = StreamingAssetHelper.ReadJsonFromFile(DATA_PATH);
        //Debug.Log(jsonData);
        JsonUtility.FromJsonOverwrite(jsonData, Data);
        //jsonData = JsonUtility.ToJson(Data);
        //Debug.Log(jsonData);
    }
    private void SaveData()
    {
        DATA_PATH = $"{Data.name}.txt";
        string jsonData = JsonUtility.ToJson(Data);
        StreamingAssetHelper.SaveJsonToFile(jsonData, DATA_PATH);
    }
    IEnumerator AutoSaveRoutine()
    {
        while (true)
        {
            yield return Yielder.WaitForSeconds(10);
            SaveData();
        }
    }
}