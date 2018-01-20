﻿using System;
using UnityEngine;

public class CharacterController : MonoBehaviour {

    // Make it 1, 2 ,3 4 from editor
    public int PlayerID;
    // Hi Ho Silver ! Awaaaay !
    public float MoveSpeed;

    // Custom delegate/event couple which is used during trigger stuff.
    public delegate Trouble TriggerDelegate(Action p_Callback);
    public event TriggerDelegate TriggerEvent;

    // Ref to progress bar. Activate Deactivate
    public GameObject ProgressBar;

    // Current equipment
    public Equipment CurrentEq;
    // Current station
    public Station CurrentStation;

    // Movement is based on camera, theese two return the necessary movement vectors
    private Vector3 forwardVector {
        get {
            Vector3 result = _mainCam.transform.forward;
            result.y = 0;
            return result;
        }
    }
    private Vector3 rightVector {
        get { return _mainCam.transform.right; }
    }

    // Render me like your french girls
    private Camera _mainCam;

    private void Awake() {
        _mainCam = Camera.main;

        ProgressBar = transform.Find("ProgressBar_Border").gameObject;
    }

	private void Update () {
	    float x = Input.GetAxis("P" + PlayerID + "_Horizontal");
	    float z = Input.GetAxis("P" + PlayerID + "_Vertical");

	    if (CurrentStation == null) {
	        transform.Translate((forwardVector * z + rightVector * x) * Time.deltaTime * MoveSpeed, Space.World);

	        // Simple yet gud, looks at the direction player is moving.
	        if (Math.Abs(x) > 0.1f || Math.Abs(z) > 0.1f) {
	            transform.LookAt(transform.position + forwardVector * z + rightVector * x);
	        }
        }

        // These regions have the if statements for the button events.
        // Seperated by regions, they are.

        #region Primary Key

	    // PrimaryButton Events Down, Hold, Up
	    if (Input.GetButtonDown("P" + PlayerID + "_Primary")) {
	        if (TriggerEvent != null) {
	            ProgressBar.SetActive(true);

	            ProgressBar.transform.LookAt(Camera.main.transform.position);
	            Vector3 scale = ProgressBar.transform.GetChild(0).localScale;
	            scale.x = 0;

	            ProgressBar.transform.GetChild(0).localScale = scale;
	        }

	        Debug.Log("P" + PlayerID + "_Primary Down");
	    } else if (Input.GetButton("P" + PlayerID + "_Primary")) {
	        if (TriggerEvent != null) {
	            Trouble t = TriggerEvent.Invoke(ClearTrigger);

	            // Trouble is the class that holds the information for the current action.
	            // Using progress, scale the progress bar.
	            ProgressBar.transform.LookAt(Camera.main.transform.position);
	            Vector3 scale = ProgressBar.transform.GetChild(0).localScale;
	            scale.x = t.Progress / 10f * 42;

	            ProgressBar.transform.GetChild(0).localScale = scale;
	        }

	        Debug.Log("P" + PlayerID + "_Primary");
	    } else if (Input.GetButtonUp("P" + PlayerID + "_Primary")) {
	        ProgressBar.SetActive(false);
	        Debug.Log("P" + PlayerID + "_Primary Up");
	    }

        #endregion

        #region Secondary Key

	    if (Input.GetButtonDown("P" + PlayerID + "_Secondary")) {

	        if (CurrentStation != null) {
	            CurrentStation.UnMan();
	            CurrentStation = null;

	            return;
	        }

	        if (CurrentEq != null) {
	            CurrentEq.Drop();
	            CurrentEq = null;
	        }

	        Debug.Log("P" + PlayerID + "_Secondary Down");
	    } else if (Input.GetButton("P" + PlayerID + "_Secondary")) {
	        Debug.Log("P" + PlayerID + "_Secondary");
	    } else if (Input.GetButtonUp("P" + PlayerID + "_Secondary")) {
	        Debug.Log("P" + PlayerID + "_Secondary Up");
	    }

        #endregion

        #region Tertiary Key

	    if (Input.GetButton("P" + PlayerID + "_Tertiary")) {
	        Debug.Log("P" + PlayerID + "_Tertiary");
	    }

        #endregion
    }

    // Callback function for Trigger.
    private void ClearTrigger() {
        ProgressBar.SetActive(false);
        TriggerEvent = null;
    }

    // Used for Equipment Focus Arrow activation.
    private void OnTriggerEnter(Collider col) {
        if (col.CompareTag("Equipment")) {
            col.GetComponent<IFocusable>().ActivateFocus();
        }
    }

    // Used for picking up equipment.
    // Used for manning stations.
    // !! CHECK !! They do check if a previous equipment or station is present.
    private void OnTriggerStay(Collider col) {
        if (col.CompareTag("Equipment")) {
            if (CurrentEq == null && Input.GetButtonDown("P" + PlayerID + "_Primary")) {
                CurrentEq = col.GetComponent<IPickable>().PickUp(this);
            }
        }else if (col.CompareTag("Station")) {
            if (CurrentStation == null && Input.GetButtonDown("P" + PlayerID + "_Primary")) {
                transform.position = col.transform.GetChild(0).position;
                CurrentStation = col.GetComponent<Station>().Man(this);

                transform.LookAt(CurrentStation.transform.GetChild(1).position);
            }
        }
    }

    // Used for Equipment Focus Arrow deactivation.
    private void OnTriggerExit(Collider col) {
        if (col.CompareTag("Equipment")) {
            col.GetComponent<IFocusable>().DeactivateFocus();
        }
    }
}
