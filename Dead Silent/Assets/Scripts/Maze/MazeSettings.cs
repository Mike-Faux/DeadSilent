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

        if(id < EnemyWeight)
        {
            //Spawn Enemy
        }else if(id < EnemyWeight + OtherWeight)
        {
            //Spawn Other
        }
        else
        {
            //Do nothing
        }

        return null;
    }


}