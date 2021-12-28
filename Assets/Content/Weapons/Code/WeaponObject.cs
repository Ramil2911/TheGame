using UnityEngine;

[CreateAssetMenu(menuName = "Create WeaponObject", fileName = "Weapon", order = 51)]
public class WeaponObject : InteractableObject
{
    [SerializeField] private int maxAmmoAmount = 10;
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private float reloadTime = 5;
    [SerializeField] private float shootingRate = 5; //Shots per second
    [SerializeField] private float bulletSpeed = 100f;
    public float damage = 10f;
    
    public int MaxAmmoAmount => maxAmmoAmount;
    public WeaponType WeaponType => weaponType;
    public float ReloadTime => reloadTime;
    public float ShootingRate => shootingRate;
    public float BulletSpeed => bulletSpeed;

    public AmmoType requiredAmmoType;
    
    [SerializeField] public int ammoAmount;
}

public enum WeaponType
{
    Auto,
    SemiAuto
}
