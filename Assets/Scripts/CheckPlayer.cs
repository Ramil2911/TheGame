using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CheckPlayer : MonoBehaviour
{
    public EnemyPathfinding enemyPathfinding;
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            enemyPathfinding.playerIsNear = true;
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if(other.CompareTag("Player"))
        {
            enemyPathfinding.playerIsNear = false;
        }
    }
}
