using UnityEngine;

public class ExpandableContainer : MonoBehaviour
{
    static ExpandableContainer _instance;
    public static ExpandableContainer Instance 
    {
        get
        {
            if(!_instance)
            {
                _instance = Resources.Load<ExpandableContainer>("UI/FullScreenContainer");
            }
            return _instance;
        }
    }
    [SerializeField] RectTransform _container;
    private Vector2 _baseAnchorMin;
    private Vector2 _baseAnchorMax;
    private Vector2 _basePivot;
    private Vector2 _baseSizeDelta;

    private void Awake()
    {
        if(!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _baseAnchorMin = _container.anchorMin;
        _baseAnchorMax = _container.anchorMax;
        _basePivot = _container.pivot;
        _baseSizeDelta = _container.sizeDelta;
        Debug.LogError(_baseSizeDelta);
    }

    public void Expand(RectTransform rt)
    {
        rt.SetParent(_container);
        rt.anchorMin = _baseAnchorMin;
        rt.anchorMax = _baseAnchorMax;
        rt.pivot = _basePivot;
        rt.sizeDelta = _baseSizeDelta;
        rt.anchoredPosition = Vector2.zero;
    }    
}
