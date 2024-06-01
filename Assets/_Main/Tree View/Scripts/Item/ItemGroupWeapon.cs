using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace Nam.TreeView
{
    public class ItemGroupWeapon : CustomMonoBehaviour
    {
        [SerializeField]
        TextMeshProUGUI _text;
        [SerializeField]
        Image _image;
        TreeView _treeView;
        [SerializeField]
        bool isGun = true;
        int _idGroupWeapon;
        private void Start()
        {
            ToggleButton toggleButton = GetComponent<ToggleButton>();

            //toggleButton.Button.onClick.AddListener(() =>
            //{

            //    toggleButton.SetNewActive(true);
            //    //_treeView.ResetAllButtonTreeView(_treeView.ListWeapon);
            //    _treeView.ResetAllButtonTreeView(_treeView.ListGroupWeapon);
            //    //_treeView.ListWeapon.GetChild(0).GetComponent<ToggleButton>().SetNewActive(true);
            //   //_treeView.LoadWeaponData(_idGroupWeapon);

            //});
        }

        //truyền dữ liệu cho item
        public void SetData(TreeViewData treeViewData, Sprite sprite, TreeView treeView)
        {
            _idGroupWeapon = treeViewData.ID;
            _text.text = treeViewData.Name;
            _image.sprite = sprite;
            _image.SetNativeSize();
            _treeView = treeView;
        }

        protected override void LoadComponent()
        {
            base.LoadComponent();
            _text = transform.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
            _image = transform.Find("Icon").GetComponent<Image>();
        }
    }
}
