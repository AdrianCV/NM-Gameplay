using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextSceneScript : MonoBehaviour
{
    bool _touching;

    private void Update()
    {
        if (_touching && Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("PaintScene");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _touching = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _touching = false;
        }
    }
}
