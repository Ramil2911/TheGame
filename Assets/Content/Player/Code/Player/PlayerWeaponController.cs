using System;
using UnityEngine;

[Obsolete]
public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private GameObject currentWeapon;

    [SerializeField] private Vector3 spawnPosition;

    private GameObject _currentSpawnedWeapon;
    private GameObject _camera;


    private void Start()
    {
        _camera = this.GetComponent<TheFirstPerson.FPSController>().cam.gameObject;
    }

    private void Update()
    {
        if (_currentSpawnedWeapon is null)
        {
            //_currentSpawnedWeapon = Instantiate(currentWeapon, _camera.transform.position + spawnPosition + _camera.transform.right + _camera.transform.up * -0.6f, Quaternion.Euler(0, -90, 0), _camera.transform);
        }
    }
}
