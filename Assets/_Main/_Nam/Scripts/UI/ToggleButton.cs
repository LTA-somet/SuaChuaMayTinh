using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class ToggleButton : MonoBehaviour
{
    [SerializeField] Sprite _active;
    [SerializeField] Sprite _unactive;
    [SerializeField] Button _button;
    [SerializeField] bool _on;
    public UnityEvent<bool> onToggle;
    // Start is called before the first frame update
    void Start()
    {
        if (!_button && TryGetComponent(out Button button))
        {
            _button = button;
        }
        _button.onClick.AddListener(Toggle);
        SetOn(_on);
    }

    void Toggle()
    {
        SetOn(!_on);
    }
    public void SetOn(bool newOn)
    {
        _on = newOn;
        _button.image.sprite = _on ? _active : _unactive;
        onToggle.Invoke(_on);
    }
}
