using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Data", menuName = "FLoat Variable")]
public class FloatVariable : ScriptableObject
{
    [SerializeField]
    private float _value;
    [SerializeField]
    private float _initialValue;
    [SerializeField]
    private float _minValue;

    public float Value
    {
        get
        {
            return _value;
        }
    }
    public float InitialValue
    {
        get
        {
            return _initialValue;
        }
    }
    public float MinValue
    {
        get
        {
            return _minValue;
        }
    }

    public UnityEvent<float> OnValueChanged;

    public void AddToValue(float newValue)
    {
        _value += newValue;
        OnValueChanged.Invoke(_value);
    }

    public void SetValue(float newValue)
    {
        _value = newValue;
        OnValueChanged.Invoke(_value);
    }

    public void Reset()
    {
        _value = _initialValue;
        OnValueChanged.Invoke(_value);
    }
}
