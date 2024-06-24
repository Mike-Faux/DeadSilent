using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponStats : ScriptableObject
{
   
    public string Name;
    public int Damage;
    public int BulletSpeed;
    public int Ammo_Capacity;
    public ItemSO Ammo_Type;
    public float FireRate;
    public int shootDist;
    public float hitChance;
    public float MaxRange = 1000f;
    public float ReloadTime;
    public bool Silenced;

    public float SoundRadius;
}
