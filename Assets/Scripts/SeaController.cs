using UnityEngine;

public class SeaController : MonoBehaviour {

    public Vector2 range = new Vector2(0.1f, 1);
    public float speed = 1;
    float[] randomTimes;
    private Mesh seaMesh;

    private void Start() {
        seaMesh = GetComponent<MeshFilter>().mesh;
        int i = 0;
        randomTimes = new float[seaMesh.vertices.Length];

        //Color[] colors = new Color[mesh.vertices.Length];

        while (i < seaMesh.vertices.Length) {
            randomTimes[i] = Random.Range(range.x, range.y);
            //colors[i] = new Color(Random.Range(0f, 0.2f), Random.Range(0f, 0.2f), Random.Range(0.9f, 1f));

            i++;
        }

        //mesh.colors = colors;
    }

    private void Update() {
        Vector3[] vertices = seaMesh.vertices;
        Vector3[] normals = seaMesh.normals;

        int i = 0;
        while (i < vertices.Length) {
            vertices[i].z = 1 * Mathf.PingPong(Time.time * speed, randomTimes[i]);
            i++;
        }
        seaMesh.vertices = vertices;
        seaMesh.RecalculateNormals();
    }
}
