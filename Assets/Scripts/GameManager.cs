using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    // Prefab for FocusArrow, comment for the sake of comment
    public static GameObject FocusArrowPrefab;

    private void Awake() {
        FocusArrowPrefab = Resources.Load("Prefabs/FocusArrow")as GameObject;
    }

    // Why not right ?
    private void OnGUI() {
        GUILayout.Label("P1: W A S D :: E");
        GUILayout.Label("P2: Up Left Down Right :: Keypad Enter");
    }
}
