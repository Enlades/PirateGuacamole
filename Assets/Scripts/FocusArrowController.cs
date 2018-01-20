using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusArrowController : MonoBehaviour {

    // The animation is done from code, this is the reference to the coroutine of animation
    private Coroutine _operation;

    public void StartAnimation() {
        _operation = StartCoroutine(FocusArrowAnimation());
    }

    public void StopAnimation() {
        StopCoroutine(_operation);
    }

    // Basic wiggle between points
    private IEnumerator FocusArrowAnimation() {
        Vector3 _startPos = transform.position;
        Vector3 _targetPos = _startPos + Vector3.up * 0.5f;

        float _timer = 1f;

        while (true) {

            if (_timer > 0.3f) {
                transform.position = Vector3.Lerp(_targetPos, _startPos, (1f - _timer) / 3f * 10f);
            }
            else if(_timer > 0){
                transform.position = Vector3.Lerp(_startPos, _targetPos, (0.3f - _timer) * 3.33f);
            } else{
                _timer = 1f;
            }

            _timer -= Time.deltaTime;

            yield return null;
        }
    }
}
