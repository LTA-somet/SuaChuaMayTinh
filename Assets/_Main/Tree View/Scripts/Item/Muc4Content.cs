using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Tu.Mohinh3D;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Muc4Content : MonoBehaviour
{

    [SerializeField] Button _button;
    Muc3ChildData _data;
    public Muc3ChildData Data => _data;
    Muc4UI _muc4UI;
    [SerializeField]
    private TextMeshProUGUI textPart;
    public TextMeshProUGUI TextPart => textPart;
    int _indexMuc4 = 0;
    int _indexMuc3 = 0;
    bool _is3D;
    private void Awake()
    {
        _button.onClick.AddListener(OpenMuc4);
    }
    public void SetData(Muc3ChildData data, int indexMuc4, int indexMuc3, bool is3D)
    {
        _data = data;
        _indexMuc4 = indexMuc4;
        _indexMuc3 = indexMuc3;
        _is3D = is3D;
    }
    public void OpenMuc4()
    {
        if (!_is3D)
        {
            MainUI.Instance.Theory.GetComponent<TheoryUI>().Muc5UI.gameObject.SetActive(false);
            MainUI.Instance.Theory.GetComponent<TheoryUI>().Muc3UI.OpenSlide(_indexMuc3);
            this.DispatchEvent(EventKey.REQUEST_OPEN_THEORY_Level_4, _data);
            this.DispatchEvent(EventKey.REQUEST_OPEN_THEORY_Level_4_Index, _indexMuc4);
        }
        else
        {
            Muc3ChildData muc3ChildData = _data;
            Muc4Data muc4Data = muc3ChildData.ChildrenContent[0];
            GameManager gameManager = GameManager.Instance;
            for (int i = 0; i < gameManager.Rigs.Count; i++)
            {
                gameManager.Rigs[i].gameObject.SetActive(false);
            }
            gameManager.Rigs[int.Parse(muc4Data.uID)].gameObject.SetActive(true);

            //MainUI.Instance.Simulation3D.GetComponent<Simulation3DUI>().
            this.DispatchEvent(EventKey.REQUEST_OPEN_MODEL_Slide3D1, _data.ChildrenContent[0].Title);
            this.DispatchEvent(EventKey.REQUEST_OPEN_EXPAND_Slide3D, _data.ChildrenContent[0].ChildrenData);
            this.DispatchEvent(EventKey.REQUEST_OPEN_MODEL_Slide3D, _data);

        }

    }
}
