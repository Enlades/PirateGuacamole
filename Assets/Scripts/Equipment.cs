using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour, IFocusable, IPickable {

    // To be used in future
    public enum EquipmentType {
        Hammer,
        FireStick
    };

    // Reference to focus arrow
    // Each equipment instantiates it's own focus arrow in Awake
    public GameObject FocusArrow;

    public EquipmentType Etype;

    private void Awake() {
        FocusArrow = Instantiate(GameManager.FocusArrowPrefab, transform.position, Quaternion.identity);
        FocusArrow.SetActive(false);
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
        GetComponent<Collider>().enabled = false;

        return this;
    }

    // Drops the equipment
    public void Drop() {
        transform.position = transform.parent.position + transform.parent.forward * 0.5f;
        transform.SetParent(null);

        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Collider>().enabled = true;
    }
}
