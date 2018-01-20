using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBallController : MonoBehaviour, IUsable {
    public void Use() {
        GameObject _tempCannonBall = Instantiate(GameManager.CannonBallPrefab, transform.GetChild(0).position,
            Quaternion.identity);

        _tempCannonBall.GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward * 30, ForceMode.Impulse);

        GameObject _tempPs = Instantiate(GameManager.CannonBallSmokePSPrefab, transform.GetChild(0).position,
            Quaternion.identity);

        Destroy(_tempPs, 2f);
    }
}
