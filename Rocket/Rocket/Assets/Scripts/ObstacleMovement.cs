using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ObstacleMovement : MonoBehaviour
{
    [SerializeField] Vector3 _movementVector;
    [SerializeField] float period = 2f; 
    [Range(0, 1)] [SerializeField] float _movementFactor;
    [SerializeField] float _rotateSpeedZ;
    [SerializeField] float _rotateSpeedY;
    Vector3 _startPos;
    

    void Start()
    {
        _startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        VerticalOscillate();
        RotationsZ();
        RotationsY();
    }
    private void RotationsZ()
    {
        transform.Rotate(Vector3.forward * _rotateSpeedZ * Time.deltaTime);
    }
    private void RotationsY()
    {
        transform.Rotate(Vector3.up * _rotateSpeedY * Time.deltaTime);
    }
    private void VerticalOscillate()
    {
        if(period <= Mathf.Epsilon) // protection against zero period
        {
            return;
        }
        float _cycles = Time.time / period;
        const float tau = Mathf.PI * 2;
        float _rawSinWave = Mathf.Sin(_cycles * tau);
        _movementFactor = _rawSinWave / 2f + 0.5f;
        Vector3 _offset = _movementVector * _movementFactor;
        transform.position = _startPos + _offset;
    }
}
