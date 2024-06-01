using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoModelPanel : MonoBehaviour
{
    [SerializeField] RootModel _rootModel;
    [SerializeField] TMPro.TextMeshProUGUI _modelName;
    
    private void Awake()
    {
        OpenModel(0);
    }
    public void OpenModel(int id)
    {
        for (int i = 0; i < _rootModel.transform.childCount; i++)
        {
            var model = _rootModel.SelectModel(i);
            model.gameObject.SetActive(false);

        }
        var selectModel = _rootModel.SelectModel(id);
        selectModel.gameObject.SetActive(true);
        _modelName.text = selectModel.ModelName;
    }
}
