using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TutorialController : MonoBehaviour {

    public GameObject Player1, Player2, Hammer, KeyE, KeyQ, Key0, KeyEnter;

    private Vector3 _player1StartPos, _player2StartPos, _hammerStartPos;
    private bool _player1OneTime, _player2OneTime;
    private float _counter;

    private void Awake() {
        _player1StartPos = Player1.transform.position;
        _player2StartPos = Player2.transform.position;
        _hammerStartPos = Hammer.transform.position;

        _counter = 10f;
    }

    private void Start() {
        StartCoroutine(Counter());
    }

    private void Update() {
        if (Vector3.Distance(Player1.transform.position, Hammer.transform.position) < 1f) {
            KeyE.SetActive(true);

            if (!_player1OneTime) {

                _player1OneTime = true;
                StartCoroutine(DelayedSetActive(KeyQ));
            }
        }

        if (Vector3.Distance(Player2.transform.position, Hammer.transform.position) < 1f) {
            KeyEnter.SetActive(true);

            if (!_player2OneTime) {

                _player2OneTime = true;
                StartCoroutine(DelayedSetActive(Key0));
            }
        }
    }

    private void OnTriggerEnter(Collider col) {
        if (col.name.Equals("Player_1")) {
            Player1.transform.position = _player1StartPos;
        }else if (col.name.Equals("Player_2")) {
            Player2.transform.position = _player2StartPos;
        }else if (col.name.Equals("Hammer")) {
            Hammer.transform.position = _hammerStartPos + Vector3.up;
        }
    }

    private IEnumerator DelayedSetActive(GameObject p_GameObject) {
        yield return new WaitForSeconds(1f);

        p_GameObject.SetActive(true);
    }

    private IEnumerator Counter() {
        

        while (_counter > 0f) {
            GameObject.Find("CounterText").GetComponent<TextMesh>().text = _counter.ToString();

            yield return new WaitForSeconds(1f);

            _counter--;
        }

        GameObject.Find("CounterText").GetComponent<TextMesh>().text = "0";

        SceneManager.LoadScene(1);
    }
}
