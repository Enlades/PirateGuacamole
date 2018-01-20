using UnityEngine;

public class SeaController : MonoBehaviour {

    public Vector2 Range = new Vector2(0.1f, 1);
    public float Speed = 1;

    private float[] _randomTimes;
    private Mesh _seaMesh;

    private void Start() {
        _seaMesh = GetComponent<MeshFilter>().mesh;

        _randomTimes = new float[_seaMesh.vertices.Length];

        for (int i = 0; i < _seaMesh.vertices.Length; i++) {
            _randomTimes[i] = Random.Range(Range.x, Range.y);
        }
    }

    private void Update() {
        Vector3[] vertices = _seaMesh.vertices;

        for (int i = 0; i < _seaMesh.vertices.Length; i++) {
            vertices[i].z = 1 * Mathf.PingPong(Time.time * Speed, _randomTimes[i]);
        }

        _seaMesh.vertices = vertices;
        _seaMesh.RecalculateNormals();
    }
}
