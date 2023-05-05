using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotator : MonoBehaviour
{
    public float rotationSpeed;
    public bool randomRotation;
    public Vector2 randomMinMax;
    private float randomTime;
    private float currentRandomTime;
    public bool reverse;

    private void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * (reverse ? Time.deltaTime : -Time.deltaTime));
        if (randomRotation)
        {
            currentRandomTime += Time.deltaTime;
            if (currentRandomTime >= randomTime)
            {
                reverse = !reverse;
                currentRandomTime = 0;
                randomTime = Random.Range(randomMinMax.x, randomMinMax.y);
            }
        }
    }
}