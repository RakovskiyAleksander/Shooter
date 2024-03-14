using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacter : MonoBehaviour
{
    public Vector3 targetPosition { get; private set; } = Vector3.zero;
    private float _velosityMagnitude = 0;

    public void Start()
    {
        targetPosition = transform.position;
    }

    public void Update()
    {
        if (_velosityMagnitude > .1f)
        {
            float maxDistance = _velosityMagnitude * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, maxDistance);
        }
        else
        {
            transform.position = targetPosition;
        }
    }

    public void SetMovement(in Vector3 position, in Vector3 velocity, in float averageInterval)
    {
        targetPosition = position + velocity * averageInterval;
        _velosityMagnitude = velocity.magnitude;
    }
}