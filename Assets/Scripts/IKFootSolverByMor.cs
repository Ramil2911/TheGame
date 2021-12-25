using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKFootSolverByMor : MonoBehaviour
{
    public Transform origin;
    [SerializeField] LayerMask groundLayer = default;
    [SerializeField] Transform body = default;
    //[SerializeField] float footSpacing;
    [SerializeField] float stepDistance = 4; // расстояние для шага
     [SerializeField] float stepHeight = 4; // высота подняти ноги при шаге
     [SerializeField] float speed = 1; //скорость переставления ноги
     Vector3 oldPosition, currentPosition, newPosition;

    float lerp;

    public AudioSource audioSource;
    private void Start()
    {
        currentPosition = transform.position;
    }
    void Update()
    {
        transform.position = currentPosition;

        Ray ray = new Ray(origin.position /*+ (body.right * footSpacing)*/, Vector3.down);
        if(Physics.Raycast(ray, out RaycastHit info, 10, groundLayer.value))
        {
            if(Vector3.Distance(newPosition, info.point) > stepDistance)
            {
                lerp = 0;
                newPosition = info.point;
                if(audioSource != null)
                {
                    audioSource.Play();
                }
            }
        }
        if(lerp < 1)
        {
            
            Vector3 footPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            footPosition.y += Mathf.Sin((lerp * Mathf.PI) * stepHeight);

            currentPosition = footPosition;
            lerp += Time.deltaTime * speed;
        }
        else
        {
            
            oldPosition = newPosition;
        }
    }
}
