using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MazeSettings : MonoBehaviour
{
    public Material wallMat;
    public Material floorMat;
    public Material postMat;

    [SerializeField] GameObject Pickup_Prefab;
    [SerializeField] List<ItemSO> ItemSpawnables = new List<ItemSO>();
    [SerializeField] List<GameObject> OtherSpawnables = new List<GameObject>();
    [SerializeField] GameObject Prefab_EnemySpawner;

    public int EnemyWeight;
    public int OtherWeight;
    public int ItemWeight;
    public int EmptyWeight;



    public GameObject GetMainSpawnable()
    {
        int total = EnemyWeight + OtherWeight + ItemWeight + EmptyWeight;

        int id = Random.Range(0, total);


        //return Instantiate(Prefab_EnemySpawner);

        if (id < EnemyWeight)
        {
            //Spawn Enemy
            return Instantiate(Prefab_EnemySpawner);
        }else if(id < EnemyWeight + OtherWeight)
        {
            if (OtherSpawnables.Count == 0) return null;
            //Spawn Other
            int selection = Random.Range(0, OtherSpawnables.Count);
            return Instantiate(OtherSpawnables[selection]);
        }else if(id < EnemyWeight + OtherWeight + ItemWeight)
        {
            if (ItemSpawnables.Count == 0) return null;
            //Spawn Item
            GameObject pickup = Instantiate(Pickup_Prefab);
            Pickup p = pickup.GetComponent<Pickup>();

            int selection = Random.Range(0, ItemSpawnables.Count);
            ItemStack items = new ItemStack(ItemSpawnables[selection], ItemSpawnables[selection].StackSize);
            p.SetItems(items);

            return pickup;
        }
        else
        {
            //Do nothing
        }

        return null;
    }


}
