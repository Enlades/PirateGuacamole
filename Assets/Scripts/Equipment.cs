using UnityEngine;

public class Equipment : MonoBehaviour, IFocusable, IPickable, IRockable {

    // To be used in future
    public enum EquipmentType {
        Hammer,
        FireStick,
        CannonBall
    };

    // Reference to focus arrow
    // Each equipment instantiates it's own focus arrow in Awake
    public GameObject FocusArrow;

    public EquipmentType Etype;

    private void Awake() {
        FocusArrow = Instantiate(GameManager.FocusArrowPrefab, transform.position, Quaternion.identity);
        FocusArrow.SetActive(false);
    }

    private void Update() {
        Rock();
    }

    // Makes the Focus Arrow visible
    public void ActivateFocus() {
        PlaceFocus();
        FocusArrow.SetActive(true);
        FocusArrow.GetComponent<FocusArrowController>().StartAnimation();
    }

    // Makes the Focus Arrow invisible
    public void DeactivateFocus() {
        FocusArrow.GetComponent<FocusArrowController>().StopAnimation();
        FocusArrow.SetActive(false);
    }

    // Places the Focus Arrow on top of the object, !! REVISE IT BITCH
    public void PlaceFocus() {
        FocusArrow.transform.position = transform.position + Vector3.up * 0.5f;
        FocusArrow.transform.rotation = Quaternion.LookRotation(Vector3.up, Vector3.forward);
    }

    // Picks up the equipment, basically the reference is passed to the CharacterController
    public Equipment PickUp(CharacterController p_Owner) {
        transform.position = p_Owner.transform.position + Vector3.up / 2;
        transform.SetParent(p_Owner.transform);
        GetComponent<Rigidbody>().isKinematic = true;

        // Since there is one collider for collision and the other for trigger
        // We need to disable both so that the equipment won't be triggered again.
        var colliders = GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++) {
            colliders[i].enabled = false;
        }

        DeactivateFocus();

        return this;
    }

    // Drops the equipment
    public void Drop() {
        transform.position = transform.parent.position + transform.parent.forward * 0.5f;
        transform.SetParent(null);

        GetComponent<Rigidbody>().isKinematic = false;

        var colliders = GetComponents<Collider>();
        for (int i = 0; i < colliders.Length; i++) {
            colliders[i].enabled = true;
        }
    }

    public void Rock() {
        Vector3 cameraRightVector = Camera.main.transform.right;
        cameraRightVector.y = 0;

        transform.Translate(cameraRightVector * Time.deltaTime
                                              * (Camera.main.transform.localEulerAngles.z > 180
                                                  ? 360 - Camera.main.transform.localEulerAngles.z
                                                  : Camera.main.transform.localEulerAngles.z) / 50, Space.World);
    }
}
