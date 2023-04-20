using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftObjectWhenPainted : MonoBehaviour
{
    [SerializeField] float _moveSpeed;
    [SerializeField] GameObject _objectToLift;
    [SerializeField] Vector3 _positionToLiftTo;


    PaintedObject _paintedObject;
    // Start is called before the first frame update
    void Start()
    {
        _paintedObject = GetComponent<PaintedObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_paintedObject.IsCorrect)
        {
            _objectToLift.transform.position = Vector3.Lerp(_objectToLift.transform.position, _positionToLiftTo, _moveSpeed * Time.deltaTime);
        }
    }
}
