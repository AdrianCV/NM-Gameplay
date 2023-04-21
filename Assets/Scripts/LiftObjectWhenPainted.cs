using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftObjectWhenPainted : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] GameObject[] _objectsToLift;
    [SerializeField] Vector3 _positionToLiftTo;
    List<Vector3> _startPositions;


    PaintedObject _paintedObject;
    // Start is called before the first frame update
    void Start()
    {
        _paintedObject = GetComponent<PaintedObject>();
        for (int i = 0; i < _objectsToLift.Length; i++)
        {
            _startPositions.Add(_objectsToLift[i].transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_paintedObject.IsCorrect)
        {
            for (int i = 0; i < _objectsToLift.Length; i++)
            {
                _objectsToLift[i].transform.position = Vector3.Lerp(_objectsToLift[i].transform.position, _startPositions[i] + _positionToLiftTo, _moveSpeed * Time.deltaTime);
            }
        }
    }
}
