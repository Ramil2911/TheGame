using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private Transform _player;
    public float speed;
    public float speedRotate;
    public Rigidbody rb;
    void Start() 
    {
        _player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        var position = this.transform.position;
        var relativePos = new Vector3(_player.position.x, position.y, _player.position.z) - position;
        
        var rotation = Quaternion.LookRotation(relativePos);

        var interpolated = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * speedRotate);
        transform.rotation = interpolated;
        rb.velocity = interpolated * Vector3.forward * rb.velocity.magnitude;
    }
}
