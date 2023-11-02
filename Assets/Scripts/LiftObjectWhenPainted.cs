using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PaintedObject))]
public class LiftObjectWhenPainted : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] float _moveSpeed = 1f;
    [SerializeField] GameObject[] _objectsToAffect;
    [SerializeField] private bool _shouldLift;
    [SerializeField] Vector3 _positionToLiftTo;
    [SerializeField] private bool _shouldRotateX;
    [SerializeField] private float _xRotation;
    [SerializeField] private bool _shouldRotateY;
    [SerializeField] private float _yRotation;
    [SerializeField] private bool _shouldRotateZ;
    [SerializeField] private float _zRotation;
    [SerializeField] private float _degreesPerSecond;
    [SerializeField] private GameObject _pivotPoint;
    [SerializeField] bool _isLift;
    [SerializeField] List<Vector3> _startPositions;

    List<Vector3> _finalNewPositions = new List<Vector3>();


    PaintedObject _paintedObject;
    // Start is called before the first frame update
    void Start()
    {
        _paintedObject = GetComponent<PaintedObject>();
        foreach (var t in _objectsToAffect)
        {
            _startPositions.Add(t.transform.position);
        }

        foreach (var startPos in _objectsToAffect)
        {
            _finalNewPositions.Add(startPos.transform.position + _positionToLiftTo);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_paintedObject.IsCorrect) return;
        if (_isLift)
        {
            _player.transform.SetParent(_objectsToAffect[0].transform);
        }
        
        for (int i = 0; i < _objectsToAffect.Length; i++)
        {
            if (_shouldLift)
            {
                _objectsToAffect[i].transform.position = Vector3.Lerp(_objectsToAffect[i].transform.position, _startPositions[i] + _positionToLiftTo, _moveSpeed * Time.deltaTime);

                if (Vector3.Distance(_objectsToAffect[i].transform.position, _finalNewPositions[i]) < 10)
                {
                    _isLift = false;
                    _player.transform.parent = null;
                } 
            }

            if (_shouldRotateX)
            {
                _objectsToAffect[i].transform.RotateAround(_pivotPoint.transform.position, _pivotPoint.transform.right, Time.deltaTime * _degreesPerSecond);
                print("wehoo");
                StartCoroutine(StopRotatingX());
            }

            if (_shouldRotateY)
            {
                _objectsToAffect[i].transform.RotateAround(_pivotPoint.transform.position, _pivotPoint.transform.up, Time.deltaTime * _degreesPerSecond);
                StartCoroutine(StopRotatingY());
            }

            if (_shouldRotateZ)
            {
                _objectsToAffect[i].transform.RotateAround(_pivotPoint.transform.position, _pivotPoint.transform.forward, Time.deltaTime * _degreesPerSecond);
                StartCoroutine(StopRotatingZ());
            }
        }
    }


    private IEnumerator StopRotatingX()
    {
        yield return new WaitForSeconds(_xRotation / _degreesPerSecond);
        _shouldRotateX = false;
    }
    
    private IEnumerator StopRotatingY()
    {
        yield return new WaitForSeconds(_yRotation / _degreesPerSecond);
        _shouldRotateY = false;
    }
    
    private IEnumerator StopRotatingZ()
    {
        yield return new WaitForSeconds(_zRotation / _degreesPerSecond);
        _shouldRotateZ = false;
    }
}
