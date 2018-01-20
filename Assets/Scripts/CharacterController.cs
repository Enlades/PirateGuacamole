using System;
using UnityEngine;

public class CharacterController : MonoBehaviour {

    // Make it 1, 2 ,3 4 from editor
    public int PlayerID;
    public float MoveSpeed;

    public delegate Trouble TriggerDelegate(Action p_Callback);

    public event TriggerDelegate TriggerEvent;

    //public Action<Action> TriggerEvent;

    public GameObject ProgressBar;

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

    private Camera _mainCam;

    private void Awake() {
        _mainCam = Camera.main;

        ProgressBar = transform.Find("ProgressBar_Border").gameObject;
    }

	private void Update () {
	    float x = Input.GetAxis("P" + PlayerID + "_Horizontal");
	    float z = Input.GetAxis("P" + PlayerID + "_Vertical");

	    transform.Translate((forwardVector * z + rightVector * x) * Time.deltaTime * MoveSpeed, Space.World);

	    if (x != 0 || z != 0) {
	        transform.LookAt(transform.position + forwardVector * z + rightVector * x);
	    }

	    if (Input.GetButtonDown("P" + PlayerID + "_Primary")) {
	        if (TriggerEvent != null) {
	            ProgressBar.SetActive(true);

	            ProgressBar.transform.LookAt(Camera.main.transform.position);
	            Vector3 scale = ProgressBar.transform.GetChild(0).localScale;
	            scale.x = 0;

	            ProgressBar.transform.GetChild(0).localScale = scale;
	        }

	        Debug.Log("P" + PlayerID + "_Primary Down");
	    }else if (Input.GetButton("P" + PlayerID + "_Primary")) {
	        if (TriggerEvent != null) {
                Trouble t = TriggerEvent.Invoke(ClearTrigger);

	            ProgressBar.transform.LookAt(Camera.main.transform.position);
	            Vector3 scale = ProgressBar.transform.GetChild(0).localScale;
	            scale.x = t.Progress / 10f * 42;

	            ProgressBar.transform.GetChild(0).localScale = scale;
	        }

	        Debug.Log("P" + PlayerID + "_Primary");
	    }else if (Input.GetButtonUp("P" + PlayerID + "_Primary")) {
	        ProgressBar.SetActive(false);
	        Debug.Log("P" + PlayerID + "_Primary Up");
        }

	    if (Input.GetButton("P" + PlayerID + "_Secondary")) {
	        Debug.Log("P" + PlayerID + "_Secondary");
        }

	    if (Input.GetButton("P" + PlayerID + "_Tertiary")) {
	        Debug.Log("P" + PlayerID + "_Tertiary");
        }
	}

    private void ClearTrigger() {
        ProgressBar.SetActive(false);
        TriggerEvent = null;
    }
}
