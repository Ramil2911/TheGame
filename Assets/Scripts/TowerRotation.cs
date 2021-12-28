using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerRotation : MonoBehaviour
{
    [SerializeField] private Transform _player;
    public float speed;
    private void Start()
    {
        _player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        //transform.position = castil.position;
        Vector3 relativePos = new Vector3(_player.position.x, transform.position.y, _player.position.z) - transform.position;

        // the second argument, upwards, defaults to Vector3.up
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * speed);
    }
}
