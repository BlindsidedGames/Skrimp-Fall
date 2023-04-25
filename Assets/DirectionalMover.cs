using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalMover : MonoBehaviour
{
    public float moveSpeed;
    public Vector2 positionToMoveTo;
    private Vector2 _startingPosition;
    private bool _reverse;


    private void Start()
    {
        _startingPosition = transform.position;
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, _reverse ? _startingPosition : positionToMoveTo,
            moveSpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, _reverse ? _startingPosition : positionToMoveTo) <= 0.001f)
            _reverse = !_reverse;
    }
}