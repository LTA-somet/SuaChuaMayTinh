using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Tu.Mohinh3D;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Nam.TreeView
{
    public class TreeView : CustomMonoBehaviour
    {
        [SerializeField] private GameObject itemLesson;
        [SerializeField] private Transform listLesson;
        [SerializeField] private GameObject itemModun;
        [SerializeField] private MenuSearch _menuSearch;
        [SerializeField] private Button _btnHome;
        [SerializeField] private GameObject _treeViewMovent;
        [SerializeField] private Transform listModun;
        public Transform ListModun => listModun;
        private List<GameObject> _listUnactiveLesson = new List<GameObject>();
        public List<GameObject> ListUnactiveLesson => _listUnactiveLesson;
        private List<GameObject> _lisActiveLesson = new List<GameObject>();
        public List<GameObject> ListActiveLesson => _lisActiveLesson;
        private List<GameObject> _listUnactivePart = new List<GameObject>();
        public List<GameObject> ListUnactivePart => _listUnactivePart;
        private List<GameObject> _lisActivePart = new List<GameObject>();
        public List<GameObject> LisActivePart => _lisActivePart;

        ToggleButton currSelect;
        public ToggleButton CurrSelect { get { return currSelect; } set { currSelect = value; } }

        public MenuSearch MenuSearch => _menuSearch;
        private void Start()
        {
            _btnHome.onClick.AddListener(ClickHome);
        }
        private void ClickHome()
        {
            MainUI mainUI = MainUI.Instance;
            TheoryUI theoryUI = mainUI.Theory.GetComponent<TheoryUI>();
            Simulation3DUI simulation3DUI = mainUI.Simulation3D.GetComponent<Simulation3DUI>();
            theoryUI.Muc5UI.gameObject.SetActive(false);
            theoryUI.Muc4UI.gameObject.SetActive(false);
            theoryUI.gameObject.SetActive(false);
            simulation3DUI.Pratice.SetActive(false);
            simulation3DUI.MoHinh3D.SetActive(false);
            simulation3DUI.gameObject.SetActive(false);
            Init();
            gameObject.SetActive(false);
        }
        public void Init()
        {
            scroll scroll = _treeViewMovent.GetComponent<scroll>();
            rotateButton rotateButton = _treeViewMovent.GetComponent<rotateButton>();
            if (!scroll.IsPanelLeft)
            {
                scroll.TogglePanelPosition();
                rotateButton.OnButtonClick();
            }
        }
        public void setData(List<Muc3Data> datas, bool is3D)
        {
            //foreach (Transform child in listLesson)
            //{
            //    Destroy(child.gameObject);
            //    //child.gameObject.SetActive(false);
            //}
            foreach (Transform child in listModun)
            {
                Destroy(child.gameObject);
                //child.gameObject.SetActive(false);
            }
            if (_lisActiveLesson.Count > 0)
            {
                for (int i = 0; i < _lisActiveLesson.Count; i++)
                {
                    _lisActiveLesson[i].SetActive(false);
                    _lisActiveLesson[i].transform.SetParent(null);
                    _listUnactiveLesson.Add(_lisActiveLesson[i]);
                }
                _lisActiveLesson.Clear();
            }
            if (LisActivePart.Count > 0)
            {
                for (int i = 0; i < LisActivePart.Count; i++)
                {
                    LisActivePart[i].SetActive(false);
                    LisActivePart[i].transform.SetParent(null);
                    ListUnactivePart.Add(LisActivePart[i]);
                }
                LisActivePart.Clear();
            }
            GameObject menuObject = null;
            for (int i = 0; i < datas[0].ChildrenContent.Count; i++)
            {
                if (_listUnactiveLesson.Count <= 0)
                {
                    menuObject = Instantiate(itemLesson, listLesson);
                }
                else
                {
                    menuObject = _listUnactiveLesson[0];
                    _listUnactiveLesson.Remove(menuObject);
                    menuObject.transform.SetParent(listLesson);
                }
                _lisActiveLesson.Add(menuObject);
                TMPro.TextMeshProUGUI menuText = menuObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
                menuText.text = "" + datas[0].ChildrenContent[i].Title.ToString();
                menuObject.SetActive(true);
                menuObject.GetComponent<DropChilList>().setData(datas, listLesson, i, 0, this, is3D);
                this.MenuSearch.GetDataLesson(menuObject);
            }

            for (int i = 0; i < datas.Count; i++)
            {
                GameObject newObject = Instantiate(itemModun, listModun);
                TMPro.TextMeshProUGUI menuText = newObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
                menuText.text = "" + datas[i].Title.ToString();
                newObject.SetActive(true);
                if (i == 0)
                {
                    newObject.GetComponent<ToggleButton>().SetOn(true);
                    currSelect = newObject.GetComponent<ToggleButton>();
                }
                newObject.GetComponent<ModunData>().setData(i, datas, listLesson, this, is3D);
            }
            //if (listModun.childCount > 0)
            //{
            //    listModun.GetChild(0).GetComponent<ToggleButton>().SetOn(true);
            //}
        }


    }
}
