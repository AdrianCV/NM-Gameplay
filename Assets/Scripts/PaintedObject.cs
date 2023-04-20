using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintedObject : MonoBehaviour
{
    public Material CorrectMat;
    [SerializeField] Material _realCorrectMat;
    public bool IsCorrect;

    private void Update()
    {
        if (GetComponent<Renderer>().sharedMaterial == CorrectMat)
        {
            IsCorrect = true;
        }
        else
        {
            IsCorrect = false;
        }

        if (IsCorrect)
        {
            GetComponent<Renderer>().sharedMaterial = _realCorrectMat;
        }
    }
}
