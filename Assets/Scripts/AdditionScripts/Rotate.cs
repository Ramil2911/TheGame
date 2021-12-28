using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 vecotrRotation;
    public float speed = 1;


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(vecotrRotation * speed * Time.deltaTime);
    }
}
