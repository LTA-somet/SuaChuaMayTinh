using Nam.TreeView;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuSearch : MonoBehaviour
{
    [SerializeField]
    TMP_InputField searchInput;
    List<GameObject> ListPart = new List<GameObject>();
    List<GameObject> ListLesson = new List<GameObject>();

    private void Start()
    {
        searchInput.onValueChanged.AddListener(SearchMenu);
    }
    public void GetDataPart(GameObject gameObject)
    {
        ListPart.Add(gameObject);


    }
    public void GetDataLesson(GameObject gameObject)
    {
        ListLesson.Add(gameObject);

    }
    public void ResetLis()
    {
        ListPart.Clear();
        ListLesson.Clear();


    }
    private void SearchMenu(string searchText)
    {
        foreach (var child in ListLesson)
        {
            child.SetActive(false);
        }
        if (searchText == string.Empty)
        {
            foreach (var child in ListLesson)
            {
                child.SetActive(true);
            }
            foreach (var child in ListPart)
            {
                child.SetActive(false);
            }
        }
        else
        {
            foreach (var child in ListPart)
            {
                string itemName;
                if (child.transform.GetComponent<Muc4Content>() != null)
                {
                    itemName = child.transform.GetComponent<Muc4Content>().TextPart.text;
                }
                else
                {
                    itemName = child.name;
                }

                if (itemName.Contains(searchText))
                {
                    child.SetActive(true);
                }
                else
                {
                    child.SetActive(false);
                }
            }



        }
    }
}
