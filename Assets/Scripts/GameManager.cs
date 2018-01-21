using System.Collections;
using Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static float WaterLevel
    {
        get { return _waterLevel; }
        set { _waterLevel = Mathf.Clamp01(value); }
    }

    // Prefab for FocusArrow, comment for the sake of comment
    public static GameObject FocusArrowPrefab;
    // Prefab for CannonBall
    public static GameObject CannonBallPrefab;
    // Prefab for CannonBallLaunchSmoke
    public static GameObject CannonBallSmokePSPrefab;

    public static GameObject PlankPrefab;

    public static GameObject BrokenDeckPrefab;
    private static float _waterLevel;

    public ShakyFillBar WaterLevelFillBar;
    public ScoreDisplay Score;

    public GameObject GameOverGameObject;

    public static int GameLevel;

    private GameObject _ship;
    private GameObject _waterSplashes;
    private bool _oneTimeEndGame;

    private float _timer;

    private void Awake() {
        FocusArrowPrefab = Resources.Load("Prefabs/FocusArrow")as GameObject;
        CannonBallPrefab = Resources.Load("Prefabs/CannonBall")as GameObject;
        CannonBallSmokePSPrefab = Resources.Load("Prefabs/CannonBallSmokePS")as GameObject;
        PlankPrefab = Resources.Load("Prefabs/WoodenPlank")as GameObject;
        BrokenDeckPrefab = Resources.Load("Prefabs/BrokenDeck")as GameObject;
        FindObjectOfType<SquidController>().HitCallBack += () => Score.Score += 1000;
        _timer = 0f;

        _ship = GameObject.Find("ShipMajor");
        _waterSplashes = GameObject.Find("WaterSplashes");

        _oneTimeEndGame = false;

        GameLevel = 1;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        if (WaterLevelFillBar == null)
            return;

        WaterLevelFillBar.Level = WaterLevel;

        if (!_oneTimeEndGame && WaterLevel >= 1) {
            _oneTimeEndGame = true;
            SinkShip();
        }

        GameLevel = (int)(Time.time / 5f);

        _timer += Time.deltaTime;
        if (_timer > 1f)
        {
            _timer -= 1f;
            Score.Score += 10;
        }
    }

    // Why not right ?
    /*private void OnGUI() {
        GUILayout.Label("P1: W A S D :: E");
        GUILayout.Label("P2: Up Left Down Right :: Keypad Enter");
    }So long*/

    public void SinkShip() {
        StartCoroutine(EndGame());
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

    private IEnumerator EndGame() {

        float timer = 2f;

        GameObject[] triggerAreas = GameObject.FindGameObjectsWithTag("TriggerArea");

        for (int i = 0; i < triggerAreas.Length; i++) {
            triggerAreas[i].SetActive(false);
            triggerAreas[i].GetComponent<TriggerAreaController>().BrokenDeckPart.SetActive(false);
        }

        while (timer > 0) {

            _ship.transform.Translate(Vector3.down * Time.deltaTime * 2, Space.World);

            timer -= Time.deltaTime;

            yield return null;
        }

        TriggerAreaManager.Instance.Stop();

        yield return new WaitForSeconds(2f);

        _waterSplashes.SetActive(false);

        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++) {
            players[i].SetActive(false);
        }

        GameOverGameObject.SetActive(true);

        Vector3 targetPos = GameOverGameObject.transform.position;
        targetPos.y = 2f;

        timer = 1f;

        while (timer > 0) {

            GameOverGameObject.transform.position =
                Vector3.Lerp(GameOverGameObject.transform.position, targetPos, 1 - timer);

            timer -= Time.deltaTime;

            yield return null;
        }

        timer = 2f;

        while (timer > 0) {
            timer -= Time.deltaTime;

            if(Input.GetMouseButtonDown(0) || Input.GetButtonDown("P1_Primary") || Input.GetButtonDown("P2_Primary"))
                SceneManager.LoadScene(0);

            yield return null;
        }

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(0);
    }
}
