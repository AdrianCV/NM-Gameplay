using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintedObject : MonoBehaviour
{
    [HideInInspector] public Material CurrentMaterial;
    public Material CorrectMat;
    [SerializeField] Material _realCorrectMat;
    public bool IsCorrect;
    Renderer _renderer;


    void Start()
    {
        _renderer = GetComponent<Renderer>();
        CurrentMaterial = GetComponent<Renderer>().sharedMaterial;
        gameObject.layer = 6;
    }

    void Update()
    {
        /*
        if (CurrentMaterial == CorrectMat)
        {
            _renderer.sharedMaterial = _realCorrectMat;
            IsCorrect = true;
        }
        else
        {
            _renderer.sharedMaterial = CurrentMaterial;
            IsCorrect = false;
        }
        */
    }

    public IEnumerator ChangeMat(Material currentMat)
    {
        yield return new WaitForSeconds(0.8f);
        if (currentMat == CorrectMat)
        {
            _renderer.sharedMaterial = _realCorrectMat;
            IsCorrect = true;
        }
        else
        {
            _renderer.sharedMaterial = CurrentMaterial;
            IsCorrect = false;
        } 
    }
}
