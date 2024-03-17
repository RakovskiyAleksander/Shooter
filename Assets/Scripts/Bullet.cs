using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _lifeTime = 3f;
    [SerializeField] private Rigidbody _rigidbody;
    public void Init(Vector3 velocity)
    {
        _rigidbody.velocity = velocity;
        StartCoroutine(DelayDestroy());
    }

    private IEnumerator DelayDestroy()
    { 
     yield return new WaitForSecondsRealtime(_lifeTime);
        Destroy();
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy();
    }

}
