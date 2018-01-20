using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeaCombiner : MonoBehaviour {

    private void Awake() {
        GameObject[] seaParts = GameObject.FindGameObjectsWithTag("SeaPart");

        for (int i = 0; i < seaParts.Length; i++) {
            seaParts[i].transform.SetParent(transform);
        }

        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int k = 0;
        while (k < meshFilters.Length) {
            if(meshFilters[k] == null)
                continue;

            combine[k].mesh = meshFilters[k].sharedMesh;
            combine[k].transform = meshFilters[k].transform.localToWorldMatrix;
            meshFilters[k].gameObject.SetActive(false);
            k++;
        }

        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine, true);
        transform.gameObject.SetActive(true);

        for (int i = 0; i < seaParts.Length; i++) {
            seaParts[i].transform.SetParent(null);
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
