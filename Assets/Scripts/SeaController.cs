using UnityEngine;

public class SeaController : MonoBehaviour {

    // Random distance range for vertices
    public Vector2 Range = new Vector2(0.1f, 1);
    // Speed of movement
    public float Speed = 1;

    // Random generated number for pingpong
    private float[] _randomTimes;
    // Mesh instance
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

        // Pingpong vertices with the values from random array.
        for (int i = 0; i < _seaMesh.vertices.Length; i++) {
            vertices[i].z = 1 * Mathf.PingPong(Time.time * Speed, _randomTimes[i]);
        }

        _seaMesh.vertices = vertices;
        // Makes the lighting better :D
        _seaMesh.RecalculateNormals();
    }
}
