using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveBehavior : MonoBehaviour
{
    public float waveFrequency;
    public float waveStrength;

    private Vector3[] originalVertices;
    private Vector3[] displacedVertices;

    private Mesh mesh;

    void Start()
    {
        mesh = this.GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
    }

    void Update()
    {
        for (int i = 0; i < displacedVertices.Length; i++)
        {
            Vector3 vertex = originalVertices[i];
            float waveMagnitude = Mathf.PerlinNoise(vertex.x * waveFrequency * Time.time, vertex.z * waveFrequency * Time.time) * waveStrength;
            displacedVertices[i] = new Vector3(vertex.x, waveMagnitude, vertex.z);
        }

        mesh.vertices = displacedVertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
    }
}
