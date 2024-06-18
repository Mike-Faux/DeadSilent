using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StatItemSO : ItemSO
{


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

    public StatToChange statToChange;
    public AttributesToChange attributesToChange;
    public int amountToChangeStatAttribute;
    public int amountToChangeStat;

    public new bool UseItem()
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
}
