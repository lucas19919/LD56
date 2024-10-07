using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouySwim : MonoBehaviour
{
    public float amplitude = 0.2f;
    public float frequency = 1.0f;

    private float height;

    private void Awake()
    {
        height = this.transform.position.y;
    }
    void Update()
    {
        float wave = amplitude * Mathf.Sin(Time.time * frequency);
        this.transform.position = new Vector3 (this.transform.position.x, height + wave, this.transform.position.z);
    }
}
