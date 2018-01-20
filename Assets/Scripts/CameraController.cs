using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private GameObject[] _Players;

    private GameObject _invisCameraGo;

    private void Awake() {
        _Players = GameObject.FindGameObjectsWithTag("Player");

        _invisCameraGo = new GameObject("Camera_InvisCenter");
    }

    private void Update() {
        Vector3 _centerPos = Vector3.zero;

        for (int i = 0; i < _Players.Length; i++) {
            _centerPos += _Players[i].transform.position;
        }

        _centerPos /= _Players.Length;

        _invisCameraGo.transform.position = transform.position;
        _invisCameraGo.transform.LookAt(_centerPos);

        transform.rotation = Quaternion.Lerp(transform.rotation, _invisCameraGo.transform.rotation, Time.deltaTime * 4);
    }
}
