using System;
using Mirror;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class BulletComponent : NetworkBehaviour
{
    [SerializeField] public int damageAmount = 10;
    [SerializeField] public float timeToLive = 10;
    private float _lifetime = 0.0f;
    private Vector3 _previousPos;
    public float speed;

    public GameObject explosion;
    public GameObject owner;

    private Rigidbody _rigidbody;
    private void Start()
    {
        _previousPos = this.transform.position;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = transform.forward * speed;
    }

    private void Update()
    {
        _lifetime += Time.deltaTime;
        if(_lifetime>timeToLive)
            Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
            if(owner != null && other.gameObject.GetInstanceID() == owner.GetInstanceID())
                return;
            if (other.transform.TryGetComponent<EntityComponent>(out var entityComponent))
            {
                
                entityComponent.Damage(damageAmount, null);
            }
            SpawnExplosion();
            Destroy(gameObject);
    }

    private void SpawnExplosion()
    {
        if(explosion != null)
        {
            var go = Instantiate(explosion, transform.position, transform.rotation);
        }
    }
}
