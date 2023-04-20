using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMove : MonoBehaviour
{
    public float Speed;

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(Speed / 100, 0, Speed / 100);
    }
}
