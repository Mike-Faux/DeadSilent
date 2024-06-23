using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    public float delay = 0.2f;

    public GameObject cube;
    private Camera mainCamera;
    [SerializeField] float distanceFromCamera;
    [SerializeField] float heightOffset;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        InvokeRepeating("Spawn",  delay, delay);
    }
    void Spawn()
    {
        Vector3 spawnPosition = mainCamera.transform.position + mainCamera.transform.forward * distanceFromCamera;
        spawnPosition += new Vector3(Random.Range(-8, 8), heightOffset + Random.Range(0, 0), 0);
        Instantiate(cube, spawnPosition, Quaternion.identity);
    }

        // Update is called once per frame
        void Update()
    {
        
    }
}
