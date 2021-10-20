using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Data", menuName = "Int Variable")]
public class IntVariable : ScriptableObject
{
    [SerializeField]
    private int _value;

    public int Value
    {
        get
        {
            return _value;
        }
    }

    public UnityEvent<int> OnValueChanged;

    public void AddToValue(int newValue)
    {
        _value += newValue;
        OnValueChanged.Invoke(_value);
    }

    public void SetValue(int newValue)
    {
        _value = newValue;
        OnValueChanged.Invoke(_value);
    }
}
