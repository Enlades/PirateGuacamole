using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    // Array of players which is being used for Camera LookAt()
    private GameObject[] _Players;

    // The GameObject which actually LooksAt() the center point of players
    // The Camera Lerps it's rotation to this gameObject
    // The position of this gameObject is updated to the Camera's in Update()
    private GameObject _invisCameraGo;

    private void Awake() {
        _Players = GameObject.FindGameObjectsWithTag("Player");

        _invisCameraGo = new GameObject("Camera_InvisCenter");
    }

    private void Update() {
        Vector3 _centerPos = Vector3.zero;

        // CALCULATE IT
        for (int i = 0; i < _Players.Length; i++) {
            _centerPos += _Players[i].transform.position;
        }

        _centerPos /= _Players.Length;

        // Smoothness overload
        _invisCameraGo.transform.position = transform.position;
        _invisCameraGo.transform.LookAt(_centerPos);

        transform.rotation = Quaternion.Lerp(transform.rotation, _invisCameraGo.transform.rotation, Time.deltaTime * 4);
    }
}
