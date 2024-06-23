using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    [SerializeField] int size = 20;

    List<ItemStack> items = new List<ItemStack>();


    public int AddItems(ItemStack item)
    {
        if (items.Count >= size)
            return item.count;



        return 0;
    }




}
