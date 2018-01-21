using System;
using System.Xml.Schema;
using UnityEngine;

public class CannonBallController : MonoBehaviour, IUsable {

    public enum CannonState {
        nill,
        Empty,
        Loaded
    }

    public Trouble ActiveTrouble;

    public CannonState State {
        get { return _state; }
        set {
            _state = value;
            switch (value) {
                case CannonState.Empty: {
                    ActiveTrouble = Trouble.GetReloadTrouble();
                    ActiveTrouble.FinishEvent = () => { State = CannonState.Loaded; };
                    break;
                }
                case CannonState.Loaded: {
                    ActiveTrouble = Trouble.GetFireTrouble();
                    ActiveTrouble.FinishEvent = () => {
                        FireCannon(); State = CannonState.Empty; };
                    break;
                }
            }
        }
    }
    private CannonState _state;

    private void Awake() {
        State = CannonState.Empty;
    }

    public Trouble Use(Action p_CallBack, CharacterController p_User) {
        if (p_User.CurrentEq != null)
            ActiveTrouble.Deal(p_CallBack, p_User.CurrentEq.Etype);

        return ActiveTrouble;
    }

    private void FireCannon() {
        GameObject tempCannonBall = Instantiate(GameManager.CannonBallPrefab, transform.GetChild(0).position,
            Quaternion.identity);

        tempCannonBall.GetComponent<Rigidbody>().AddForce(transform.GetChild(0).forward * 30, ForceMode.Impulse);

        GameObject tempPs = Instantiate(GameManager.CannonBallSmokePSPrefab, transform.GetChild(0).position,
            Quaternion.identity);

        Destroy(tempPs, 2f);

        State = CannonState.Empty;
    }
}
