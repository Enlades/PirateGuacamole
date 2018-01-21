using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAreaManager : MonoBehaviour {

    public static TriggerAreaManager Instance;

    private GameObject[] _triggers;

    private Coroutine _operation;

    private void Awake() {
        _triggers = GameObject.FindGameObjectsWithTag("TriggerArea");

        Instance = this;
    }

    private void Start() {
        _operation = StartCoroutine(HandleGameLevel());

        _triggers[0].GetComponent<TriggerAreaController>().StartTrigger();
    }

    public void Stop() {
        if (_operation != null)
            StopCoroutine(_operation);
    }

    private IEnumerator HandleGameLevel() {

        yield return new WaitForSeconds(5f);

        int loopMax = 0;

        while (true) {
            yield return new WaitForSeconds(5f);

            loopMax = GameManager.GameLevel > _triggers.Length ? _triggers.Length - 1 : GameManager.GameLevel;

            for (int i = 0; i < loopMax; i++) {
                    _triggers[i].GetComponent<TriggerAreaController>().StartTrigger();
            }
        }
    }
}
