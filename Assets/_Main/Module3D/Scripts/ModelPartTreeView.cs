
using System.Collections;
using System.Collections.Generic;
using TDLN.CameraControllers;
using Tu.Animation.ThaoLap;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace PCmodel.TreeView.Model
{
    //Lớp Component cho một bộ phận hiển thị và tương tác trên giao diện TreeView
    public class ModelPartTreeView : TreeViewObject
    {
        private void Start()
        {
            AddComponent();

            if (_renderers.Length < 1)
            {
                _renderers = GetComponentsInChildren<MeshRenderer>();
                _baseMaterials = new Material[_renderers.Length];
                for (int i = 0; i < _renderers.Length; i++)
                {
                    _baseMaterials[i] = _renderers[i].material;
                }
            }
            if (_renderers2.Length < 1)
            {
                _renderers2 = GetComponentsInChildren<SkinnedMeshRenderer>();
                _baseMaterials2 = new Material[_renderers2.Length];


                for (int i = 0; i < _renderers2.Length; i++)
                {
                    _baseMaterials2[i] = _renderers2[i].material;
                }
            }

        }
        public override void OnDeselect()
        {
            if (_renderers != null)
            //_renderer.enabled = false;
            {
                if (_treeView.Slider.value < 1)
                {
                    for (int i = 0; i < _renderers.Length; i++)
                    {
                        _renderers[i].material = _newMaterial;
                    }
                    SetOpacity(_treeView.Slider.value);
                }
                _isDeselect = true;
            }
            if (_renderers2 != null)
            //_renderer.enabled = false;
            {
                if (_treeView.Slider.value < 1)
                {
                    for (int i = 0; i < _renderers2.Length; i++)
                    {
                        _renderers2[i].material = _newMaterial;
                    }
                    SetOpacity(_treeView.Slider.value);
                }
                _isDeselect = true;
            }
        }

        public override void OnSelect()
        {
            if (_renderers != null)
            //_renderer.enabled = true;
            {
                if (GameManager.Instance != null && GameManager.Instance.OrbitCamera != null)
                    GameManager.Instance.OrbitCamera.GetComponent<CameraController>().SetObject(_renderers[_renderers.Length / 2].gameObject.transform);
                for (int i = 0; i < _renderers.Length; i++)
                {
                    _renderers[i].material = _baseMaterials[i];
                }

                _isDeselect = false;
            }
            if (_renderers2 != null)
            //_renderer.enabled = true;
            {
                for (int i = 0; i < _renderers2.Length; i++)
                {
                    _renderers2[i].material = _baseMaterials2[i];
                }

                _isDeselect = false;
            }
        }
        private void AddComponent()
        {
            // Check if the component is already added to the gameobject
            if (gameObject.GetComponent<MeshRenderer>() == null)
            {
                // Add the component to the gameobject
                gameObject.AddComponent<MeshRenderer>();
            }
        }
        private void Reset()
        {
            AddComponent();
            _renderers = GetComponentsInChildren<MeshRenderer>();
            _renderers2 = GetComponentsInChildren<SkinnedMeshRenderer>();
            _baseMaterials = new Material[_renderers.Length];
            _baseMaterials2 = new Material[_renderers2.Length];
            for (int i = 0; i < _renderers.Length; i++)
            {
                _baseMaterials[i] = _renderers[i].sharedMaterial;
            }
            for (int i = 0; i < _renderers2.Length; i++)
            {
                _baseMaterials2[i] = _renderers2[i].sharedMaterial;
            }
        }
    }
}

