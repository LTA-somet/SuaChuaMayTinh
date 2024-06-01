using System;
using System.Collections;
using System.Collections.Generic;
using Tu.Animation.ThaoLap;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
namespace Tu.Mohinh3D
{
    public class Simulation3DUI : MonoBehaviour
    {
        string DATA_PATH = "Simulate3D.txt";
        public TheoryData Data;
        [SerializeField] Muc3Simulate muc3;
        public Muc3Simulate Muc3Simulate => muc3;
        [SerializeField] Slide3DUI muc3DS;
        [SerializeField] GameObject pratice;
        public GameObject Pratice => pratice;
        [SerializeField] GameObject moHinh3D;
        public GameObject MoHinh3D => moHinh3D;

        [SerializeField] bool _load, _save;
        private void Awake()
        {

            this.AddEvent(EventKey.REQUEST_OPEN_EXPAND_Slide3D, OpenContent);
            this.AddEvent(EventKey.REQUEST_OPEN_MODEL_Slide3D1, Open3DSlide1);
            this.AddEvent(EventKey.REQUEST_OPEN_MODEL_Slide3D, Open3DSlide);
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

            muc3.gameObject.SetActive(true);
            moHinh3D.SetActive(false);
            pratice.SetActive(false);
            Debug.Log(Data.name);

        }
        private void OnDestroy()
        {
            this.DropEvent(EventKey.REQUEST_OPEN_MODEL_Slide3D, Open3DSlide);
            this.DropEvent(EventKey.REQUEST_OPEN_MODEL_Slide3D1, Open3DSlide1);
            this.DropEvent(EventKey.REQUEST_OPEN_EXPAND_Slide3D, OpenContent);
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
        private void Open3DSlide(object o)
        {
            Muc3ChildData muc3ChildData = (Muc3ChildData)o;
            Muc4Data muc4Data = muc3ChildData.ChildrenContent[0];
            GameManager gameManager = GameManager.Instance;
            for (int i = 0; i < gameManager.Rigs.Count; i++)
            {
                gameManager.Rigs[i].gameObject.SetActive(false);
            }
            gameManager.Rigs[int.Parse(muc4Data.uID)].gameObject.SetActive(true);
            moHinh3D.gameObject.SetActive(true);
            moHinh3D.GetComponent<Slide3DUI>().Init((Muc3ChildData)o);
            moHinh3D.GetComponent<Slide3DUI>().ReloadTreeView(gameManager.Rigs[int.Parse(muc4Data.uID)]);
        }
        private void Open3DSlide1(object o)
        {
            moHinh3D.GetComponent<Slide3DUI>().InitTitle((string)o);
        }
        private void OpenContent(object o)
        {
            moHinh3D.gameObject.SetActive(true);
            List<ContentData> a = (List<ContentData>)o;
            moHinh3D.GetComponent<Slide3DUI>().InitContent(a);
        }


        private void LoadData()
        {
            DATA_PATH = $"{Data.name}.txt";
            string jsonData = StreamingAssetHelper.ReadJsonFromFile(DATA_PATH);
            JsonUtility.FromJsonOverwrite(jsonData, Data);
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
}