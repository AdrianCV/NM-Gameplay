using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintedObject : MonoBehaviour
{
    public Material CorrectMat;
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
            GetComponent<Collider>().isTrigger = true;
        }
        else
        {
            GetComponent<Collider>().isTrigger = false;
        }
    }
}
