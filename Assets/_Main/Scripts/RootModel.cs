using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootModel : MonoBehaviour
{
    public DemoModel SelectModel(int id)
    {
        var model = transform.GetChild(id).GetComponent<DemoModel>();
        return model;
    }
}
