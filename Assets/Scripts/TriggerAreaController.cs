using System.Collections;
using UnityEngine;
using System;

public class TriggerAreaController : MonoBehaviour{

    public bool Triggered = false;

    public Trouble ActiveTrouble = null;

    private bool _generatingTrouble = false;

    private void Awake() {
        DisableTrigger();

        StartCoroutine(GenereateTrouble());
    }

    private Trouble ProgressTrigger(Action p_Callback) {
        Color transGreen = Color.green;
        transGreen.a = 0.2f;
        GetComponent<MeshRenderer>().material.color = transGreen;

        ActiveTrouble.Deal(p_Callback);

        return ActiveTrouble;
    }

    private void EnableTrigger() {
        Color transRed = Color.red;
        transRed.a = 0.2f;
        GetComponent<MeshRenderer>().material.color = transRed;

        GetComponent<Renderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }

    private void DisableTrigger() {
        GetComponent<Renderer>().enabled = false;
        GetComponent<Collider>().enabled = false;

        StartCoroutine(GenereateTrouble());
    }

    // AND MAKE IT DOUBLE
    private IEnumerator GenereateTrouble() {
        _generatingTrouble = true;

        yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 2f));

        EnableTrigger();

        ActiveTrouble = new Trouble((int)UnityEngine.Random.Range(1, Mathf.Clamp(Time.time / 10, 1, 10)));

        ActiveTrouble.FinishEvent += DisableTrigger;

        _generatingTrouble = false;
    }

    private void OnTriggerEnter(Collider col) {
        if (!Triggered) {
            if (col.CompareTag("Player")) {
                Color transYellow = Color.yellow;
                transYellow.a = 0.2f;
                GetComponent<MeshRenderer>().material.color = transYellow;

                col.GetComponent<CharacterController>().TriggerEvent += ProgressTrigger;
            }
        }
    }

    private void OnTriggerExit(Collider col) {
        if (!Triggered) {
            if (col.CompareTag("Player")) {
                Color transRed = Color.red;
                transRed.a = 0.2f;
                GetComponent<MeshRenderer>().material.color = transRed;

                col.GetComponent<CharacterController>().TriggerEvent -= ProgressTrigger;
            }
        }
    }
}
