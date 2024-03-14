using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckFly : MonoBehaviour
{
    public bool IsFly { get; private set; }
    [SerializeField] private float _radius = 0.2f;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _coyoteTime = 0.15f;
    private float _flyTime = 0;


    public void Update()
    {
        if (Physics.CheckSphere(transform.position, _radius, _layerMask))
        {
            IsFly = false;
            _flyTime = 0;
        }
        else
        {
            _flyTime += Time.deltaTime;
            if (_flyTime > _coyoteTime) { IsFly = true; }
        }
    }

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, _radius);
    }
#endif
}
