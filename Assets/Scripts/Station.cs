using UnityEngine;

public class Station : MonoBehaviour, IManable {
    // After this point, player is using the station, handle it !
    public Station Man(CharacterController p_Owner) {
        Debug.Log("Station manned.");

        return this;
    }

    // Right now, secondary button of player UnMans the station, so there's that.
    public void UnMan() {
        Debug.Log("Station unmanned.");
    }
}
