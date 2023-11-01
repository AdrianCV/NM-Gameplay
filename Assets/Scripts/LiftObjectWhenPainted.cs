using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PaintedObject))]
public class LiftObjectWhenPainted : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] float _moveSpeed = 1f;
    [SerializeField] GameObject[] _objectsToLift;
    [SerializeField] Vector3 _positionToLiftTo;
    [SerializeField] bool _shouldRotate;
    [SerializeField] bool _isLift;
    [SerializeField] Vector3 _rotationToRotateTo;
    [SerializeField] Vector3 _newRotation;
    [SerializeField] List<Vector3> _startPositions;

    List<Vector3> _finalNewPositions = new List<Vector3>();


    PaintedObject _paintedObject;
    // Start is called before the first frame update
    void Start()
    {
        _paintedObject = GetComponent<PaintedObject>();
        foreach (var t in _objectsToLift)
        {
            _startPositions.Add(t.transform.position);
        }

        foreach (var startPos in _objectsToLift)
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
            _player.transform.SetParent(_objectsToLift[0].transform);
        }
        
        for (int i = 0; i < _objectsToLift.Length; i++)
        {
            _objectsToLift[i].transform.position = Vector3.Lerp(_objectsToLift[i].transform.position, _startPositions[i] + _positionToLiftTo, _moveSpeed * Time.deltaTime);

            if (Vector3.Distance(_objectsToLift[i].transform.position, _finalNewPositions[i]) < 10)
            {
                _isLift = false;
                _player.transform.parent = null;
            }

            if (!_shouldRotate) continue;
            var newRotation = Quaternion.Euler(_objectsToLift[i].transform.rotation.eulerAngles + _rotationToRotateTo);

            _newRotation = newRotation.eulerAngles;

            _objectsToLift[i].transform.rotation = Quaternion.Lerp(_objectsToLift[i].transform.rotation, newRotation,
                _moveSpeed * Time.deltaTime);
        }
    }
}
