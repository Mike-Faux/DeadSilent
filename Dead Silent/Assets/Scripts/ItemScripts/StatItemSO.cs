using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StatItemSO : ItemSO
{


    public enum StatToChange
    {
        none,
        health
    };

    public StatToChange statToChange;
    public int amountToChangeStatAttribute;
    public int amountToChangeStat;

    public new bool UseItem()
    {
        return false;
    }
}
