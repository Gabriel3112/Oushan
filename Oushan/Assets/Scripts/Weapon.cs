using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Weapon",menuName = "ScriptableObjects/Weapon" ,order = 1)]
public class Weapon : ScriptableObject
{
    public string nameWeapon;
    public int bulletsInTheComb;
    public int bulletsInTheInitialComb;
    public int combs;
    public int combsInTheIventory;
    public float firingRate;
    public float damage;
    public AudioSource shootingSound;
    public GameObject weaponPrefab;
}
