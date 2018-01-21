using System.Collections;
using UnityEngine;
using System;

public class TriggerAreaController : MonoBehaviour{

    // TRIGGERED !!!
    public bool Triggered = false;

    // Trouble is the class that's holding the information for the mini-game.
    public Trouble ActiveTrouble = null;

    public Equipment.EquipmentType TroubleRequsiteSet;

    // Not used yet, maybe some time.
    private bool _generatingTrouble = false;

    private void Awake() {
        DisableTrigger();

        StartCoroutine(GenereateTrouble());
    }

    // Function is called from player while the player holds the primary button.
    private Trouble ProgressTrigger(Action p_Callback, Equipment p_UsedEquipment) {
        Color transGreen = Color.green;
        transGreen.a = 0.2f;
        GetComponent<MeshRenderer>().material.color = transGreen;

        // Progress is made in Trouble class.
        if (p_UsedEquipment != null) {
            ActiveTrouble.Deal(p_Callback, p_UsedEquipment.Etype);
        }
        else {
            ActiveTrouble.Deal(p_Callback, Equipment.EquipmentType.Nill);
        }

        return ActiveTrouble;
    }

    // Simple function for debug purposes, this function is called when player exits the trigger.
    private void EnableTrigger() {
        Color transRed = Color.red;
        transRed.a = 0.2f;
        GetComponent<MeshRenderer>().material.color = transRed;

        GetComponent<Renderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }

    // This function is called when the trouble is dealt with.
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

        ActiveTrouble =
            new Trouble((int) UnityEngine.Random.Range(1, Mathf.Clamp(Time.time / 10, 1, 10))) {
                Requisite = TroubleRequsiteSet
            };

        ActiveTrouble.FinishEvent += DisableTrigger;

        _generatingTrouble = false;
    }

    // Enter me senpai
    // What ?
    // What ?
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
    
    // RETREAAT !
    // HURRY !
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
