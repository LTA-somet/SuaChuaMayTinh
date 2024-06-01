using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class rotateButton : MonoBehaviour
{
    [SerializeField]
    private Button button; // Nút cần xoay và di chuyển
    public Button Button { get { return button; } }
    [SerializeField]
    private RectTransform targetTransform; // RectTransform cần di chuyển
    private bool isRotated = true; // Trạng thái ban đầu của nút
    public bool IsRotated { get { return isRotated; } set { isRotated = value; } }
    [SerializeField] private GameObject objectToToggle; // Đối tượng cần bật/tắt
    //private bool isObjectActive = false; // Trạng thái ban đầu của đối tượng 

    private void Start()
    {
     
        button.onClick.AddListener(OnButtonClick); // Gán sự kiện khi nút được nhấn
      // button.enabled = false;
    }

    public void OnButtonClick()
    {
        
        // Xoay nút 180 độ (theo trục Z) và di chuyển sang trái khi click
        float moveAmount = -480f;
        float rotateAmount = -180f;

        if (isRotated)
        {
            targetTransform.DOAnchorPos(new Vector2(targetTransform.anchoredPosition.x - moveAmount, targetTransform.anchoredPosition.y), 0.25f);
            button.transform.DORotate(new Vector3(0, 0, 0), 0.25f);
        }
        else
        {
            targetTransform.DOAnchorPos(new Vector2(targetTransform.anchoredPosition.x + moveAmount, targetTransform.anchoredPosition.y), 0.25f);
            button.transform.DORotate(new Vector3(0, 0, rotateAmount), 0.25f);
        }
        // Bật hoặc tắt hiện đối tượng dựa vào trạng thái hiện tại của nó
        objectToToggle.SetActive(!objectToToggle.activeInHierarchy);

        // Đảo ngược trạng thái của đối tượng (nếu đang hiển thị thì ẩn và ngược lại)
        //isObjectActive = !isObjectActive;
        // Đảo ngược trạng thái của nút (đã xoay thì chưa xoay và ngược lại)
        isRotated = !isRotated;
        Debug.Log("button");
    }
    public void HideTreeView()
    {
        Debug.Log("hide");
        // Xoay nút 180 độ (theo trục Z) và di chuyển sang trái khi click
        float moveAmount = -360f;
       

        if (isRotated)
        {
            targetTransform.DOAnchorPos(new Vector2(targetTransform.anchoredPosition.x + moveAmount, targetTransform.anchoredPosition.y), 0.25f);
            button.transform.DORotate(new Vector3(0, 0, 0), 0.25f);
        }
        else
        {
            //targetTransform.DOAnchorPos(new Vector2(targetTransform.anchoredPosition.x - moveAmount, targetTransform.anchoredPosition.y), 0.25f);
            //button.transform.DORotate(new Vector3(0, 0, rotateAmount), 0.25f);
        }
        // Bật hoặc tắt hiện đối tượng dựa vào trạng thái hiện tại của nó
        objectToToggle.SetActive(!objectToToggle.activeInHierarchy);

        // Đảo ngược trạng thái của đối tượng (nếu đang hiển thị thì ẩn và ngược lại)
        //isObjectActive = !isObjectActive;
        // Đảo ngược trạng thái của nút (đã xoay thì chưa xoay và ngược lại)
      //  isRotated = !isRotated;
    }
    private void Reset()
    {
        button = transform.parent.Find("Btn Show Hide TreeVIew").GetComponent<Button>();
        targetTransform = transform.parent.Find("Btn Show Hide TreeVIew").GetComponent<RectTransform>();
        objectToToggle = transform.Find("Background3").GetComponent<GameObject>();
    }
}
