using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPathfinding : MonoBehaviour
{
    [SerializeField] private Transform _player;
    private NavMeshAgent _agent;
    [SerializeField] private float speed = 2;
    public float distanceReaction = 10;
    public bool canAttack = false;
    public bool playerIsNear = false;
    private void Start()
    {
        
        _player = GameObject.FindWithTag("Player").transform;
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        Pathfinding();
    }

    private void Pathfinding()
    {
        if (playerIsNear == false)
        {
            _agent.SetDestination(_player.position);
            _agent.speed = speed;
            _agent.angularSpeed = 120f;
            canAttack = false;
        }
        else
        {
            CheckVisibility();
        }

    }

    private void CheckVisibility()
    {
        if (Physics.Linecast(transform.position, _player.position) && Vector3.Distance(transform.position, _player.position) <= distanceReaction)
        {
            Debug.DrawLine(transform.position, _player.position);
            _agent.speed = 0.01f;
            _agent.angularSpeed = 360f;
            canAttack = true;
        }
        else
        {
            Debug.DrawLine(transform.position, _player.position);
            canAttack = false;
        }
    }
}