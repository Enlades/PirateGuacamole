using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class ScoreDisplay : MonoBehaviour
{
    private TextMeshPro _textMesh;
    private int _value;
    private Vector3 _defaultPos;

    private void Start()
    {
        _textMesh = GetComponent<TextMeshPro>();
        _defaultPos = transform.localPosition;
        _value = int.Parse(_textMesh.text);
    }

    public int Score
    {
        get { return _value; }
        set
        {
            if (_value != value)
            {
                _value = value;
                _textMesh.text = _value.ToString();
                transform.DOLocalMove(Vector3.up * 3, 0.05f).SetRelative().onComplete += () =>
                    transform.DOLocalMove(_defaultPos, 0.05f);
            }
        }
    }
}