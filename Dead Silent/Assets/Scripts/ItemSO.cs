using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public StatToChange statToChange = new StatToChange();
    public AttributesToChange attributesToChange = new AttributesToChange();
    public int amountToChangeStatAttribute;
    public int amountToChangeStat;

    public bool UseItem()
    {
        if (statToChange == StatToChange.ammo)
        {
            FireArm m4FireArm = GameObject.Find("M4 Assault Rifle").GetComponent<FireArm>();
            if (m4FireArm.Ammo == m4FireArm.Stats.Ammo_Capacity)
            {
                return false;
            }
            else
            {
                m4FireArm.ChangeAmmo(amountToChangeStat);
                return true;
            }
                
        }
        return false;
    }


    public enum StatToChange
    {
        none,
        health,
        mana,
        ammo,
        stamina
    };
    public enum AttributesToChange
    {
        none,
        strength,
        intelligence,
        agility
    };
 }
