using System.Collections;
using UnityEngine;
using System;

public class TriggerAreaController : MonoBehaviour {

    // TRIGGERED !!!
    public bool Triggered = false;

    public bool Started;

    // Trouble is the class that's holding the information for the mini-game.
    public Trouble ActiveTrouble = null;

    public Equipment.EquipmentType TroubleRequsiteSet;

    public GameObject BrokenDeckPart;

    private GameObject _lightning;

    private Collider _collider;

    // Not used yet, maybe some time.
    private bool _generatingTrouble = false;

    private void Awake() {

        _lightning = transform.GetChild(0).gameObject;

        _lightning.SetActive(false);

        _collider = GetComponent<Collider>();

        _collider.enabled = false;

        Started = false;
    }

    public void StartTrigger() {
        if (!Started)
            DisableTrigger();
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

        //GetComponent<Renderer>().enabled = true;
        _collider.enabled = true;
    }

    // This function is called when the trouble is dealt with.
    private void DisableTrigger() {
        Started = true;
        //GetComponent<Renderer>().enabled = false;
        _collider.enabled = false;

        if (BrokenDeckPart != null)
            Destroy(BrokenDeckPart);

        StartCoroutine(GenereateTrouble());
    }

    // AND MAKE IT DOUBLE
    private IEnumerator GenereateTrouble() {
        _generatingTrouble = true;

        float randomWait = UnityEngine.Random.Range(5f, 10f);

        yield return new WaitForSeconds(randomWait - 0.7f);

        _lightning.SetActive(true);

        yield return new WaitForSeconds(0.7f);

        EnableTrigger();

        GameManager.SplashPlanks(transform.position);

        BrokenDeckPart = Instantiate(GameManager.BrokenDeckPrefab, transform.position + Vector3.down * 0.4f, Quaternion.identity);
        BrokenDeckPart.transform.rotation = Quaternion.LookRotation(Vector3.up, Vector3.forward);

        ActiveTrouble =
            new Trouble(Mathf.Clamp(GameManager.GameLevel / 2, 1, 10)) {
                Requisite = TroubleRequsiteSet
            };

        ActiveTrouble.FinishEvent += DisableTrigger;

        _generatingTrouble = false;

        _lightning.SetActive(false);
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

    private void Update()
    {
        if(_collider.enabled)
            GameManager.WaterLevel += Time.deltaTime * 0.01f;
    }
}
