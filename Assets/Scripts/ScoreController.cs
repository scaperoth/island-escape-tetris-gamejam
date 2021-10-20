
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class ScoreController : MonoBehaviour
{
    [SerializeField]
    private IntVariable _score;
    private TextMeshPro _tmpro;
    [SerializeField]
    private string _scorePrefix = "Score: ";

    // Start is called before the first frame update
    void Start()
    {
        _tmpro = GetComponent<TextMeshPro>();
        _score.OnValueChanged.AddListener(UpdateScoreText);
        UpdateScoreText(_score.Value);
    }

    void UpdateScoreText(int newValue)
    {
        if (_scorePrefix != "")
        {
            _tmpro.text = $"{_scorePrefix} {newValue}";
        }
        else
        {
            _tmpro.text = $"{newValue}";
        }
    }
}
