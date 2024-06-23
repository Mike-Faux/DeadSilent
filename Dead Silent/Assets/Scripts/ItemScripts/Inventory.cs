using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    [SerializeField] int size = 20;

    List<ItemStack> items = new();


    public int AddItems(ItemStack item)
    {

        foreach(ItemStack i in items)
        {
            if (i.item == item.item && i.count < i.item.StackSize)
            {
                i.count += item.count;
                if(i.count > i.item.StackSize)
                {
                    item.count = i.count - i.item.StackSize;
                    i.count = i.item.StackSize;
                }
                else
                {
                    return 0;
                }
            }
        }


        while(item.count >= item.item.StackSize && items.Count < size)
        {
            item.count -= item.item.StackSize;
            items.Add(new ItemStack(item.item, item.item.StackSize));
        }

        if (items.Count >= size)
            return item.count;


        items.Add(item);

        return 0;
    }



    public int GetItemCount(ItemSO item)
    {
        int total = 0;

        foreach (ItemStack i in items)
        {
            if(i.item == item) total += i.count;
        }

        return total;
    }

    public int RemoveItems(ItemSO item, int count)
    {
        int removed = 0;
        foreach (ItemStack i in items)
        {
            if (i.item == item)
            {
                count -= i.count;
                removed += i.count;

                if(count < 0)
                {
                    i.count = -count;
                    removed -= i.count;
                    return removed;
                }
                else
                {
                    items.Remove(i);
                }
            }
        }

        return removed;
    }
}
