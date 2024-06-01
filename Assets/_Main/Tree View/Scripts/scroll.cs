using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class scroll : MonoBehaviour
{
    [SerializeField]
    private RectTransform menu1,menu2,backgroundMn1,backgroundMn2,btHome,btUser;
    private bool isPanelLeft = true; // Trạng thái ban đầu của menu là sang trái
    public bool IsPanelLeft { get { return isPanelLeft; } set { isPanelLeft = value; } }
    private Vector2 treeViewPosition;
    private Vector2 menuOriginalPosition1; 
    private Vector2 bgOriginalPosition1;
    private Vector2 menuOriginalPosition2; 
    private Vector2 bgOriginalPosition2;
    private Vector2 btHomePosition;
    private Vector2 btUserPosition;

    private void Start()
    {
      
        menuOriginalPosition1 = menu1.anchoredPosition;
        bgOriginalPosition1 = backgroundMn1.anchoredPosition;
        menuOriginalPosition2 = menu2.anchoredPosition;
        bgOriginalPosition2 = backgroundMn2.anchoredPosition;
        btHomePosition=btHome.anchoredPosition;
        btUserPosition=btUser.anchoredPosition;
    }

    public void TogglePanelPosition()
    {
        float moveAmount1 = 490f;
      
        if (isPanelLeft)
        {
          //  treeView.DOAnchorPos(new Vector2(treeView.anchoredPosition.x + moveAmount1, treeView.anchoredPosition.y), 0.25f);
             menu1.DOAnchorPos(new Vector2(menu1.anchoredPosition.x + moveAmount1, menu1.anchoredPosition.y), 0.25f);
             backgroundMn1.DOAnchorPos(new Vector2(backgroundMn1.anchoredPosition.x + moveAmount1, backgroundMn1.anchoredPosition.y), 0.25f);
              menu2.DOAnchorPos(new Vector2(menu2.anchoredPosition.x + moveAmount1, menu2.anchoredPosition.y), 0.25f);
             backgroundMn2.DOAnchorPos(new Vector2(backgroundMn2.anchoredPosition.x + moveAmount1, backgroundMn2.anchoredPosition.y), 0.25f);
             btHome.DOAnchorPos(new Vector2(btHome.anchoredPosition.x + moveAmount1, btHome.anchoredPosition.y), 0.25f);
              btUser.DOAnchorPos(new Vector2(btUser.anchoredPosition.x + moveAmount1, btUser.anchoredPosition.y), 0.25f);
        }
        else
        {
           // treeView.DOAnchorPos(treeViewPosition, 0.25f);
            menu1.DOAnchorPos(menuOriginalPosition1, 0.25f);
            backgroundMn1.DOAnchorPos(bgOriginalPosition1, 0.25f);

           menu2.DOAnchorPos(menuOriginalPosition2, 0.25f);
           backgroundMn2.DOAnchorPos(bgOriginalPosition2, 0.25f);

           btHome.DOAnchorPos(btHomePosition, 0.25f);
           btUser.DOAnchorPos(btUserPosition, 0.25f);
        }

        isPanelLeft = !isPanelLeft; // Đảo ngược trạng thái của menu sau mỗi lần nhấn
    }
    private void Reset()
    {
        menu1 = transform.Find("List Group Weapon").GetComponent<RectTransform>();
        menu2 = transform.Find("List Weapon ").GetComponent<RectTransform>();
        backgroundMn1 = transform.Find("Background 1w").GetComponent<RectTransform>();
        backgroundMn2 = transform.Find("Background 2").GetComponent<RectTransform>();
        btHome = transform.parent.Find("Btn Home").GetComponent<RectTransform>();
       btUser= transform.parent.Find("Btn Login").GetComponent<RectTransform>();

    }
}
