using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class FightManager : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private int enemyPercent = 50;
    private bool _wasAlreadyIn = false;
    private BoxCollider box;

    public MusicManager musicManager;
    private bool isISwitchMusic = false;
    private void Start()
    {
        box = GetComponent<BoxCollider>();
        musicManager = GameObject.Find("MusicManager").GetComponent<MusicManager>();
    }
    void FixedUpdate() 
    {
        if (_wasAlreadyIn)
        {
            CheckForFightEnd();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!_wasAlreadyIn)
        {
            if (other.CompareTag("Player"))
            {
                CloseDoors();
                SpawnEnemies();
                _wasAlreadyIn = true;
                Destroy(box);
            }
        }
    }

    private void CloseDoors()
    {
        Door[] doors = transform.Find("Doors").GetComponentsInChildren<Door>();
        musicManager.isBattleMod = true;
        isISwitchMusic = true;
        foreach (Door door in doors)
        {
            door.Close();
            door.Lock();
        }
    }

    private void SpawnEnemies()
    {
        Transform enemiesPositions = transform.Find("EnemyPositions");
        for (int i = 0; i < enemiesPositions.childCount; i++)
        {
            int roll = Random.Range(1, 101);
            if (roll <= enemyPercent)
            {
                int enemyIndex = Random.Range(0, enemyPrefabs.Length);
                Instantiate(enemyPrefabs[enemyIndex],  enemiesPositions.GetChild(i).position + new Vector3(0, 3f, 0), Quaternion.identity, transform.Find("Enemies").transform);
            }
        }
        CheckForFightEnd();
    }


    public void CheckForFightEnd()
    {
        
        if (transform.Find("Enemies").childCount <= 0)
        {
            if(isISwitchMusic == true)
            {
                musicManager.isBattleMod = false;
                isISwitchMusic = false;
            }
            FightEndSequence();
        }
    }

    private void FightEndSequence()
    {
        OpenDoors();
    }

    private void OpenDoors()
    {
        Door[] doors = transform.Find("Doors").GetComponentsInChildren<Door>();
        foreach (Door door in doors)
        {
            door.Open();
            door.Unlock();
        }
        
    }
}
