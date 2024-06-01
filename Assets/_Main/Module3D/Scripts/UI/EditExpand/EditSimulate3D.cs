using Tu.Mohinh3D;
using UnityEngine;

public class EditSimulate3D : Singleton<EditSimulate3D>
{
    [SerializeField] EditDetail _editDetail;

    public EditDetail editDetail => _editDetail;

}
