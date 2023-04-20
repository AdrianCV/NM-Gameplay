using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintedObject : MonoBehaviour
{
    [HideInInspector] public Material CurrentMaterial;
    public Material CorrectMat;
    [SerializeField] Material _realCorrectMat;
    public bool IsCorrect;


    void Start()
    {
        CurrentMaterial = GetComponent<Renderer>().sharedMaterial;
    }

    void Update()
    {
        if (CurrentMaterial == CorrectMat)
        {
            GetComponent<Renderer>().sharedMaterial = _realCorrectMat;
            IsCorrect = true;
        }
        else
        {
            GetComponent<Renderer>().sharedMaterial = CurrentMaterial;
            IsCorrect = false;
        }
    }
}
