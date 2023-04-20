using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathMove : MonoBehaviour
{
    [SerializeField] float _speed;

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(_speed / 100, 0, _speed / 100);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            _speed = 0;
        }
    }
}
