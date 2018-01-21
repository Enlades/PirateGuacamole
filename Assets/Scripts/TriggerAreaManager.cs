using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAreaManager : MonoBehaviour {

    private GameObject[] _triggers;

    private void Awake() {
        _triggers = GameObject.FindGameObjectsWithTag("TriggerArea");
    }

    private void Start() {
        StartCoroutine(HandleGameLevel());

        _triggers[0].GetComponent<TriggerAreaController>().StartTrigger();
    }

    private IEnumerator HandleGameLevel() {

        yield return new WaitForSeconds(5f);

        while (true) {
            yield return new WaitForSeconds(5f);

            for (int i = 0; i < GameManager.GameLevel; i++) {
                _triggers[i].GetComponent<TriggerAreaController>().StartTrigger();
            }
        }
    }
}
