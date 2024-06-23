using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class ItemSO : ScriptableObject
{
    public ItemType Type;
    public string Name;
    public string Description;
    public int StackSize;
    public GameObject Prefab;

    public bool UseItem()
    {
        if(Type != ItemType.Stats) return false;
        else return((StatItemSO)this).UseItem();
    }

    public enum ItemType
    {
        Ammo,
        Stats,
        Key
    }
 }
