using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MazeSettings : MonoBehaviour
{
    public Material wallMat;
    public Material floorMat;
    public Material postMat;

    [SerializeField] List<GameObject> OtherSpawnables = new List<GameObject>();
    [SerializeField] GameObject Prefab_EnemySpawner;

    public int EnemyWeight;
    public int OtherWeight;
    public int EmptyWeight;



    public GameObject GetMainSpawnable()
    {
        int total = EnemyWeight + OtherWeight + EmptyWeight;

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
        }
        else
        {
            //Do nothing
        }

        return null;
    }


}
