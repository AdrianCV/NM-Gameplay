using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PaintedObject))]
public class TriggerWhenPainted : MonoBehaviour
{
    PaintedObject _paintedObject;

    Collider _collider;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
        _paintedObject = GetComponent<PaintedObject>();
    }

    // Update is called once per frame
    void Update()
    {
        _collider.isTrigger = _paintedObject.IsCorrect;
    }
}
