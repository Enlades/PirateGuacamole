using System.Collections.Generic;
using UnityEngine;

namespace Helpers
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(BoxCollider))]
    public class ShakyFillBar : MonoBehaviour
    {
        [Range(0f, 1f)]
        public float Level;

        private Mesh _fillMesh;
        private BoxCollider _box;
        private Vector3[] _vertices;

        private void Start()
        {
            _box = GetComponent<BoxCollider>();
            _fillMesh = GenerateFillMesh();
            _vertices = _fillMesh.vertices;
            GetComponent<MeshFilter>().sharedMesh = _fillMesh;
        }

        private Mesh GenerateFillMesh()
        {
            var mesh = new Mesh();
            var vertices = new List<Vector3>();
            var triangles = new List<int>();

            //First 2 vertices, these are static
            vertices.Add(_box.center - _box.size.y * Vector3.up / 2 - _box.size.x * Vector3.right / 2 + _box.size.z * Vector3.forward / 2);
            vertices.Add(_box.center - _box.size.y * Vector3.up / 2 + _box.size.x * Vector3.right / 2 + _box.size.z * Vector3.forward / 2);
            //Next 2, still for the same quad
            vertices.Add(vertices[0] + _box.size.y * Vector3.up);
            vertices.Add(vertices[1] + _box.size.y * Vector3.up);
            //First quad
            triangles.AddRange(new[] { 0, 1, 2, 1, 3, 2 });

            //Next quad
            vertices.Add(vertices[0] + _box.size.y * Vector3.up);
            vertices.Add(vertices[1] + _box.size.y * Vector3.up);
            vertices.Add(vertices[0] + _box.size.y * Vector3.up - _box.size.z * Vector3.forward);
            vertices.Add(vertices[1] + _box.size.y * Vector3.up - _box.size.z * Vector3.forward);
            //Second quad
            triangles.AddRange(new[] { 4, 5, 6, 5, 7, 6 });

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();

            return mesh;
        }

        private void Update()
        {
            var minY = _box.center.y - _box.size.y / 2;
            var sizeY = _box.size.y;
            //Skip the first 2 indexes as they are the bottom of the fill
            for (var i = 2; i < _vertices.Length; i++)
            {
                _vertices[i].y = Mathf.Clamp(minY + sizeY * Level +
                    Mathf.PerlinNoise(_vertices[i].x * 100f + Time.time, _vertices[i].z * 100f + Time.time), minY, minY + sizeY);
            }

            _fillMesh.vertices = _vertices;
            _fillMesh.RecalculateNormals();
        }
    }
}
