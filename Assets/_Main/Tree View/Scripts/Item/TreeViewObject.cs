using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Nam.TreeView
{
    //Lớp đối tượng hiển thị trên treeview
    public abstract class TreeViewObject : MonoBehaviour
    {
        [SerializeField]
        protected MeshRenderer[] _renderers;
        public MeshRenderer[] Renderers { get { return _renderers; } }
        [SerializeField] protected Material[] _baseMaterials;
        protected Material _newMaterial;
        //Tên của đối tượng hiển thị trên TreeView
        [SerializeField] string _objectId;
        [SerializeField] string _objectName;
        protected bool _isDeselect = false;

        public bool IsDeselect { get { return _isDeselect; } }
        [SerializeField]
        protected TreeView _treeView;

        //Tên để hiển thị trên giao diện TreeView
        public string Id => _objectId;
        public string Name => _objectName;
        //Khi được chọn trên TreeView
        public abstract void OnSelect();
        //Khi bị bỏ chọn trên TreeView
        public abstract void OnDeselect();
        public void AssignTransMat(Material newMat)
        {
            _newMaterial = newMat;
            //foreach (var renderer in Renderers)
            //{
            //    if (renderer.materials.Length > 0)
            //    {
            //        _newMaterial.mainTexture = renderer.material.mainTexture;
            //    }
            //}
        }
        private void OnDisable()
        {
            OnSelect();
        }
        //Đặt tên cho đối tượng
        public void SetName(string newName)
        {
            _objectName = newName;
        }
        public void SetData(TreeView treeView)
        {
            _treeView = treeView;
        }
        public void SetTransparent(bool isTrans)
        {

            for(int i = 0; i < Renderers.Length; i++)
            {
                var renderer = Renderers[i];
                var newMat = !isTrans ? _baseMaterials[i] : _newMaterial;
                if (renderer.sharedMaterial != newMat)
                {
                    renderer.sharedMaterial = newMat;
                }
            }    
        }
        public void SetOpacity(float newOpacity)
        {
            for (int i = 0; i < Renderers.Length; i++)
            {
                var renderer = Renderers[i];
                var color = renderer.material.color;
                color.a = newOpacity;
                renderer.material.color = color;
            }

        }    
    }
}
