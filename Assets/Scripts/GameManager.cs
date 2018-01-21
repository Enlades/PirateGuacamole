using Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static float WaterLevel;
    // Prefab for FocusArrow, comment for the sake of comment
    public static GameObject FocusArrowPrefab;
    // Prefab for CannonBall
    public static GameObject CannonBallPrefab;
    // Prefab for CannonBallLaunchSmoke
    public static GameObject CannonBallSmokePSPrefab;

    public static GameObject PlankPrefab;

    public static GameObject BrokenDeckPrefab;

    public ShakyFillBar WaterLevelFillBar;

    private void Awake() {
        FocusArrowPrefab = Resources.Load("Prefabs/FocusArrow")as GameObject;
        CannonBallPrefab = Resources.Load("Prefabs/CannonBall")as GameObject;
        CannonBallSmokePSPrefab = Resources.Load("Prefabs/CannonBallSmokePS")as GameObject;
        PlankPrefab = Resources.Load("Prefabs/WoodenPlank")as GameObject;
        BrokenDeckPrefab = Resources.Load("Prefabs/BrokenDeck")as GameObject;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        WaterLevelFillBar.Level = WaterLevel;
    }

    // Why not right ?
    private void OnGUI() {
        GUILayout.Label("P1: W A S D :: E");
        GUILayout.Label("P2: Up Left Down Right :: Keypad Enter");
    }

    public static void SplashPlanks(Vector3 p_Position) {
        int amount = Random.Range(3, 7);

        for (int i = 0; i < amount; i++) {
            GameObject tempPlank = Instantiate(PlankPrefab, p_Position + Vector3.up * Random.Range(-1f,1f) + Vector3.forward * Random.Range(-1f,1f), Quaternion.identity);
            tempPlank.transform.Rotate(Random.Range(0, 180f), Random.Range(0, 180f), Random.Range(0, 180f));

            tempPlank.GetComponent<Rigidbody>().AddExplosionForce(200f, p_Position, 10f);

            Destroy(tempPlank, Random.Range(2f, 3f));
        }
    }
}
