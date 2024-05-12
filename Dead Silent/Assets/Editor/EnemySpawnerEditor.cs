using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEngine.Experimental.AI;
using UnityEngine.AI;
using Unity.VisualScripting.YamlDotNet.Core;
using Unity.VisualScripting;
using UnityEngine.UIElements;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
    private void OnEnable()
    {
        EnemySpawner spawner = (EnemySpawner)target;

        if (spawner.transform.childCount == 0)
        {
            spawner.PatrolPath.Clear();
            return;
        }

        if (spawner.transform.childCount != spawner.PatrolPath.Count)
        {
            UpdatePatrolPath(spawner);
        }
    }

    public override void OnInspectorGUI()
    {
        EnemySpawner spawner = (EnemySpawner)target;

        if (spawner.transform.childCount == 0)
        {
            spawner.PatrolPath.Clear();
            return;
        }

        if (spawner.transform.childCount != spawner.PatrolPath.Count)
        {
            UpdatePatrolPath(spawner);
        }

        base.OnInspectorGUI();
    }

    private void OnSceneGUI()
    {
        if (!Application.isPlaying)
        {
            int s = 0;

            EnemySpawner spawner = (EnemySpawner)target;
            Vector3 current = Vector3.zero;
            Vector3 previous;
            Handles.color = Color.red;

            if (spawner.SpawnOnFirstPoint && spawner.PatrolPath.Count > 0) 
            {
                previous = spawner.PatrolPath[0].transform.position;
                s++;
            }
            else
            {
                previous = spawner.transform.position;
            }

            
        
            Handles.DrawWireArc(previous, Vector3.up, Vector3.forward, 360f, 2);

            for(int i = s; i < spawner.PatrolPath.Count; i++)
            {
                current = spawner.PatrolPath[i].transform.position;
                Handles.DrawWireArc(current, Vector3.up, Vector3.forward, 360f, 2);
                Handles.DrawLine(previous, current);

                previous = current;
            }

            current = spawner.PatrolPath[0].transform.position;
            Handles.DrawWireArc(current, Vector3.up, Vector3.forward, 360f, 2);
            Handles.DrawLine(previous, current);
        }
        
    }

    private void UpdatePatrolPath(EnemySpawner spawner)
    {
        if(spawner.transform.childCount > spawner.PatrolPath.Count)
        {
            for(int i = 0; i < spawner.transform.childCount; i++)
            {
                Transform child = spawner.transform.GetChild(i);
                if (child.TryGetComponent(out PatrolWaypoint waypoint))
                {
                    if (spawner.PatrolPath.Contains(waypoint)) continue;
                    else
                    {
                        spawner.PatrolPath.Add(waypoint);
                    }
                }
                else
                {
                    child.parent = spawner.transform.parent;
                }
            }
        }
        else
        {
            for (int i = 0; i < spawner.PatrolPath.Count; i++)
            {
                if (spawner.PatrolPath[i].transform.parent == spawner.transform) 
                    continue;
                else
                {
                    spawner.PatrolPath[i].transform.parent = spawner.transform;
                }
            }
        }

        if (spawner.transform.childCount != spawner.PatrolPath.Count)
        {
            //UpdatePatrolPath(spawner);
        }
    }
}
