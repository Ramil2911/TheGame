using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetEndShoot : MonoBehaviour
{
    WeaponController weaponController;
    // Start is called before the first frame update
    void Start()
    {
        weaponController = GetComponentInParent<WeaponController>();
    }
    public void IShoot()
    {
        weaponController.doIShoot = true;
    }
    public void EndShoot()
    {
        weaponController.doIShoot = false;
    }
}
