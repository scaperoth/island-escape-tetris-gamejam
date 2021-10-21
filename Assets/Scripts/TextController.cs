
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class TextController : MonoBehaviour
{
    [SerializeField]
    private IntVariable _score;
    private TextMeshPro _tmpro;
    [SerializeField]
    private string _textPrefix = "Score: ";

    // Start is called before the first frame update
    void Start()
    {
        _tmpro = GetComponent<TextMeshPro>();
        _score.OnValueChanged.AddListener(UpdateText);
        UpdateText(_score.Value);
    }

    void UpdateText(int newValue)
    {
        if (_textPrefix != "")
        {
            _tmpro.text = $"{_textPrefix} {newValue}";
        }
        else
        {
            _tmpro.text = $"{newValue}";
        }
    }
}
