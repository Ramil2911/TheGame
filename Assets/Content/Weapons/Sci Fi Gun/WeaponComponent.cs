using System;
using Mirror;
using UnityEngine;

public class WeaponComponent : MonoBehaviour
{
    //Functionality
    [SerializeField] private int ammoAmount;
    [SerializeField] private GameObject bulletSource;
    [SerializeField] private GameObject bulletProjectile;
    //Functionality
    
    
    //Weapon properties
    [SerializeField] private WeaponObject weapon;
    //Weapon properties
    
    private float _shootTimer;
    
    
    void Start()
    {
        ammoAmount = weapon.MaxAmmoAmount;
        
    }

    
    void Update()
    {
        if(_shootTimer >= 0)
            _shootTimer -= Time.deltaTime * weapon.ShootingRate;
     
        //Shoot
        if (ammoAmount <= 0 || !(_shootTimer <= 0)) return;
        switch (weapon.WeaponType)
        {
            case WeaponType.Auto:
            {
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    _shootTimer = 1;
                    CmdShoot();
                    ammoAmount -= 1;
                }

                break;
            }
            case WeaponType.SemiAuto:
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    _shootTimer = 1;
                    CmdShoot();
                    ammoAmount -= 1;
                }

                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    //TODO: Adapt for online
    void CmdShoot()
    {
        var rotation = this.transform.rotation;
        var body = Instantiate(bulletProjectile, bulletSource.transform.position, rotation).GetComponent<Rigidbody>();
        body.velocity = rotation * Vector3.right * weapon.BulletSpeed;
    }
}
