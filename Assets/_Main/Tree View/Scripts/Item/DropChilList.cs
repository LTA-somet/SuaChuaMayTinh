using Nam.TreeView;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class DropChilList : MonoBehaviour
{
    [SerializeField] private Button _dropButton;
    [SerializeField] private Button _button;
    [SerializeField] private Sprite _image;
    [SerializeField] private GameObject itemPartPrefab;
    [SerializeField] private List<GameObject> menuList;
    public List<GameObject> MenuList => menuList;
    [SerializeField] private MenuSearch _menuSearch;
    public MenuSearch MenuSearch => _menuSearch;
    private Sprite _originImage;
    private bool isMenuVisible = false;
    private Muc3ChildContent _childContent;

    private Dictionary<GameObject, List<GameObject>> parentToChildren = new Dictionary<GameObject, List<GameObject>>();


    void Start()
    {
        _originImage = _dropButton.image.sprite;
        _button.onClick.AddListener(ToggleMenu);

    }
    public void setData(List<Muc3Data> datas, Transform menuChildParent, int indexC, int indexF, TreeView treeView, bool is3D)
    {
        //if (treeView.LisActivePart.Count > 0)
        //{
        //    for (int i = 0; i < treeView.LisActivePart.Count; i++)
        //    {
        //        treeView.LisActivePart[i].SetActive(false);
        //        treeView.ListUnactivePart.Add(treeView.LisActivePart[i]);
        //        treeView.LisActivePart[i].transform.SetParent(null);
        //    }
        //    treeView.LisActivePart.Clear();
        //    menuList.Clear();
        //}

        for (int j = 0; j < datas[indexF].ChildrenContent[indexC].ChildrenContent.Count; j++)
        {

            GameObject childMenuObject = null;
            if (treeView.ListUnactivePart.Count <= 0)
            {
                childMenuObject = Instantiate(itemPartPrefab, menuChildParent);

            }
            else
            {
                childMenuObject = treeView.ListUnactivePart[0];
                treeView.ListUnactivePart.Remove(childMenuObject);
                childMenuObject.transform.SetParent(menuChildParent);
            }
            treeView.LisActivePart.Add(childMenuObject);
            TMPro.TextMeshProUGUI childMenuText = childMenuObject.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();
            childMenuText.text = "" + datas[indexF].ChildrenContent[indexC].ChildrenContent[j].Title.ToString();
            childMenuObject.GetComponent<Muc4Content>().SetData(datas[indexF].ChildrenContent[indexC], j, indexF, is3D);
            childMenuObject.SetActive(false);
            treeView.MenuSearch.GetDataPart(childMenuObject);
            menuList.Add(childMenuObject);


        }
    }

    private void ToggleMenu()
    {
        isMenuVisible = !isMenuVisible;
        if (isMenuVisible)
        {
            _dropButton.image.sprite = _image;
        }
        else
        {
            _dropButton.image.sprite = _originImage;
        }
        foreach (var item in menuList)
        {
            item.SetActive(!item.activeInHierarchy);

        }
    }
}


