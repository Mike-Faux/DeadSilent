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
    public float FireRate;
    public float ReloadTime;
    public bool Silenced;

    public float SoundRadius;
}
