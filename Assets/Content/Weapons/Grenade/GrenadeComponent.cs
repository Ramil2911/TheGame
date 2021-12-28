using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeComponent : MonoBehaviour
{
    [SerializeField] public int damageAmount = 180;
    [SerializeField] public float timeToLive = 10;
    [SerializeField] public float radius = 10;
    private float _lifetime = 0.0f;

    private void Update()
    {
        _lifetime += Time.deltaTime;
        if (_lifetime > timeToLive)
        {
            var cast = Physics.OverlapSphere(this.transform.position, radius);
            foreach (var hit in cast)
            {
                if (hit.transform.gameObject.TryGetComponent<EntityComponent>(out var go))
                {
                    go.Damage((int)(hit.contactOffset/radius)*damageAmount, null);
                }
                //Debug.Log(hit.name);
            }
            Destroy(this.gameObject);
        }
    }
}
