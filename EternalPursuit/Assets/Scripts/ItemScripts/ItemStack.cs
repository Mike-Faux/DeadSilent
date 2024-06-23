using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStack 
{
    public ItemSO item;
    public int count;

    public ItemStack(ItemSO item, int count)
    {
        this.item = item;
        this.count = count;
    }
}
