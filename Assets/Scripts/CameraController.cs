using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraController : MonoBehaviour {

    // Array of players which is being used for Camera LookAt()
    private GameObject[] _players;

    // The GameObject which actually LooksAt() the center point of players
    // The Camera Lerps it's rotation to this gameObject
    // The position of this gameObject is updated to the Camera's in Update()
    private GameObject _invisCameraGo;

    [Range(-1f,1f)]
    public float CameraRollStrength;
    [Range(-1f, 1f)]
    public float CameraZoomStrength;
    public float MaxStrength = 4;

    private void Awake() {
        _players = GameObject.FindGameObjectsWithTag("Player");

        _invisCameraGo = new GameObject("Camera_InvisCenter");
        _invisCameraGo.transform.position = transform.position;
    
        StartCoroutine(CameraShake());
    }

    private void Update() {
        Vector3 _centerPos = Vector3.zero;

        // CALCULATE IT
        for (int i = 0; i < _players.Length; i++) {
            _centerPos += _players[i].transform.position;
        }

        _centerPos /= _players.Length;

        // Smoothness overload
        _invisCameraGo.transform.rotation = Quaternion.LookRotation(_centerPos - _invisCameraGo.transform.position, _invisCameraGo.transform.up);

        // So simple
        transform.rotation = Quaternion.Lerp(transform.rotation, _invisCameraGo.transform.rotation, Time.deltaTime * 4);
        transform.position = Vector3.Lerp(transform.position, _invisCameraGo.transform.position, Time.deltaTime * 4);
    }

    // Oh boy here i go killing again
    private IEnumerator CameraShake() {

        // These variables are used for roll stuff, rotation around z axis
        // Rolls are like turn based, first clockwise then counter-clockwise
        // If the Camera rotates toom much to either direction, it's corrected.
        float rollDuration = Random.Range(0.5f, 1f);
        // This one is used in Lerp function
        float rollDurationMax = rollDuration;
        // The random
        float rollAngle = Random.Range(5f, 10f) * GetRollAmplify();
        // Direction
        int yawDirection = 1;

        // These variables are used for zoom stuff, translate on z axis.
        // Again it's turn based, first forwards then backwards.
        // Again, if the movement in any direction is too much, it's corrected.
        float zoomDuration = Random.Range(0.5f, 1f);
        // Used in Lerp function
        float zoomDurationMax = zoomDuration;
        // THE RANDOM
        float zoomAmount = Random.Range(0.1f, 0.3f) * GetZoomAplify();
        // Onward stallion
        int zoomDirection = 1;

        // These structs are used in Quaternion.Lerp() function
        // They are basically created with the values above.
        Quaternion startQuaternion = _invisCameraGo.transform.rotation;
        Quaternion targetQuaternion = startQuaternion;
        targetQuaternion *= Quaternion.Euler(Vector3.forward * rollAngle * yawDirection);

        // Same stuff for position, movement is made in forward flat direction. There's no y movement.
        Vector3 startPosition = _invisCameraGo.transform.position;
        Vector3 flatForward = _invisCameraGo.transform.forward;
        flatForward.y = 0;
        Vector3 targetPosition = startPosition + flatForward * zoomDirection * zoomAmount;
        Vector3 firstStartPosition = _invisCameraGo.transform.position;
        
        while (true) {

            // Since we're in an endless loop, here's a repeating loop. If the timer reaches zero, randomize the values and start again.
            if (rollDuration > 0f) {
                // Since we're in the loop, keep lerping that shit.
                _invisCameraGo.transform.rotation =
                    Quaternion.Lerp(startQuaternion, targetQuaternion, (rollDurationMax - rollDuration) * 1 / rollDurationMax);
            }
            else {
                // Oh boi, here i go random again.
                yawDirection *= -1;
                rollDuration = Random.Range(0.5f, 1f);
                rollDurationMax = rollDuration;
                rollAngle = Random.Range(5f, 10f) * GetRollAmplify();

                // Correction part for over-rotation. Since the localEulerAngles has a range of 0-360, unlike the editor which has -inf,+inf
                // I had to use some wierd voodoo stuff to understand to which direction we overshoot.
                if (_invisCameraGo.transform.localEulerAngles.z < 330 && _invisCameraGo.transform.localEulerAngles.z > 270) {
                    yawDirection = 1;
                    rollAngle = 20f;
                    rollDuration = 0.5f;
                } else if (_invisCameraGo.transform.localEulerAngles.z > 30 && _invisCameraGo.transform.localEulerAngles.z < 90) {
                    yawDirection = -1;
                    rollAngle = 20f;
                    rollDuration = 0.5f;
                }

                // Ready, set, go.
                startQuaternion = _invisCameraGo.transform.rotation;
                targetQuaternion = startQuaternion;
                targetQuaternion *= Quaternion.Euler(Vector3.forward * rollAngle * yawDirection);
            }

            // Same stuff for position.
            // Seriously
            // Position instead of Quaternion
            // STOP READING
            // Ok you're gay now.
            if (zoomDuration > 0f) {
                _invisCameraGo.transform.position =
                    Vector3.Lerp(startPosition, targetPosition, (zoomDurationMax - zoomDuration) * 1 / zoomDurationMax);
            } else {
                zoomDirection *= -1;
                zoomDuration = Random.Range(2f, 4f);
                zoomDurationMax = rollDuration;
                zoomAmount = Random.Range(0.1f, 0.3f) * GetZoomAplify();

                if (Vector3.Distance(firstStartPosition, _invisCameraGo.transform.position) > 5f) {
                    if ((_invisCameraGo.transform.position - firstStartPosition).z > 0) {
                        zoomDirection = -1;
                    }
                    else {
                        zoomDirection = 1;
                    }
                }

                startPosition = _invisCameraGo.transform.position;
                flatForward = _invisCameraGo.transform.forward;
                flatForward.y = 0;
                targetPosition = startPosition + flatForward * zoomDirection * zoomAmount;
            }

            rollDuration -= Time.deltaTime * Random.Range(0.9f, 1.2f) * 1.3f;
            zoomDuration -= Time.deltaTime * Random.Range(0.9f, 1.2f) * 1.3f;

            // F U
            yield return null;
        }
    }

    private float GetRollAmplify() {
        if (Math.Abs(CameraRollStrength) < 0.01f) {
            return 1;
        }
        else {
            return MaxStrength * (CameraRollStrength > 0 ? CameraRollStrength : 1 / CameraRollStrength);
        }
    }

    private float GetZoomAplify() {
        if (Math.Abs(CameraZoomStrength) < 0.01f) {
            return 1;
        } else {
            return MaxStrength * CameraZoomStrength;
        }
    }
}
