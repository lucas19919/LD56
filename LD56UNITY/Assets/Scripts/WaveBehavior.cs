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
    private MeshCollider meshCollider;

    void Start()
    {
        // Initialize mesh and collider, store original vertex positions
        mesh = this.GetComponent<MeshFilter>().mesh;
        meshCollider = this.GetComponent<MeshCollider>();

        originalVertices = mesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
    }

    void Update()
    {
        // Apply wave effect to each vertex based on Perlin noise
        for (int i = 0; i < displacedVertices.Length; i++)
        {
            Vector3 vertex = originalVertices[i];
            float waveMagnitude = Mathf.PerlinNoise(vertex.x * waveFrequency * Time.time, vertex.z * waveFrequency * Time.time) * waveStrength;
            displacedVertices[i] = new Vector3(vertex.x, waveMagnitude, vertex.z);
        }

        // Update the mesh and collider with the displaced vertices
        mesh.vertices = displacedVertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        meshCollider.sharedMesh = mesh;
    }
}
