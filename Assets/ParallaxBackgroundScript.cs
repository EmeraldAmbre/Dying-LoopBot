using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ParallaxBackgroundScript : MonoBehaviour
{
    private float _lenght, _startPosition;
    [SerializeField] private GameObject _camera;
    [SerializeField] private float _parrallaxSpeed;

    [SerializeField] private bool _isAutoScrolling = false;
    [SerializeField] private float _isAutoScrollingSpeed;
    private float _autoScrollingOffset;



    // Start is called before the first frame update
    void Start()
    {
        _startPosition = transform.position.x;
        _lenght = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distanceRelativeToCamera = (_camera.transform.position.x * (1 - _parrallaxSpeed));
        float distance;
        if (!_isAutoScrolling)
        {
            distance = (_camera.transform.position.x * _parrallaxSpeed);

            transform.position = new Vector3(_startPosition + distance, transform.position.y, transform.position.z);

            if (distanceRelativeToCamera > _startPosition + _lenght) _startPosition += _lenght;
            else if (distanceRelativeToCamera < _startPosition - _lenght) _startPosition -= _lenght;
        }
        else
        {
            _autoScrollingOffset += _isAutoScrollingSpeed;
            distance = _camera.transform.position.x + _autoScrollingOffset;


            transform.position = new Vector3(_startPosition + distance, transform.position.y, transform.position.z);

            if (Mathf.Abs(_autoScrollingOffset) > _lenght) _autoScrollingOffset = 0;
        }
    }
}
