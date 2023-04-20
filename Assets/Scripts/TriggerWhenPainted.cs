using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWhenPainted : MonoBehaviour
{
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
            GetComponent<Collider>().isTrigger = true;
        }
        else
        {
            GetComponent<Collider>().isTrigger = false;
        }
    }
}
