using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadiationSourceComponent : MonoBehaviour
{
    public float radius = 0;
    public float damage = 0;
    public float fadeDuration = 0;
    
    public float inheritedRadius = 0;
    public float inheritedDamage = 0;
    public float inheritedDuration = 0;

    public List<RadValue> inheritedProperties = new List<RadValue>();

    public EntityComponent entityComponent;

    public GameObject particles;

    private IDictionary<int, RadValue> _affectedObjects 
        = new Dictionary<int, RadValue>(new IntValueComparer());
    
    private RaycastHit[] _hits = new RaycastHit[128]; //i hope you wont make more than 128 walls between entity and player
    private SphereCollider _sphereCollider;
    private GameObject _particleSource;
    private MeshCollider _uranCollider;

    public bool isPlayer = false;
    private void Start()
    {   if(gameObject.CompareTag("Player"))
        {
            isPlayer = true;
        }
        TryGetComponent<EntityComponent>(out entityComponent);
        _sphereCollider = this.gameObject.AddComponent<SphereCollider>();
        _sphereCollider.radius = radius;
        _sphereCollider.isTrigger = true;

        var transform1 = this.transform;
        if(isPlayer == false)
        {
            _particleSource = Instantiate(particles, transform1.position, Quaternion.identity, transform1);
            _uranCollider = _particleSource.GetComponentInChildren<MeshCollider>();
            _uranCollider.transform.localScale = new Vector3(radius * 2, radius * 2, radius * 2);
        }
    }

    private void Update()
    {
        inheritedDamage = damage;
        inheritedRadius = radius;
        for (var index = 0; index < inheritedProperties.Count; index++)
        {
            var property = inheritedProperties[index];
            inheritedDamage += property.damage;
            if (property.radius > inheritedRadius) inheritedRadius = property.radius;
        }
        if(isPlayer == false)
        {
            _uranCollider.transform.localScale = new Vector3(inheritedRadius * 2, inheritedRadius * 2, inheritedRadius * 2);
            _sphereCollider.radius = inheritedRadius;
        }

        if (entityComponent != null && !entityComponent.isImmuneToRadiation)
        {
            entityComponent.Damage(inheritedDamage * Time.deltaTime, null);
        }
        
        //recount damage of objects inside
        foreach (var pair in _affectedObjects)
        {
            if(gameObject)
            {
                CountChildDamage(pair.Value.owner, pair.Value); //it is faster to operate with Dictionaries using foreach
            }
            else
            {
                
            }
            
        }
        
        
    }

    private void CountChildDamage(RadiationSourceComponent subject, RadValue value)
    {
        var entityPosition = subject.transform.position;
        var size = Physics.RaycastNonAlloc(entityPosition, this.transform.position - entityPosition, _hits, inheritedRadius);
        var damageMultiplier = 1.0f;
        var impactRadius = inheritedRadius;
        for (var index = 0; index < size; index++)
        {
            var wall = _hits[index];
            if (wall.transform.CompareTag("ThinWall"))
            {
                damageMultiplier *= 0.9f;
                impactRadius -= (impactRadius - wall.distance) * 0.1f;
            }
            else if (wall.transform.CompareTag("MediumWall"))
            {
                damageMultiplier *= 0.6f;
                impactRadius -= (impactRadius - wall.distance) * 0.4f;
            }
            else if (wall.transform.CompareTag("ThickWall"))
            {
                damageMultiplier *= 0.1f;
                impactRadius -= (impactRadius - wall.distance) * 0.6f;
            }
            else if (wall.transform.CompareTag("BlockingWall"))
            {
                damageMultiplier = 0f;
                break;
            }
        }
        
        var sourcePosition = transform.position;
        if(((entityPosition - sourcePosition).magnitude > impactRadius))
        {
            value.damage = 0;
            value.radius = 0;
        }
        else
        {
            value.damage = inheritedDamage * damageMultiplier * (1.0f - (entityPosition - sourcePosition).magnitude / inheritedRadius);
            value.radius = inheritedRadius * damageMultiplier * (1.0f - (entityPosition - sourcePosition).magnitude / inheritedRadius);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var instanceID = other.gameObject.GetInstanceID();
        if(instanceID == this.gameObject.GetInstanceID()) return;
        if(_affectedObjects.ContainsKey(instanceID)
           || instanceID == this.gameObject.GetInstanceID()) return;
        if (other.gameObject.TryGetComponent<EntityComponent>(out _))
        {
            if (!other.gameObject.TryGetComponent<RadiationSourceComponent>(out var source))
            {
                source = other.gameObject.AddComponent<RadiationSourceComponent>();
                source.particles = particles;
            }

            var radValue = new RadValue() {parent = this, owner = source};
            _affectedObjects.Add(source.gameObject.GetInstanceID(), radValue);
            source.inheritedProperties.Add(radValue);
            //CountChildDamage();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var instanceID = other.gameObject.GetInstanceID();
        if(instanceID == this.gameObject.GetInstanceID()) return;

        if (other.gameObject.TryGetComponent<EntityComponent>(out _) && _affectedObjects.ContainsKey(instanceID))
        {
            var rad = _affectedObjects[instanceID];
            //radiationSourceComponent.FadeAway(fadeDuration);
            _affectedObjects.Remove(rad.owner.gameObject.GetInstanceID());
            var obj = other.GetComponent<RadiationSourceComponent>();
            obj.inheritedProperties.Remove(rad);
            if(obj.inheritedProperties.Count == 0) Destroy(obj);
        }
    }

    public void FadeAway(float duration)
    {
        StartCoroutine(Fade(duration)); 
        //i decided not to process fading values, they'll be destroyed anyways
    }

    private IEnumerator Fade(float duration)
    {
        var time = 0f;
        var startDamage = damage;
        var startRadius = radius;
        while (time < duration)
        {
            time += Time.deltaTime;
            damage = startDamage * (duration - time) / duration;
            radius = startRadius * (duration - time) / duration;
            yield return null;
        }
        Destroy(this);
    }

    private void OnDestroy()
    {
        foreach (var pair in _affectedObjects)
        {
            pair.Value.owner.inheritedProperties.Remove(pair.Value);
        }
        Destroy(_sphereCollider);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.2f, 0.2f, 0.2f, 0.2f);
        Gizmos.DrawSphere(transform.position, inheritedRadius);
    }
}

public class IntValueComparer : IEqualityComparer<int>
{
    public bool Equals(int x, int y)
    {
        return x == y;
    }

    public int GetHashCode(int obj)
    {
        return obj;
    }
}

public class RadValue
{
    public float radius = 100;
    public float damage = 2f;
    public float fadeDuration = 4;
    public RadiationSourceComponent parent;
    public RadiationSourceComponent owner;
}

