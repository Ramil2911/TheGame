using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonReset : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    
    private void FixedUpdate()
    {
        CheckForInput();
    }

    private void CheckForInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (Vector3.Distance(transform.position, hit.point) <= 10)
                {
                    if (hit.transform.CompareTag("ResetButton"))
                    {
                        SceneManager.LoadScene("Game");
                    }
                }
            }
        }
    }
}
