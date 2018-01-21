using System.Linq;
using DG.Tweening;
using UnityEngine;

public class SquidAnimator : MonoBehaviour
{
    public Transform[] Arms;

    private Vector3[] _defaultPos;
    private Vector3[] _defaultRots;

    private void Start()
    {
        _defaultPos = Arms.Select(x => x.position).ToArray();
        _defaultRots = Arms.Select(x => x.eulerAngles).ToArray();
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
        for (var i = 0; i < Arms.Length; i++)
            Flail(i);
    }

    private void Flail(int i)
    {
        var randRot = Random.insideUnitCircle * 15f;
        Arms[i].DORotate(new Vector3(randRot.x, 0f, randRot.y), 0.45f).SetRelative();
        Arms[i].DOMove(new Vector3(Random.Range(-1f, 1f), 1f, Random.Range(-1f, 1f)), 0.5f)
            .SetEase(Ease.InOutSine).SetRelative().onComplete = () => Return(i);
    }

    private void Return(int i)
    {
        Arms[i].DORotate(_defaultRots[i], 0.45f);
        Arms[i].DOMove(_defaultPos[i], 0.5f).SetEase(Ease.InOutSine)
            .onComplete = () => Flail(i);
    }
}