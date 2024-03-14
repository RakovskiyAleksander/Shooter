using Unity.Mathematics;
using UnityEngine;

public class PlayerChracter : Character
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _head;
    [SerializeField] private Transform _cameraPoint;
    [SerializeField] private float _maxHeadAngle = 90;
    [SerializeField] private float _minHeadAngle = -90;
    [SerializeField] private float _jumpForce = 5;
    [SerializeField] private CheckFly _checkFly;
    private float _jumpDelay = 0.2f;

    private float _inputH, _inputV, _rotateY;
    private float _currentRotateX;
    private float _jumpTime;

    public void Start()
    {
        Transform camera = Camera.main.transform;
        camera.parent = _cameraPoint;
        camera.localPosition = Vector3.zero;
        camera.localRotation = Quaternion.identity;
    }

    public void SetInput(float inputH, float inputV, float rotateY)
    {
        _inputH = inputH;
        _inputV = inputV;
        _rotateY += rotateY;
    }

    private void FixedUpdate()
    {
        Move();
        RotateY();
    }

    private void Move()
    {
        //Vector3 direction = new Vector3(_inputH, 0, _inputV).normalized;
        //transform.position += direction * Time.deltaTime * _speed;
        Vector3 velociti = (transform.forward * _inputV + transform.right * _inputH).normalized * speed;
        velociti.y = _rigidbody.velocity.y;
        velocity = velociti;
        _rigidbody.velocity = velocity;

    }

    private void RotateY()
    {
        _rigidbody.angularVelocity = new Vector3(0, _rotateY, 0);
        _rotateY = 0;
    }

    public void RotateX(float value)
    {
        _currentRotateX = Mathf.Clamp(_currentRotateX + value, _minHeadAngle, _maxHeadAngle);
        _head.localEulerAngles = new Vector3(_currentRotateX, 0, 0);
    }

    public void GetMoveInfo(out Vector3 position, out Vector3 velocity, out float rotateX, out float rotateY)
    {
        position = transform.position;
        velocity = _rigidbody.velocity;
        
        rotateX = _head.localEulerAngles.x;
        rotateY = transform.eulerAngles.y;
    }


    public void Jump()
    {
        if (_checkFly.IsFly) return;
        //if (Time.deltaTime - _jumpTime < _jumpDelay) return;
        _jumpTime = Time.time;
        _rigidbody.AddForce(0, _jumpForce, 0, ForceMode.VelocityChange);
    }
}
