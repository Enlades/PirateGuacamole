using System.Linq;
using UnityEngine;

public class SeaController : MonoBehaviour {
    [Header("Sea Parameters")]
    public int XDimension;
    public int YDimension;
    public float Gap;

    [Header("Wave Parameters")]
    public float Speed;
    public float Period;
    public float WaveSize;
    public float WrinkleFrequency;
    public float WrinkleSize;

    private Vector3[] _vertices;
    private Vector3[] _verticesCur;
    // Mesh instance
    private Mesh _seaMesh;

    private void Start() {
        _seaMesh = GenerateMesh(XDimension, YDimension, Gap);
        GetComponent<MeshFilter>().sharedMesh = _seaMesh;
        _vertices = _seaMesh.vertices;
        _verticesCur = _vertices.ToArray();
    }

    private static Mesh GenerateMesh(int dimX, int dimY, float gap)
    {
        var mesh =  new Mesh();
        var vertices = new Vector3[dimX * dimY * 6];
        var triangles = new int[vertices.Length];
        var ind = 0;
        for (var y = 0; y < dimY; y++) {
            for (var x = 0; x < dimX; x++) {
                var pos = new Vector3(x - dimX / 2f, 0, y - dimY / 2f);

                //Triangle 1
                vertices[ind] = pos * gap;
                vertices[ind + 1] = (pos + Vector3.forward) * gap;
                vertices[ind + 2] = (pos + Vector3.right) * gap;
                triangles[ind] = ind;
                triangles[ind + 1] = ind + 1;
                triangles[ind + 2] = ind + 2;
                ind += 3;

                //Triangle 2
                vertices[ind] = (pos + Vector3.forward) * gap;
                vertices[ind + 1] = (pos + Vector3.right + Vector3.forward) * gap;
                vertices[ind + 2] = (pos + Vector3.right) * gap;
                triangles[ind] = ind;
                triangles[ind + 1] = ind + 1;
                triangles[ind + 2] = ind + 2;
                ind += 3;
            }
        }
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }

    private void Update() {
        float time = Time.time;
        for (var i = 0; i < _vertices.Length; i++) {
            _verticesCur[i].y = (Mathf.Sin(_vertices[i].z * Period + time * Speed) + Mathf.Sin(_vertices[i].z * Period * 0.4f + time * Speed) + Mathf.PerlinNoise(_vertices[i].x + time, _vertices[i].z + time)) * WaveSize;
            _verticesCur[i].z = _vertices[i].z + Mathf.Sin((_vertices[i].x + time * Gap) * WrinkleFrequency) * WrinkleSize + Mathf.Sin((_vertices[i].z * Mathf.Sin(time) +_vertices[i].x * Mathf.Cos(time)) * Period * 0.4f + time * Speed);
        }

        _seaMesh.vertices = _verticesCur;
        // Makes the lighting better :D
        _seaMesh.RecalculateNormals();
    }
}
