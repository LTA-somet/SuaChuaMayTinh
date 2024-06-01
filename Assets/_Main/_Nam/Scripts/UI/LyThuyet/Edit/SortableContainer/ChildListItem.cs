using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
static class NumbersToRoman
{
    public static string IntToRoman(int num)
    {
        string romanResult = "";
        Dictionary<string, int> romanNumbersDictionary = new() {
            {
                "I",
                1
            }, {
                "IV",
                4
            }, {
                "V",
                5
            }, {
                "IX",
                9
            }, {
                "X",
                10
            }, {
                "XL",
                40
            }, {
                "L",
                50
            }, {
                "XC",
                90
            }, {
                "C",
                100
            }, {
                "CD",
                400
            }, {
                "D",
                500
            }, {
                "CM",
                900
            }, {
                "M",
                1000
            }
        };
        foreach (var item in romanNumbersDictionary.Reverse())
        {
            if (num <= 0) break;
            while (num >= item.Value)
            {
                romanResult += item.Key;
                num -= item.Value;
            }
        }
        return romanResult;
    }
}
public class ChildListItem : MonoBehaviour
{
    [SerializeField] TMPro.TMP_InputField _index;
    [SerializeField] TMPro.TMP_InputField _content;
    [SerializeField] Button _delBtn;
    [SerializeField] DragOrderObject _dragOrderObject;
    [SerializeField] bool _usingRomanNumber;

    object _genericData = null;
    public string Content => _content.text;
    public object GenericData
    {
        get => _genericData;
        set => _genericData = value;
    }
    private void Awake()
    {
        _dragOrderObject.onDrop += UpdateAllSiblings;
        _delBtn.onClick.AddListener(Delete);
    }
    private void OnEnable()
    {
        UpdateIndex();
    }
    public void SetContent(string content)
    {
        _content.text = content;
    }
    public void UpdateIndex()
    {
        if(_usingRomanNumber)
        {
            _index.text = NumbersToRoman.IntToRoman(transform.GetSiblingIndex() + 1);
        }    
        else
        {
            _index.text = (transform.GetSiblingIndex() + 1).ToString();
        }    
    }
    private void Delete()
    {
        var papa = transform.parent;
        DestroyImmediate(gameObject);
        for (int i = 0; i < papa.childCount; i++)
        {
            if (papa.GetChild(i).TryGetComponent(out ChildListItem kid))
            {
                kid.UpdateIndex();
            }
        }
    }
    private void UpdateAllSiblings()
    {
        var papa = transform.parent;
        for (int i = 0; i < papa.childCount; i++)
        {
            if(papa.GetChild(i).TryGetComponent(out ChildListItem kid))
            {
                kid.UpdateIndex();
            }
        }
    }
}
