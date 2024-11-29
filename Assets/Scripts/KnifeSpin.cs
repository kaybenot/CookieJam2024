using System;
using UnityEngine;

public class KnifeSpin : MonoBehaviour
{
    [SerializeField] private float degreePerSecond = 180f;
    [SerializeField] private float heightDiff = 0.4f;

    private Vector3 startPos;

    private void Awake()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up, degreePerSecond * Time.deltaTime);
        transform.position = startPos + Mathf.Sin(Time.time) * heightDiff * Vector3.up;
    }
}
