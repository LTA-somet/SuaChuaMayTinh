using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditTheory : Singleton<EditTheory>
{
    [SerializeField] EditMuc4 _muc4;
    [SerializeField] EditMuc5 _muc5;
    [SerializeField] EditMuc3 _muc3;

    public EditMuc3 Muc3 => _muc3;
    public EditMuc4 Muc4 => _muc4;
    public EditMuc5 Muc5 => _muc5;
}
